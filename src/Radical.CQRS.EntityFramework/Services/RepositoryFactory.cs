using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using Topics.Radical.Reflection;
using Newtonsoft.Json;
using Radical.CQRS.Reflection;

namespace Radical.CQRS.Services
{
    class RepositoryFactory : IRepositoryFactory
    {
        class Repository : IRepository
        {
            public void Dispose()
            {
                this.session.Dispose();

                this.aggregateTracking.Clear();
            }

            readonly HashSet<IAggregate> aggregateTracking = new HashSet<IAggregate>();
            readonly Guid txId;
            readonly DbContext session;

            public Repository(DbContext session)
            {
                this.txId = Guid.NewGuid();
                this.session = session;
            }

            public void Add<TAggregate>(TAggregate aggregate) where TAggregate : class, IAggregate
            {
                var db = this.session.Set<TAggregate>();
                db.Add(aggregate);
                this.TrackIfRequired(aggregate);
            }

            public void CommitChanges()
            {
                try
                {
                    var db = this.session.Set<DomainEventCommit>();
                    var commits = this.aggregateTracking
                        .Where(a => a.IsChanged)
                        .Select(aggregate => new
                        {
                            Aggregate = aggregate,
                            Commits = aggregate.GetUncommittedEvents().Select(e => new DomainEventCommit()
                            {
                                EventId = e.Id,
                                AggregateId = aggregate.Id,
                                TransactionId = this.txId,
                                PublishedOn = e.OccurredAt,
                                EventType = ConcreteProxyCreator.GetValidTypeName(e.GetType()),
                                EventBlob = JsonConvert.SerializeObject(e),
                                Version = e.AggregateVersion

                            })
                        })
                        .SelectMany(a => a.Commits)
                        .ToArray()
                        .ForEach(temp =>
                        {
                            db.Add(temp);
                        });

                    this.session.SaveChanges();

                    this.aggregateTracking.ForEach(a => a.ClearUncommittedEvents());
                    this.aggregateTracking.Clear();

                }
                catch (Exception)
                {
                    //TODO: log
                    throw;
                }
            }

            void TrackIfRequired(IAggregate aggregate)
            {
                if (!this.aggregateTracking.Contains(aggregate))
                {
                    this.aggregateTracking.Add(aggregate);
                }
            }

            public TAggregate GetById<TAggregate>(Guid aggregateId) where TAggregate : class, IAggregate
            {
                var db = this.session.Set<TAggregate>();
                var aggregate = db.Where(a => a.Id == aggregateId).Single();
                this.TrackIfRequired(aggregate);

                return aggregate;
            }

            public IEnumerable<TAggregate> GetById<TAggregate>(params Guid[] aggregateIds) where TAggregate : class, IAggregate
            {
                var db = this.session.Set<TAggregate>();
                var aggregates = db.Where(a => aggregateIds.Contains(a.Id));
                foreach (var a in aggregates)
                {
                    this.TrackIfRequired(a);
                }

                return aggregates.AsEnumerable();
            }
        }

        readonly IDbContextFactory<Data.DomainContext> factory;

		public RepositoryFactory( IDbContextFactory<Data.DomainContext> factory )
        {
            this.factory = factory;
        }

        public IRepository OpenSession()
        {
            var db = this.factory.Create();
            return new Repository(db);
        }
    }
}

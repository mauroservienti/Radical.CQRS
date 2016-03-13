using System;
using System.Collections.Generic;
using System.Linq;
using Topics.Radical.Linq;
using Topics.Radical.Reflection;
using System.Threading.Tasks;
using System.Reflection;

namespace Radical.CQRS
{
    public abstract class AbstractSyncRepository<TSession, TCommit>
        : AbstractRepository<TSession, TCommit>,
        IRepository
        where TCommit : DomainEventCommit, new()
    {
        protected AbstractSyncRepository(TSession session, IAggregateFinderProvider<TSession> aggregateFinderProvider, IAggregateStateFinderProvider<TSession> aggregateStateFinderProvider)
            : base(session, aggregateFinderProvider, aggregateStateFinderProvider)
        {

        }

        public void CommitChanges()
        {
            var commits = this.AggregateTracking
                    .Where(a => a.IsChanged)
                    .Select(aggregate => new
                    {
                        Aggregate = aggregate,
                        Commits = aggregate.GetUncommittedEvents().Select(e => this.FillDomainEventCommit(e, aggregate))
                    })
                    .SelectMany(a => a.Commits)
                    .ToArray();

            this.OnAdd(commits);
            this.OnCommitChanges();

            this.AggregateTracking.ForEach(a => a.ClearUncommittedEvents());
            this.AggregateTracking.Clear();
        }

        protected abstract void OnCommitChanges();

        static bool TryGetAggregateStateType<TAggregate>(out Type iAggregateStateType)
        {
            iAggregateStateType = typeof(TAggregate)
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericArguments().All(p => p.Is<IAggregateState>()))
                .Select(i => i.GetGenericArguments().Single())
                .SingleOrDefault();

            return iAggregateStateType != null;
        }

        protected virtual TAggregate CreateAggregateInstance<TAggregate>(IAggregateState state)
        {
            var aggregateType = typeof(TAggregate);
            var stateCtor = aggregateType
                .GetConstructors(bindingAttr: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .SingleOrDefault(c => c.GetParameters().All(p => p.ParameterType == state.GetType()));

            if(stateCtor != null)
            {
                var aggregate = stateCtor.Invoke(new Object[] { state });

                return (TAggregate)aggregate;
            }
            else
            {
                var aggregateInstance = (IHaveState)Activator.CreateInstance(typeof(TAggregate), true);
                aggregateInstance.AcceptState(state);

                return (TAggregate)aggregateInstance;
            }
        }

        public virtual TAggregate GetById<TAggregate>(Guid aggregateId) where TAggregate : class, IAggregate
        {
            return this.GetById<TAggregate>(new AggregateQuery(aggregateId));
        }

        public virtual IEnumerable<TAggregate> GetById<TAggregate>(params Guid[] aggregateIds)
            where TAggregate : class, IAggregate
        {
            return this.GetById<TAggregate>( aggregateIds.Select(id=> new AggregateQuery(id)).ToArray());
        }

        public virtual TAggregate GetById<TAggregate>(AggregateQuery aggregateQuery) where TAggregate : class, IAggregate
        {
            TAggregate result = null;
            Type iAggregateStateType;
            if(TryGetAggregateStateType<TAggregate>(out iAggregateStateType))
            {
                var state = this.GetAggregateStateById(iAggregateStateType, aggregateQuery);
                result = this.CreateAggregateInstance<TAggregate>(state);
            }
            else
            {
                result = this.GetAggregateById<TAggregate>(aggregateQuery);
            }

            this.TrackIfRequired(result);

            return result;
        }

        public virtual IEnumerable<TAggregate> GetById<TAggregate>(params AggregateQuery[] aggregateQueries) where TAggregate : class, IAggregate
        {
            IEnumerable<TAggregate> results = null;
            Type iAggregateStateType;
            if(TryGetAggregateStateType<TAggregate>(out iAggregateStateType))
            {
                var states = this.GetAggregateStateById(iAggregateStateType, aggregateQueries);

                results = states.Select(state => this.CreateAggregateInstance<TAggregate>(state));
            }
            else
            {
                results = this.GetAggregateById<TAggregate>(aggregateQueries);
            }

            foreach(var a in results)
            {
                this.TrackIfRequired(a);
            }

            return results;
        }

        protected virtual TAggregate GetAggregateById<TAggregate>(AggregateQuery aggregateQuery) where TAggregate : class, IAggregate
        {
            var finder = this.aggregateFinderProvider.GetFinder<TAggregate>();
            if(finder != null)
            {
                return finder.FindById(this.session, aggregateQuery);
            }

            var defaultFinder = this.aggregateFinderProvider.GetDefaultFinder();
            return defaultFinder.FindById<TAggregate>(this.session, aggregateQuery);
        }

        protected virtual IAggregateState GetAggregateStateById(Type stateType, AggregateQuery aggregateQuery)
        {
            var finder = this.aggregateStateFinderProvider.GetFinder(stateType);
            if(finder != null)
            {
                return finder.FindById(this.session, aggregateQuery);
            }

            var defaultFinder = this.aggregateStateFinderProvider.GetDefaultFinder();
            return defaultFinder.FindById(this.session, stateType, aggregateQuery);
        }

        protected virtual IEnumerable<TAggregate> GetAggregateById<TAggregate>(params AggregateQuery[] aggregateQueries) where TAggregate : class, IAggregate
        {
            var finder = this.aggregateFinderProvider.GetFinder<TAggregate>();
            if(finder != null)
            {
                return finder.FindById(this.session, aggregateQueries);
            }

            var defaultFinder = this.aggregateFinderProvider.GetDefaultFinder();
            return defaultFinder.FindById<TAggregate>(this.session, aggregateQueries);
        }

        protected virtual IEnumerable<IAggregateState> GetAggregateStateById(Type stateType, params AggregateQuery[] aggregateQueries)
        {
            var finder = this.aggregateStateFinderProvider.GetFinder(stateType);
            if(finder != null)
            {
                return finder.FindById(this.session, aggregateQueries);
            }

            var defaultFinder = this.aggregateStateFinderProvider.GetDefaultFinder();
            return defaultFinder.FindById(this.session, stateType, aggregateQueries);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Newtonsoft.Json;
using Radical.CQRS.Reflection;
using Topics.Radical.Linq;
using System.Threading.Tasks;

namespace Radical.CQRS.Runtime
{
	class SyncRepository : AbstractSyncRepository
	{
		public override void Dispose()
		{
			this._session.Dispose();
		}

		readonly DbContext _session;

		public SyncRepository( DbContext session )
		{
			this._session = session;
		}

		public override void Add<TAggregate>( TAggregate aggregate )
		{
			try
			{
				var db = this._session.Set<TAggregate>();
				db.Add( aggregate );
				this.TrackIfRequired( aggregate );
			}
			catch( Exception ex )
			{
				//TODO: log
				throw;
			}
		}

		public override void CommitChanges()
		{
			try
			{
				var db = this._session.Set<DomainEventCommit>();

				this.AggregateTracking
					.Where( a => a.IsChanged )
					.Select( aggregate => new
					{
						Aggregate = aggregate,
						Commits = aggregate.GetUncommittedEvents().Select( e => new DomainEventCommit()
						{
							EventId = e.Id,
							AggregateId = aggregate.Id,
							TransactionId = this.TransactionId,
							PublishedOn = e.OccurredAt,
							EventType = ConcreteProxyCreator.GetValidTypeName( e.GetType() ),
							EventBlob = JsonConvert.SerializeObject( e ),
							Version = e.AggregateVersion
						} )
					} )
					.SelectMany( a => a.Commits )
					.ToArray()
					.ForEach( temp =>
					{
						db.Add( temp );
					} );

				this._session.SaveChanges();

				this.AggregateTracking.ForEach( a => a.ClearUncommittedEvents() );
				this.AggregateTracking.Clear();

			}
			catch( Exception ex )
			{
				//TODO: log
				throw;
			}
		}

		public override TAggregate GetById<TAggregate>( Guid aggregateId )
		{
			var db = this._session.Set<TAggregate>();
			var aggregate = db.Single( a => a.Id == aggregateId );
			this.TrackIfRequired( aggregate );

			return aggregate;
		}

		public override IEnumerable<TAggregate> GetById<TAggregate>( params Guid[] aggregateIds )
		{
			var db = this._session.Set<TAggregate>();
			var aggregates = db.Where( a => aggregateIds.Contains( a.Id ) );
			foreach( var a in aggregates )
			{
				this.TrackIfRequired( a );
			}

			return aggregates.AsEnumerable();
		}
	}
}

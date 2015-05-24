using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Newtonsoft.Json;
using Radical.CQRS.Reflection;
using Topics.Radical.Linq;
using System.Threading.Tasks;
using Radical.CQRS.Services;

namespace Radical.CQRS.Runtime
{
	class SyncRepository : AbstractSyncRepository
	{
		public override void Dispose()
		{
			this._session.Dispose();
		}

		readonly AggregateLoaderProvider aggregateLoaderProvider;
		readonly DbContext _session;

		public SyncRepository( DbContext session, AggregateLoaderProvider aggregateLoaderProvider )
		{
			this.aggregateLoaderProvider = aggregateLoaderProvider;
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
						Commits = aggregate.GetUncommittedEvents().Select( e => new EntityFrameworkDomainEventCommit()
						{
							EventId = e.Id,
							AggregateId = aggregate.Id,
							Version = e.AggregateVersion,
							TransactionId = this.TransactionId,
							PublishedOn = e.OccurredAt,
							Event = e,
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
			TAggregate aggregate = null;
			var loader = this.aggregateLoaderProvider.GetLoader<TAggregate>();
			if( loader != null )
			{
				aggregate = loader.GetById( this._session, aggregateId );
			}
			else
			{
				var db = this._session.Set<TAggregate>();
				aggregate = db.Single( a => a.Id == aggregateId );
			}

			this.TrackIfRequired( aggregate );

			return aggregate;
		}

		public override IEnumerable<TAggregate> GetById<TAggregate>( params Guid[] aggregateIds )
		{
			IEnumerable<TAggregate> results = null;
			var loader = this.aggregateLoaderProvider.GetLoader<TAggregate>();
			if( loader != null )
			{
				results = loader.GetById( this._session, aggregateIds );
			}
			else
			{
				var db = this._session.Set<TAggregate>();
				results = db.Where( a => aggregateIds.Contains( a.Id ) )
					.ToList();
			}

			foreach( var a in results )
			{
				this.TrackIfRequired( a );
			}

			return results;
		}
	}
}

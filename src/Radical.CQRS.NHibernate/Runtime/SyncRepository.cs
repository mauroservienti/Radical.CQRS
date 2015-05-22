using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Radical.CQRS.Reflection;
using Topics.Radical.Linq;
using System.Threading.Tasks;
using Radical.CQRS.Services;
using NHibernate;

namespace Radical.CQRS.Runtime
{
	class SyncRepository : AbstractSyncRepository
	{
		public override void Dispose()
		{
			this._session.Dispose();
		}

		readonly AggregateLoaderProvider aggregateLoaderProvider;
		readonly ISession _session;

		public SyncRepository( ISession session, AggregateLoaderProvider aggregateLoaderProvider )
		{
			this.aggregateLoaderProvider = aggregateLoaderProvider;
			this._session = session;
		}

		public override void Add<TAggregate>( TAggregate aggregate )
		{
			try
			{
				this._session.Save( aggregate );
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
						this._session.Save( temp );
					} );

				this._session.Flush();

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
				aggregate = this._session.Load<TAggregate>( aggregateId );
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
				results = this._session.QueryOver<TAggregate>()
					.WhereRestrictionOn(a => a.Id)
					.IsIn( aggregateIds )
					.List();
			}

			foreach( var a in results )
			{
				this.TrackIfRequired( a );
			}

			return results;
		}
	}
}

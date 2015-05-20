//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using Newtonsoft.Json;
//using Radical.CQRS.Reflection;
//using Topics.Radical.Linq;
//using System.Threading.Tasks;
//using Radical.CQRS.Services;

//namespace Radical.CQRS.Runtime
//{
//	class AsyncRepository : AbstractAsyncRepository
//	{
//		public override void Dispose()
//		{
//			this._session.Dispose();
//		}

//		readonly AggregateLoaderProvider aggregateLoaderProvider;
//		readonly DbContext _session;

//		public AsyncRepository( DbContext session, AggregateLoaderProvider aggregateLoaderProvider )
//		{
//			this.aggregateLoaderProvider = aggregateLoaderProvider;
//			this._session = session;
//		}

//		public override void Add<TAggregate>( TAggregate aggregate )
//		{
//			try
//			{
//				var db = this._session.Set<TAggregate>();
//				db.Add( aggregate );
//				this.TrackIfRequired( aggregate );
//			}
//			catch( Exception ex )
//			{
//				//TODO: log
//				throw;
//			}
//		}

//		public override async Task CommitChangesAsync()
//		{
//			try
//			{
//				var db = this._session.Set<DomainEventCommit>();

//				this.AggregateTracking
//					.Where( a => a.IsChanged )
//					.Select( aggregate => new
//					{
//						Aggregate = aggregate,
//						Commits = aggregate.GetUncommittedEvents().Select( e => new DomainEventCommit()
//						{
//							EventId = e.Id,
//							AggregateId = aggregate.Id,
//							TransactionId = this.TransactionId,
//							PublishedOn = e.OccurredAt,
//							EventType = ConcreteProxyCreator.GetValidTypeName( e.GetType() ),
//							EventBlob = JsonConvert.SerializeObject( e ),
//							Version = e.AggregateVersion
//						} )
//					} )
//					.SelectMany( a => a.Commits )
//					.ToArray()
//					.ForEach( temp =>
//					{
//						db.Add( temp );
//					} );

//				await this._session.SaveChangesAsync();

//				this.AggregateTracking.ForEach( a => a.ClearUncommittedEvents() );
//				this.AggregateTracking.Clear();

//			}
//			catch( Exception ex )
//			{
//				//TODO: log
//				throw;
//			}
//		}

//		public override async Task<TAggregate> GetByIdAsync<TAggregate>( Guid aggregateId )
//		{
//			TAggregate aggregate = null;
//			var loader = this.aggregateLoaderProvider.GetAsyncLoader<TAggregate>();
//			if( loader != null )
//			{
//				aggregate = await loader.GetByIdAsync( this._session, aggregateId );
//			}
//			else
//			{
//				var db = this._session.Set<TAggregate>();
//				aggregate = await db.SingleAsync( a => a.Id == aggregateId );
//			}

//			this.TrackIfRequired( aggregate );

//			return aggregate;
//		}

//		public override async Task<IEnumerable<TAggregate>> GetByIdAsync<TAggregate>( params Guid[] aggregateIds )
//		{
//			IEnumerable<TAggregate> results = null;
//			var loader = this.aggregateLoaderProvider.GetAsyncLoader<TAggregate>();
//			if( loader != null )
//			{
//				results = await loader.GetByIdAsync( this._session, aggregateIds );
//			}
//			else 
//			{
//				var db = this._session.Set<TAggregate>();
//				results = await db.Where( a => aggregateIds.Contains( a.Id ) )
//					.ToListAsync();
//			}

//			foreach( var a in results )
//			{
//				this.TrackIfRequired( a );
//			}

//			return results;
//		}
//	}
//}

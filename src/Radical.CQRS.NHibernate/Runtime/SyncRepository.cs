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
	class SyncRepository : AbstractSyncRepository<ISession, NHibernateDomainEventCommit>
	{
		public override void Dispose()
		{
			base.Dispose();

			this._transaction.Dispose();
			this.session.Dispose();
		}

		readonly ITransaction _transaction;

		public SyncRepository( ISession session, IAggregateFinderProvider<ISession> aggregateFinderProvider, IAggregateStateFinderProvider<ISession> aggregateStateFinderProvider )
			: base(session, aggregateFinderProvider, aggregateStateFinderProvider)
		{
			this._transaction = session.BeginTransaction();
		}

		protected override void OnCommitChanges()
		{
			this._transaction.Commit();
			this.session.Close();
		}

		protected override void OnAdd( object aggregateOrState )
		{
			this.session.Save( aggregateOrState );
		}

		protected override void OnAdd( IEnumerable<NHibernateDomainEventCommit> commits )
		{
			foreach( var commit in commits )
			{
				this.session.Save( commit );
			}
		}


		//public override TAggregate GetById<TAggregate>( Guid aggregateId )
		//{
		//	TAggregate aggregate = null;
		//	var loader = this.aggregateLoaderProvider.GetLoader<TAggregate>();
		//	if( loader != null )
		//	{
		//		aggregate = loader.GetById( this._session, aggregateId );
		//	}
		//	else
		//	{
		//		aggregate = this._session.Load<TAggregate>( aggregateId );
		//	}

		//	this.TrackIfRequired( aggregate );

		//	return aggregate;
		//}

		//public override IEnumerable<TAggregate> GetById<TAggregate>( params Guid[] aggregateIds )
		//{
		//	IEnumerable<TAggregate> results = null;
		//	var loader = this.aggregateLoaderProvider.GetLoader<TAggregate>();
		//	if( loader != null )
		//	{
		//		results = loader.GetById( this._session, aggregateIds );
		//	}
		//	else
		//	{
		//		results = this._session.QueryOver<TAggregate>()
		//			.WhereRestrictionOn(a => a.Id)
		//			.IsIn( aggregateIds )
		//			.List();
		//	}

		//	foreach( var a in results )
		//	{
		//		this.TrackIfRequired( a );
		//	}

		//	return results;
		//}
	}
}

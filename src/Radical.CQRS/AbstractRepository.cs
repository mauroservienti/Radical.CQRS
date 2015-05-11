using System;
using System.Collections.Generic;
using System.Linq;

namespace Radical.CQRS
{
	public abstract class AbstractRepository : IRepository
	{
		public virtual void Dispose()
		{
			this.AggregateTracking.Clear();
		}

		protected readonly Guid TransactionId;
		protected AbstractRepository()
		{
			this.TransactionId = Guid.NewGuid();
		}

		protected readonly HashSet<IAggregate> AggregateTracking = new HashSet<IAggregate>();
		protected virtual void TrackIfRequired( IAggregate aggregate )
		{
			if( !this.AggregateTracking.Contains( aggregate ) )
			{
				this.AggregateTracking.Add( aggregate );
			}
		}

		public abstract void Add<TAggregate>(TAggregate aggregate) where TAggregate : class, IAggregate;

		public abstract void CommitChanges();

		public virtual TAggregate GetById<TAggregate>( Guid aggregateId ) where TAggregate : class, IAggregate
		{
			return this.GetById<TAggregate>(new[] {aggregateId}).Single();
		}

		public abstract IEnumerable<TAggregate> GetById<TAggregate>(params Guid[] aggregateIds)
			where TAggregate : class, IAggregate;
	}
}

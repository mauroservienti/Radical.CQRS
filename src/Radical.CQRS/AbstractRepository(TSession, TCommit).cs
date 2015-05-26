using System;
using System.Collections.Generic;
using System.Linq;
using Topics.Radical.Linq;
using Topics.Radical.Reflection;
using System.Threading.Tasks;

namespace Radical.CQRS
{
	public abstract class AbstractRepository<TSession, TCommit> where TCommit : DomainEventCommit, new()
	{
		protected readonly TSession session;
		protected readonly IAggregateFinderProvider<TSession> aggregateFinderProvider;
		protected readonly IAggregateStateFinderProvider<TSession> aggregateStateFinderProvider;

		public virtual void Dispose()
		{
			this.AggregateTracking.Clear();
		}

		protected readonly Guid TransactionId;
		protected AbstractRepository( TSession session, IAggregateFinderProvider<TSession> aggregateFinderProvider, IAggregateStateFinderProvider<TSession> aggregateStateFinderProvider )
		{
			this.TransactionId = Guid.NewGuid();

			this.session = session;
			this.aggregateFinderProvider = aggregateFinderProvider;
			this.aggregateStateFinderProvider = aggregateStateFinderProvider;
		}

		protected readonly HashSet<IAggregate> AggregateTracking = new HashSet<IAggregate>();

		protected virtual void TrackIfRequired( IAggregate aggregate )
		{
			if( !this.AggregateTracking.Contains( aggregate ) )
			{
				this.AggregateTracking.Add( aggregate );
			}
		}

		public virtual void Add<TAggregate>( TAggregate aggregate ) where TAggregate : class, IAggregate
		{
			try
			{
				if( aggregate is IHaveState )
				{
					var state = ( ( IHaveState )aggregate ).State;

					this.OnAggregateStateAdd( state );
				}
				else
				{
					this.OnAggregateAdd( aggregate );
				}

				this.TrackIfRequired( aggregate );
			}
			catch( Exception ex )
			{
				//TODO: log
				throw;
			}
		}

		protected virtual void OnAggregateStateAdd( IAggregateState state )
		{
			this.OnAdd( state );
		}

		protected virtual void OnAggregateAdd( IAggregate aggregate )
		{
			this.OnAdd( aggregate );
		}

		protected abstract void OnAdd( Object aggregateOrState );

		protected abstract void OnAdd( IEnumerable<TCommit> commits );

		protected virtual TCommit FillDomainEventCommit( IDomainEvent @event )
		{
			var commit = new TCommit();

			commit.EventId = @event.Id;
			commit.AggregateId = @event.AggregateId;
			commit.Version = @event.AggregateVersion;
			commit.TransactionId = this.TransactionId;
			commit.PublishedOn = @event.OccurredAt;
			commit.Event = @event;

			return commit;
		}
	}

	//public abstract class AbstractAsyncRepository : AbstractRepository, IAsyncRepository
	//{
	//	public abstract Task CommitChangesAsync();

	//	public virtual async Task<TAggregate> GetByIdAsync<TAggregate>( Guid aggregateId ) where TAggregate : class, IAggregate
	//	{
	//		return ( await this.GetByIdAsync<TAggregate>( new[] { aggregateId } ) ).Single();
	//	}

	//	public abstract Task<IEnumerable<TAggregate>> GetByIdAsync<TAggregate>( params Guid[] aggregateIds )
	//		where TAggregate : class, IAggregate;		
	//}
}

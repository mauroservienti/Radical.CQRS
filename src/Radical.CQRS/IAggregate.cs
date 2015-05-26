using System;
using System.Collections.Generic;

namespace Radical.CQRS
{
	public interface IAggregate
	{
		Guid Id { get; }
		int Version { get; }

		Boolean IsChanged { get; }
		IEnumerable<IDomainEvent> GetUncommittedEvents();
		void ClearUncommittedEvents();
	}

	public interface IHaveState 
	{
		IAggregateState State { get; }
		void AcceptState( IAggregateState state );
	}

	public interface IAggregate<TState> : IAggregate, IHaveState where TState : class, IAggregateState
	{

	}

	public interface IAggregateState 
	{
		Guid Id { get; set; }
		int Version { get; set; }
	}
}

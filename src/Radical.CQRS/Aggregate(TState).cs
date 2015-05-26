using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Radical.CQRS.Reflection;
using Topics.Radical.Validation;

namespace Radical.CQRS
{
	public abstract class Aggregate<TState> : Aggregate, IAggregate<TState> where TState : class, IAggregateState
	{
		protected TState State { get; private set; }

		IAggregateState IHaveState.State
		{
			get { return this.State; }
		}

		void IHaveState.AcceptState( IAggregateState state )
		{
			Ensure.That( state ).IsNotNull();
			Ensure.That( this.State ).Is( null );

			this.State = ( TState )state;
		}
	}
}

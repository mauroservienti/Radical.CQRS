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
		protected Aggregate()
		{

		}

		protected Aggregate( TState state )
		{
			this.Data = state;
		}

		protected TState Data { get; private set; }

		public override Guid Id
		{
			get { return this.Data.Id; }
			protected set { this.Data.Id = value; }
		}

		public override int Version
		{
			get { return this.Data.Version; }
			protected set { this.Data.Version = value; }
		}

		IAggregateState IHaveState.State
		{
			get { return this.Data; }
		}

		void IHaveState.AcceptState( IAggregateState state )
		{
			Ensure.That( state ).IsNotNull();
			Ensure.That( this.Data ).Is( null );

			this.Data = ( TState )state;
		}
	}
}

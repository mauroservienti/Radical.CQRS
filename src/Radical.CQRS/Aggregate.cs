using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Radical.CQRS.Reflection;

namespace Radical.CQRS
{
	public abstract class Aggregate : IAggregate, IEquatable<IAggregate> //, IHaveState<TState>
	{
		[Key]
		public Guid Id { get; protected set; }

		public int Version { get; protected set; }

		[Timestamp]
		protected byte[] RowVersion { get; set; }

		[NotMapped]
		public bool IsChanged { get { return this._uncommittedEvents.Any(); } }

		readonly List<IDomainEvent> _uncommittedEvents = new List<IDomainEvent>();

		protected Aggregate()
		{
			this.Id = Guid.NewGuid();
		}

		IEnumerable<IDomainEvent> IAggregate.GetUncommittedEvents()
		{
			return this._uncommittedEvents.ToArray();
		}

		void IAggregate.ClearUncommittedEvents()
		{
			this._uncommittedEvents.Clear();
		}

		protected void RaiseEvent<TEvent>( Action<TEvent> builder ) where TEvent : IDomainEvent
		{
			var newVersion = this.Version + 1;
			var @event = ConcreteProxyCreator.CreateInsance<TEvent>();
			@event.Id = Guid.NewGuid();
			@event.OccurredAt = DateTimeOffset.Now;
			@event.AggregateId = this.Id;
			@event.AggregateVersion = newVersion;

			builder( @event );

			this._uncommittedEvents.Add( @event );
			this.Version = newVersion;
		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		public override bool Equals( object obj )
		{
			return this.Equals( obj as IAggregate );
		}

		public virtual bool Equals( IAggregate other )
		{
			return other != null && other.Id == this.Id;
		}
	}
}

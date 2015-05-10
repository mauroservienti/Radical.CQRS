using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Radical.CQRS.Reflection;

namespace Radical.CQRS
{
    public abstract class Aggregate : IAggregate, IEquatable<IAggregate> //, IHaveState<TState>
    {
		public Guid GetKey()
		{
			return this.Id;
		}

        protected internal abstract Guid Id { get; set; }
        protected internal virtual int Version { get; set; }

        [NotMapped]
        int IAggregate.Version { get { return this.Version; } }
        [NotMapped]
        Guid IAggregate.Id { get { return this.Id; } }

        [NotMapped]
        public Boolean IsChanged { get { return this.uncommittedEvents.Any(); } }

        List<IDomainEvent> uncommittedEvents = new List<IDomainEvent>();

        protected Aggregate()
        {
            this.Id = Guid.NewGuid();
        }

        IEnumerable<IDomainEvent> IAggregate.GetUncommittedEvents()
        {
            return this.uncommittedEvents.ToArray();
        }

        void IAggregate.ClearUncommittedEvents()
        {
            this.uncommittedEvents.Clear();
        }

        protected void RaiseEvent<TEvent>(Action<TEvent> builder) where TEvent : IDomainEvent
        {
            var newVersion = this.Version + 1;
            var @event = ConcreteProxyCreator.CreateInsance<TEvent>();
            @event.Id = Guid.NewGuid();
            @event.OccurredAt = DateTimeOffset.Now;
            @event.AggregateId = this.Id;
            @event.AggregateVersion = newVersion;

            builder(@event);

            this.uncommittedEvents.Add(@event);
            this.Version = newVersion;
        }

        public override Int32 GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override Boolean Equals(object obj)
        {
            return this.Equals(obj as IAggregate);
        }

        public virtual Boolean Equals(IAggregate other)
        {
            return other != null && other.Id == this.Id;
        }
        //TState IHaveState<TState>.State { get; set; }
    }
}

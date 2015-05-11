using Radical.CQRS;

namespace Sample.Domain.People
{
    public interface IPersonCreated : IDomainEvent
    {

        string Name { get; set; }
    }
}

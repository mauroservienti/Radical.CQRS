using Radical.CQRS;

namespace Sample.Domain.People
{
    public interface IPersonNameChanged: IDomainEvent
    {
        string NewName { get; set; }
    }
}

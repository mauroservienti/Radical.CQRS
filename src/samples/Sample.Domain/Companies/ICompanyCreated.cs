using Radical.CQRS;

namespace Sample.Domain.Companies
{
    public interface ICompanyCreated : IDomainEvent
    {
        string Name { get; set; }
    }
}

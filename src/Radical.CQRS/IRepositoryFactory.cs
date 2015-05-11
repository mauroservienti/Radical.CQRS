namespace Radical.CQRS
{
    public interface IRepositoryFactory
    {
        IRepository OpenSession();
    }
}

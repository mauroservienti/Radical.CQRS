namespace Radical.CQRS
{
    public interface IRepositoryFactory
    {
        IAsyncRepository OpenAsyncSession();
		IRepository OpenSession();
    }
}

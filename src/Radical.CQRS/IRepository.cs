using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radical.CQRS
{
	//public interface IAsyncRepository : IDisposable
	//{
	//	void Add<TAggregate>( TAggregate aggregate ) where TAggregate : class, IAggregate;
	//	Task CommitChangesAsync();

	//	Task<TAggregate> GetByIdAsync<TAggregate>( Guid aggregateId ) where TAggregate : class, IAggregate;

	//	Task<IEnumerable<TAggregate>> GetByIdAsync<TAggregate>( params Guid[] aggregateIds ) where TAggregate : class, IAggregate;
	//}

	public interface IRepository : IDisposable
	{
		void Add<TAggregate>( TAggregate aggregate ) where TAggregate : class, IAggregate;
		void CommitChanges();

		TAggregate GetById<TAggregate>( Guid aggregateId ) where TAggregate : class, IAggregate;

		IEnumerable<TAggregate> GetById<TAggregate>( params Guid[] aggregateIds ) where TAggregate : class, IAggregate;
	}
}

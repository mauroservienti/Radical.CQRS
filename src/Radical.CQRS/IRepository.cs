using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radical.CQRS
{
	public interface IRepository : IDisposable
	{
		void Add<TAggregate>( TAggregate aggregate ) where TAggregate : class, IAggregate;
		Task CommitChangesAsync();

		Task<TAggregate> GetByIdAsync<TAggregate>( Guid aggregateId ) where TAggregate : class, IAggregate;

		Task<IEnumerable<TAggregate>> GetByIdAsync<TAggregate>( params Guid[] aggregateIds ) where TAggregate : class, IAggregate;
	}
}

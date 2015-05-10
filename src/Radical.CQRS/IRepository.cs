using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.CQRS
{
	public interface IRepository : IDisposable
	{
		void Add<TAggregate>( TAggregate aggregate ) where TAggregate : class, IAggregate;
		void CommitChanges();

        TAggregate GetById<TAggregate>(Guid aggregateId) where TAggregate : class, IAggregate;

        IEnumerable<TAggregate> GetById<TAggregate>(params Guid[] aggregateIds) where TAggregate : class, IAggregate;
	}
}

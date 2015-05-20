//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radical.CQRS
//{
//	public interface IAggregateAsyncLoader<TAggregate> where TAggregate : class, IAggregate
//	{
//		Task<TAggregate> GetByIdAsync( DbContext session, Guid aggregateId );

//		Task<IEnumerable<TAggregate>> GetByIdAsync( DbContext session, Guid[] aggregateIds );
//	}
//}

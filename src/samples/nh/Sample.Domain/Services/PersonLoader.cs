//using Radical.CQRS;
//using Sample.Domain.People;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Sample.Domain.Services
//{
//	class PersonLoader : IAggregateLoader<Person>
//	{
//		public Person GetById( ISession session, Guid aggregateId )
//		{
//			var db = session.Set<Person>();
//			var aggregate = db.Include( p => p.Addresses )
//				.Single( p => p.Id == aggregateId );

//			return aggregate;
//		}

//		public IEnumerable<Person> GetById( ISession session, Guid[] aggregateIds )
//		{
//			var db = session.Set<Person>();
//			var results = db.Include( p => p.Addresses )
//				.Where( p => aggregateIds.Contains( p.Id ) )
//				.ToList();

//			return results;
//		}
//	}
//}

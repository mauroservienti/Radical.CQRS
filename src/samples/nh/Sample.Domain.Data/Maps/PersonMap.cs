//using FluentNHibernate.Mapping;
//using Sample.Domain.People;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Sample.Domain.Data.Maps
//{
//	public class PersonMap : ClassMap<Person>
//	{
//		public PersonMap()
//		{
//			Id( p=>p.Id ).GeneratedBy.Assigned();
//			Map( Reveal.Member<Person>( "RowVersion" ) ).OptimisticLock();
//			Map( p=>p.Version );
//			Map( p=>p.Name );
//			Map( Reveal.Member<Project>( "_status" ), "Status" ).CustomType<int>();
//			HasMany<Task>( Reveal.Member<Project>( "_tasks" ) ).KeyColumn( "ProjectId" ).Cascade.All();
//		}
//	}
//}

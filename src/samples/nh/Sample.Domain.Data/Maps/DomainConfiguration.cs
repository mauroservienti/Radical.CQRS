//using FluentNHibernate.Automapping;
//using Radical.CQRS;
//using Sample.Domain.People;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Topics.Radical.Reflection;

//namespace Sample.Domain.Data.Maps
//{
//	public class DomainConfiguration : DefaultAutomappingConfiguration
//	{
//		public override bool ShouldMap( Type type )
//		{
//			return type.Is<IAggregate>();
//		}

//		public override bool IsVersion( FluentNHibernate.Member member )
//		{
//			if( member.Name == "RowVersion" ) 
//			{
//				return true;
//			}

//			return base.IsVersion( member );
//		}

//		public override bool IsComponent( Type type )
//		{
//			return type == typeof( BornInfo )
//				|| type == typeof( Address );
//		}
//	}
//}

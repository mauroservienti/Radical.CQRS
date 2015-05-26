using FluentNHibernate;
using FluentNHibernate.Mapping;
using Sample.Domain.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Data.Maps
{
	public class CompanyStateMap : ClassMap<Company.State>
	{
		public CompanyStateMap()
		{
			Table( "Companies" );
			Not.LazyLoad();
			Id( s => s.Id ).GeneratedBy.Assigned();
			Map( s => s.Version ).OptimisticLock();
			Map( s => s.Name );
		}
	}
}

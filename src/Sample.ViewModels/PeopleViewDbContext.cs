using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.ViewModels
{
	class PeopleViewDbContext : DbContext
	{
		public DbSet<PersonView> PeopleView { get; set; }

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			
			modelBuilder.Types<PersonView>()
				.Configure( c => c.ToTable( "PeopleView" ) );
		}
	}
}

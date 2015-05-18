using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Sample.ViewModels
{
	public class PeopleViewDbContext : DbContext
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

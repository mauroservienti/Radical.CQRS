using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Utilities;
using System.Data.Entity.SqlServer;

namespace Radical.CQRS.DbMigrations
{
	public class CustomSqlServerMigrationSqlGenerator: SqlServerMigrationSqlGenerator
	{
		protected override void Generate( MigrationOperation migrationOperation )
		{
			var createVperation = migrationOperation as CreateViewOperation;
			if( createVperation != null )
			{
				using( IndentedTextWriter writer = Writer() )
				{
					writer.WriteLine( "CREATE VIEW {0} AS {1} ; ", createVperation.ViewName, createVperation.ViewString );
					Statement( writer );
				}
			}

			var dropVperation = migrationOperation as DropViewOperation;
			if( dropVperation != null )
			{
				using( IndentedTextWriter writer = Writer() )
				{
					writer.WriteLine( "DROP VIEW {0}; ", dropVperation.ViewName );
					Statement( writer );
				}
			}
		}
	}
}

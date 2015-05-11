using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS.DbMigrations
{
	public static class DropViewOperationExtensions
	{
		public static void DropView( this DbMigration migration, string viewName )
		{
			( ( IDbMigration )migration ).AddOperation( new DropViewOperation( viewName ) );
		}		
	}

	public class DropViewOperation : MigrationOperation
	{
		public DropViewOperation( string viewName )
			: base ( null )
		{
			ViewName = viewName;
		}
		public string ViewName { get; private set; }

		public override bool IsDestructiveChange
		{
			get { return false; }
		}
	}
}

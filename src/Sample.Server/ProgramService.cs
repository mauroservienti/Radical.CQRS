using Castle.Windsor;
using Microsoft.Owin.Hosting;
using Radical.Bootstrapper;
using Radical.CQRS.Server;
using System;
using System.Linq;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Web.OData.Builder;

namespace Sample.Server
{
	class ProgramService : ServiceBase
	{
		ServerHost server = null;

		static void Main()
		{
			using( var service = new ProgramService() )
			{
				// so we can run interactive from Visual Studio or as a windows service
				if( Environment.UserInteractive )
				{
					service.OnStart( null );
					Console.WriteLine( "\r\nPress enter key to stop program\r\n" );
					Console.ReadLine();
					service.OnStop();
					return;
				}
				Run( service );
			}
		}

		protected override void OnStart( string[] args )
		{
			string baseAddress = ConfigurationManager.AppSettings[ "owin/baseAddress" ];

			var bootstrapper = new WindsorBootstrapper( AppDomain.CurrentDomain.BaseDirectory );
			var windsor = bootstrapper.Boot();

			this.server = new ServerHost(
				baseAddress,
				bootstrapper.ProbeDirectory,
				windsor );

			var objectModelBuilder = new ODataConventionModelBuilder();
			objectModelBuilder.EntitySet<Sample.ViewModels.PersonView>( "PeopleView" )
				.EntityType.HasKey( p => p.Id );

			this.server.AddOData( objectModelBuilder );
			this.server.Start();
		}

		protected override void OnStop()
		{
			if( this.server != null )
			{
				this.server.Stop();
				this.server = null;
			}
		}
	}
}
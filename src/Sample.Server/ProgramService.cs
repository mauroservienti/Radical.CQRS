using Castle.Windsor;
using Microsoft.Owin.Hosting;
using Radical.Bootstrapper;
using Radical.CQRS.Server;
using System;
using System.Linq;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.ServiceProcess;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.Owin.Cors;
using Owin;
using Radical.CQRS.Data;
using Sample.ViewModels;

namespace Sample.Server
{
	class ProgramService : ServiceBase
	{
		ServerHost _server = null;

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

		protected override void OnStop()
		{
			if( this._server == null )
			{
				return;
			}
			this._server.Stop();
			this._server = null;
		}

		protected override void OnStart( string[] args )
		{
			var baseAddress = ConfigurationManager.AppSettings[ "owin/baseAddress" ];

			var bootstrapper = new WindsorBootstrapper( AppDomain.CurrentDomain.BaseDirectory );
			var windsor = bootstrapper.Boot();

			this._server = new ServerHost(
				baseAddress,
				bootstrapper.ProbeDirectory,
				windsor );

			AddODataSupport( this._server );
			AddSignalRSupport( this._server );

			this._server.Start();
		}

		static void AddODataSupport( ServerHost server )
		{
			var objectModelBuilder = new ODataConventionModelBuilder();
			objectModelBuilder.EntitySet<Sample.ViewModels.PersonView>( "PeopleView" )
				.EntityType.HasKey( p => p.Id );

			server.AddHttpConfigurationCustomization( cfg =>
			{
				cfg.MapODataServiceRoute(
						routeName: "ODataRoute",
						routePrefix: null,
						model: objectModelBuilder.GetEdmModel()
					);
			} );
		}

		static void AddSignalRSupport( ServerHost server )
		{
			server.AddAppBuilderCustomization( appBuilder =>
			{
				appBuilder.UseCors( CorsOptions.AllowAll );
				appBuilder.MapSignalR();
			} );
		}
	}
}
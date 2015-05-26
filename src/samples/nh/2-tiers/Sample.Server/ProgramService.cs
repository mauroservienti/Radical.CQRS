using System;
using System.Configuration;
using System.ServiceProcess;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.Owin.Cors;
using Owin;
using Radical.Bootstrapper;
using Radical.CQRS.Server;
using Sample.ViewModels;
using Microsoft.AspNet.SignalR;

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

			var bornInfo = objectModelBuilder.ComplexType<BornInfoView>();
			
			bornInfo.Property( bi => bi.When );
			bornInfo.Property( bi => bi.Where );

			var person = objectModelBuilder.EntitySet<PersonView>( "PeopleView" )
				.EntityType.HasKey( p => p.Id );

			person.Property( p => p.Name );
			person.Property( p => p.Version );
			person.ComplexProperty( p => p.BornInfo );

			var address = objectModelBuilder.EntitySet<AddressView>( "AddressesView" )
				.EntityType.HasKey( a => a.AddressId );

			address.Property( a => a.Street );
			address.Property( a => a.PersonId );

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
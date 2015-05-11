using Castle.Windsor;
using Jason.Configuration;
using Jason.WebAPI.Validation;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Serialization;
using Owin;
using Radical.Bootstrapper.Windsor.AspNet.Infrastructure;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Jason.WebAPI;

namespace Radical.CQRS.Server
{
	public class ServerHost
	{
		//ODataModelBuilder oDataModelBuilder;
		String probeDirectory;
		IWindsorContainer windsor;
		IDisposable owinHost = null;
		String httpBaseAddress;

		public ServerHost( String httpBaseAddress, String probeDirectory, IWindsorContainer windsor )
		{
			this.httpBaseAddress = httpBaseAddress;
			this.probeDirectory = probeDirectory;
			this.windsor = windsor;

			this.CustomizeHttpConfiguration = cfg => { };
			this.CustomizeAppBuilder = builder => { };
			this.CustomizeJasonConfig = cfg => { };
			this.CustomizeJasonWebAPIEndpoint= endpoint => { };
		}

		//public void AddOData(ODataModelBuilder builder) 
		//{
		//	this.oDataModelBuilder = builder;
		//}

		public Action<HttpConfiguration> CustomizeHttpConfiguration { get; set; }
		public Action<IAppBuilder> CustomizeAppBuilder { get; set; }

		public Action<IJasonServerConfiguration> CustomizeJasonConfig { get; set; }

		public Action<JasonWebAPIEndpoint> CustomizeJasonWebAPIEndpoint { get; set; }

		public void Start()
		{
			// Start OWIN host 
			this.owinHost = WebApp.Start( this.httpBaseAddress, appBuilder => 
			{
				var config = new HttpConfiguration();

				this.WebApiConfig( config, appBuilder );
				this.JasonConfig(config);
			} );
		}

		void WebApiConfig(HttpConfiguration config, IAppBuilder appBuilder )
		{
			config.Formatters
				.JsonFormatter
				.SerializerSettings
				.ContractResolver = new CamelCasePropertyNamesContractResolver();

			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				"DefaultApiWithId",
				"api/{controller}/{id}",
				new { id = RouteParameter.Optional },
				new { id = @"\b[a-fA-F0-9]{8}(?:-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}\b" } );

			config.Routes.MapHttpRoute(
				"DefaultApiWithAction",
				"api/{controller}/{action}" );

			config.Routes.MapHttpRoute(
				"DefaultApiWithActionAndId",
				"api/{controller}/{action}/{id}",
				new { id = RouteParameter.Optional } );

			config.Routes.MapHttpRoute(
				"DefaultApiGet",
				"api/{controller}",
				new { action = "Get", controller = "Root" },
				new { httpMethod = new HttpMethodConstraint( HttpMethod.Get ) } );

			config.Routes.MapHttpRoute(
				"DefaultApiPost",
				"api/{controller}",
				new { action = "Post" },
				new { httpMethod = new HttpMethodConstraint( HttpMethod.Post ) } );

			config.DependencyResolver = new WindsorDependencyResolver( this.windsor );

			this.CustomizeHttpConfiguration(config);

			appBuilder.UseWebApi( config );

			this.CustomizeAppBuilder( appBuilder );
		}

		void JasonConfig( HttpConfiguration config )
		{
			var jasonConfig = new DefaultJasonServerConfiguration( this.probeDirectory )
			{
				Container = new WindsorJasonContainerProxy( this.windsor ),
				//TypeFilter = t => !t.Is<ShopperFallbackCommandHandler>()
			};

			var endpoint = new Jason.WebAPI.JasonWebAPIEndpoint(config)
			{
				IsCommandConvention = t => t.Namespace != null && t.Namespace.EndsWith(".Messages.Commands")
			};

			this.CustomizeJasonWebAPIEndpoint( endpoint );

			jasonConfig.AddEndpoint( endpoint )
				.UsingAsFallbackCommandValidator<ObjectDataAnnotationValidator>();

			this.CustomizeJasonConfig( jasonConfig );

			jasonConfig.Initialize();
		}

		public void Stop()
		{
			if( this.owinHost != null )
			{
				this.owinHost.Dispose();
				this.owinHost = null;
			}
		}
	}
}
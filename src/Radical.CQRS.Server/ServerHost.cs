using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Castle.Windsor;
using Jason.Configuration;
using Jason.WebAPI;
using Jason.WebAPI.Validation;
using Jason.Windsor;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Serialization;
using Owin;
using Radical.Bootstrapper.Windsor.AspNet.Infrastructure;

namespace Radical.CQRS.Server
{
	public class ServerHost
	{
		String probeDirectory;
		IWindsorContainer windsor;
		IDisposable owinHost = null;
		String httpBaseAddress;

		readonly List<Action<HttpConfiguration>> httpConfigurationCustomizations = new List<Action<HttpConfiguration>>();
		readonly List<Action<IAppBuilder>> appBuilderCustomizations = new List<Action<IAppBuilder>>();
		readonly List<Action<IJasonServerConfiguration>> jasonServerConfigurationCustomizations = new List<Action<IJasonServerConfiguration>>();
		readonly List<Action<JasonWebAPIEndpoint>> jasonWebAPIEndpointCustomizations = new List<Action<JasonWebAPIEndpoint>>();

		public ServerHost( String httpBaseAddress, String probeDirectory, IWindsorContainer windsor )
		{
			this.httpBaseAddress = httpBaseAddress;
			this.probeDirectory = probeDirectory;
			this.windsor = windsor;
		}

		public void AddHttpConfigurationCustomization( Action<HttpConfiguration> customization )
		{
			this.httpConfigurationCustomizations.Add(customization);
		}

		public void AddAppBuilderCustomization( Action<IAppBuilder> customization )
		{
			this.appBuilderCustomizations.Add( customization );
		}

		public void AddJasonServerConfigurationCustomization( Action<IJasonServerConfiguration> customization )
		{
			this.jasonServerConfigurationCustomizations.Add( customization );
		}

		public void AddJasonWebAPIEndpointCustomization( Action<JasonWebAPIEndpoint> customization )
		{
			this.jasonWebAPIEndpointCustomizations.Add( customization );
		}

		public void Start()
		{
			// Start OWIN host 
			this.owinHost = WebApp.Start( this.httpBaseAddress, appBuilder =>
			{
				var config = new HttpConfiguration();

				this.WebApiConfig( config, appBuilder );
				this.JasonConfig( config );
			} );
		}

		void WebApiConfig( HttpConfiguration config, IAppBuilder appBuilder )
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

			this.httpConfigurationCustomizations.ForEach( c => c( config ) );

			appBuilder.UseWebApi( config );

			this.appBuilderCustomizations.ForEach( c => c( appBuilder ) );
		}

		void JasonConfig( HttpConfiguration config )
		{
			var jasonConfig = new DefaultJasonServerConfiguration( this.probeDirectory )
			{
				Container = new WindsorJasonContainerProxy( this.windsor ),
				//TypeFilter = t => !t.Is<ShopperFallbackCommandHandler>()
			};

			var endpoint = new JasonWebAPIEndpoint( config )
			{
				IsCommandConvention = t => t.Namespace != null && t.Namespace.EndsWith( ".Messages.Commands" ),
				OnJasonRequest = e =>
				{
					if( !e.IsCommandInterceptor && !e.IsJasonExecute )
					{
						return;
					}

					if( !e.RequestContainsCorrelationId )
					{
						e.CorrelationId = Guid.NewGuid().ToString();
						e.AppendCorrelationIdToResponse = true;
					}

					var operationContextManager = this.windsor.Resolve<IOperationContextManager>();
					var context = operationContextManager.GetCurrent();
					context.ForOperation( e.CorrelationId );
				}
			};

			this.jasonWebAPIEndpointCustomizations.ForEach( c => c( endpoint ) );

			jasonConfig.AddEndpoint( endpoint )
				.UsingAsFallbackCommandValidator<ObjectDataAnnotationValidator>();

			this.jasonServerConfigurationCustomizations.ForEach( c => c( jasonConfig ) );

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
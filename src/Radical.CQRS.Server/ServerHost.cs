using Castle.Windsor;
using Jason.Configuration;
using Jason.WebAPI.Validation;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Serialization;
using Owin;
using Radical.Bootstrapper;
using Radical.Bootstrapper.Windsor.AspNet.Infrastructure;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace Radical.CQRS.Server
{
	public class ServerHost
	{
		ODataModelBuilder oDataModelBuilder;
		String probeDirectory;
		IWindsorContainer windsor;
		IDisposable owinHost = null;
		String httpBaseAddress;

		public ServerHost( String httpBaseAddress, String probeDirectory, IWindsorContainer windsor )
		{
			this.httpBaseAddress = httpBaseAddress;
			this.probeDirectory = probeDirectory;
			this.windsor = windsor;
		}

		public void AddOData(ODataModelBuilder builder) 
		{
			this.oDataModelBuilder = builder;
		}

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

			if( this.oDataModelBuilder != null )
			{
				config.MapODataServiceRoute(
					routeName: "ODataRoute",
					routePrefix: null,
					model: this.oDataModelBuilder.GetEdmModel()
				);
			}

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

			appBuilder.UseWebApi( config );
		}

		void JasonConfig( HttpConfiguration config )
		{
			var jasonConfig = new DefaultJasonServerConfiguration( this.probeDirectory )
			{
				Container = new WindsorJasonContainerProxy( this.windsor ),
				//TypeFilter = t => !t.Is<ShopperFallbackCommandHandler>()
			};

			jasonConfig.AddEndpoint( new Jason.WebAPI.JasonWebAPIEndpoint( config )
			{
				IsCommandConvention = t =>
				{
					return t.Namespace != null
						&& t.Namespace.EndsWith( ".Messages.Commands" );
				},
				//OnJasonRequest = e =>
				//{
				//	//if( !e.IsCommandInterceptor && !e.IsJasonExecute )
				//	//{
				//	//	return;
				//	//}

				//	//if( !e.RequestContainsCorrelationId )
				//	//{
				//	//	e.CorrelationId = Guid.NewGuid().ToString();
				//	//	e.AppendCorrelationIdToResponse = true;
				//	//}

				//	//var operationContextManager = container.Resolve<IOperationContextManager>();
				//	//var context = operationContextManager.GetCurrent();
				//	//context.ForOperation( e.CorrelationId );
				//}
			} )
			.UsingAsFallbackCommandValidator<ObjectDataAnnotationValidator>()
				//.UsingAsFallbackCommandHandler<ShopperFallbackCommandHandler>()
			.Initialize();
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
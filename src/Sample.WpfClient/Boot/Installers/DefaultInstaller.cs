using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Radical.CQRS.Client;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WpfClient.Boot.Installers
{
	[Export( typeof( IWindsorInstaller ) )]
	public class DefaultInstaller: IWindsorInstaller
	{
		public void Install( IWindsorContainer container, IConfigurationStore store )
		{
			container.Register(
				Component.For<CommandClient>()
					.UsingFactoryMethod( () => 
					{
						string baseAddress = ConfigurationManager.AppSettings[ "jason/baseAddress" ];
						var client = new CommandClient( baseAddress );

						return client;
					} )
				);

			container.Register(
				Component.For<ODataClient>()
					.UsingFactoryMethod( () =>
					{
						string baseAddress = ConfigurationManager.AppSettings[ "odata/baseAddress" ];
						var client = new ODataClient( baseAddress );

						return client;
					} )
				);
		}
	}
}

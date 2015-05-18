using System.ComponentModel.Composition;
using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Sample.WpfClient.Boot.Installers
{
	[Export( typeof( IWindsorInstaller ) )]
	public class DefaultInstaller: IWindsorInstaller
	{
		public void Install( IWindsorContainer container, IConfigurationStore store )
		{
		
		}
	}
}

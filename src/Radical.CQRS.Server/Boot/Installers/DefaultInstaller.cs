using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Radical.CQRS.Server.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS.Server.Boot.Installers
{
	[Export( typeof( IWindsorInstaller ) )]
	public sealed class DefaultInstaller : IWindsorInstaller
	{
		public void Install( IWindsorContainer container, IConfigurationStore store )
		{
			container.Register
			(
				Component.For<IOperationContext>()
					.ImplementedBy<OperationContext>()
					.LifeStyle.Is( Castle.Core.LifestyleType.Scoped )
			);
		}
	}
}

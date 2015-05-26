using System.ComponentModel.Composition;
using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Conventions.Helpers;
using Sample.Domain.People;
using FluentNHibernate.Automapping;
using Sample.Domain.Data.Maps;
using Radical.CQRS;
using FluentNHibernate;

namespace Sample.Domain.Data.Boot.Installers
{
	[Export( typeof( IWindsorInstaller ) )]
	public class DefaultInstaller : IWindsorInstaller
	{
		public void Install( IWindsorContainer container, IConfigurationStore store )
		{
			container.Register
			(
				Types.FromAssemblyContaining<Person>()
					.IncludeNonPublicTypes()
					.Where( t => !t.IsInterface && !t.IsAbstract && !t.IsGenericType && t.Namespace != null && t.IsNested && t.Name.EndsWith( "Factory" ) )
					.WithService.Select( ( type, baseTypes ) => new[] { type } )
					.LifestyleSingleton()
			);
		}
	}
}

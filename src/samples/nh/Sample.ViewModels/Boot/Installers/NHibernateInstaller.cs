using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System.ComponentModel.Composition;

namespace Sample.ViewModels.Boot.Installers
{
	[Export( typeof( IWindsorInstaller ) )]
	public class NHibernateInstaller : IWindsorInstaller
	{
		public void Install( IWindsorContainer container, IConfigurationStore store )
		{
			container.Register
			(
				Component.For<ISessionFactory>()
					.Named( "ViewModelsSessionFactory" )
					.UsingFactoryMethod( x => CreateSessionFactory() ) 
			);

			container.Register
			(
				Component.For<IStatelessSession>()
					.UsingFactoryMethod( x => 
					{
						var factory = container.Resolve<ISessionFactory>( "ViewModelsSessionFactory" );
						return factory.OpenStatelessSession();
					} )
					.LifestyleTransient()
			);
		}

		static ISessionFactory CreateSessionFactory()
		{
			return Fluently.Configure()
				.Database
				( 
					MsSqlConfiguration.MsSql2012.ConnectionString( c => c.FromConnectionStringWithKey( "SampleViewModels" ) ) 
				)
				.Mappings( m =>
				{
					m.FluentMappings.AddFromAssemblyOf<NHibernateInstaller>();
				} )
				.BuildSessionFactory();
		}
	}
}

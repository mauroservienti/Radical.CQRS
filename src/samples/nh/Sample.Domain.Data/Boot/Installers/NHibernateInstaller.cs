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
	public class NHibernateInstaller : IWindsorInstaller
	{
		public void Install( IWindsorContainer container, IConfigurationStore store )
		{
			container.Register( Component.For<ISessionFactory>()
				.UsingFactoryMethod( x => CreateSessionFactory() ) );
		}

		static ISessionFactory CreateSessionFactory()
		{
			//var domain = new DomainConfiguration();

			return Fluently.Configure()
				.Database( MsSqlConfiguration.MsSql2012
								.ConnectionString( c => c.FromConnectionStringWithKey( "SampleDomain" ) ) )
				.Mappings( m =>
				{
					//m.AutoMappings.Add( AutoMap.AssemblyOf<Person>( domain )
					//	.Override<IAggregate>( map =>
					//	{
					//		map.IgnoreProperty( a => a.IsChanged );
					//		map.Map( Reveal.Member<IAggregate>( "RowVersion" ) )
					//			.CustomSqlType( "timestamp" );
					//	} ) );
					//m.FluentMappings.Conventions.Add( DefaultLazy.Never() );
					m.FluentMappings.AddFromAssemblyOf<NHibernateInstaller>();
				} )
				//.CurrentSessionContext( "web" )
				.ExposeConfiguration( BuildSchema )
				.BuildSessionFactory();
		}

		static void BuildSchema( NHibernate.Cfg.Configuration config )
		{
			var buildSchema = ConfigurationManager.AppSettings[ "NHibernate/BuildSchema" ];
			bool build = false;
			bool.TryParse( buildSchema, out build );

			if( build )
			{
				new SchemaExport( config )
				  .Create( false, true );
			}
		}
	}
}

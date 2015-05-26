using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Topics.Radical.Helpers;
using Topics.Radical.Reflection;

namespace Radical.CQRS.Data
{
	public abstract class DomainContext : DbContext
	{
		protected DomainContext()
		{

		}

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			modelBuilder.Entity<EntityFrameworkDomainEventCommit>().ToTable( "DomainEventCommits" );
			modelBuilder.MapPropertiesOf<EntityFrameworkDomainEventCommit>( propertiesToSkip: new[] { "Event" } );
		}

		protected Expression<Func<T, Object>> Skip<T>( Expression<Func<T, Object>> property )
		{
			return property;
		}
	}

	//public class From
	//{
	//	public static IEnumerable<PropertyToSkip<T>> Skip<T>( params Expression<Func<T, Object>>[] properties )
	//	{
	//		var temp = new List<PropertyToSkip<T>>();

	//		foreach( var item in properties )
	//		{
	//			temp.Add( new PropertyToSkip<T> { Property = item } );
	//		}

	//		return temp;
	//	}
	//}

	//public class PropertyToSkip<T>
	//{
	//	public Expression<Func<T, Object>> Property { get; set; }
	//}

	public static class DbModelBuilderExtensions
	{
		public static void MapPropertiesOf<T>( this DbModelBuilder modelBuilder, Dictionary<String, Action<ConventionTypeConfiguration>> interceptors = null, String[] propertiesToSkip = null, Boolean autoConfigureAggregateVersionProperty = true, Boolean autoConfigureAggregateStateVersionProperty = true, Boolean skipAggregateIsChangedProperty= true ) where T : class
		{
			if( propertiesToSkip == null )
			{
				propertiesToSkip = new String[ 0 ];
			}
			var toSkip = new HashSet<String>( propertiesToSkip );

			if( interceptors == null )
			{
				interceptors = new Dictionary<String, Action<ConventionTypeConfiguration>>();
			}

			if( typeof( T ).Is<IAggregate>() )
			{
				if( skipAggregateIsChangedProperty ) 
				{
					toSkip.Add( ReflectionHelper.GetPropertyName<IAggregate>( a => a.IsChanged ) );
				}

				if( autoConfigureAggregateVersionProperty )
				{
					var version = ReflectionHelper.GetPropertyName<IAggregate>( a => a.Version );
					interceptors.Add( version, cfg =>
					{
						var property = cfg.Property( version );
						property.HasColumnName( version );
						property.IsConcurrencyToken();
					} );
				}
			}

			if( typeof( T ).Is<IAggregateState>() )
			{
				if( autoConfigureAggregateStateVersionProperty )
				{
					var version = ReflectionHelper.GetPropertyName<IAggregateState>( a => a.Version );
					interceptors.Add( version, cfg =>
					{
						var property = cfg.Property( version );
						property.HasColumnName( version );
						property.IsConcurrencyToken();
					} );
				}
			}

			modelBuilder
				.Types()
				.Where( t => t == typeof( T ) )
				.Configure( c =>
				{
					var properties = GetAllProperties( c.ClrType );

					foreach( var p in properties )
					{
						if( interceptors.ContainsKey( p.Name ) )
						{
							var interceptor = interceptors[ p.Name ];
							interceptor( c );
						}
						else if( !toSkip.Contains( p.Name ) )
						{
							var property = c.Property( p );
							property.HasColumnName( p.Name );
							if( p.Name == "Id" )
							{
								property.IsKey();
							}
						}
					}
				} );
		}

		static IEnumerable<PropertyInfo> GetAllProperties( Type type )
		{
			var all = new List<Type>
			{
				type
			};

			var current = type;
			while( current.BaseType != null )
			{
				all.Add( current.BaseType );
				current = current.BaseType;
			}

			var props = all.SelectMany( t => t.GetProperties( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance ) )
				.Where( p => !p.IsAttributeDefined<NotMappedAttribute>() );

			return props;

		}
	}
}

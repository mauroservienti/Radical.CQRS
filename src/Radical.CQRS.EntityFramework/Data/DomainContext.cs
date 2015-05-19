using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
			modelBuilder.RegisterEntityType( typeof( DomainEventCommit ) );
			modelBuilder.MapPropertiesOf<DomainEventCommit>();
		}
	}

	public static class DbModelBuilderExtensions 
	{
		public static void MapPropertiesOf<T>( this DbModelBuilder modelBuilder ) where T : class
		{
			MapPropertiesOf<T>( modelBuilder, p => true );
		}

		public static void MapPropertiesOf<T>( this DbModelBuilder modelBuilder, Func<PropertyInfo, Boolean> filter ) where T : class
		{
			modelBuilder
				.Types()
				.Where( t => t == typeof( T ) )
				.Configure( c =>
				{
					var properties = GetAllProperties( c.ClrType, filter );

					foreach( var p in properties )
					{
						c.Property( p ).HasColumnName( p.Name );
						if( p.Name == "Id" )
						{
							c.Property( p ).IsKey();
						}
					}
				} );
		}

		static IEnumerable<PropertyInfo> GetAllProperties( Type type, Func<PropertyInfo, Boolean> filter )
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
				.Where( p => !p.IsAttributeDefined<NotMappedAttribute>() && filter( p ) );

			return props;

		}
	}
}

using Radical.CQRS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Reflection;

namespace Radical.CQRS.Data
{
	public class DomainContext : DbContext
	{
		readonly Assembly[] _domainAssemblies;
		public DomainContext( params Assembly[] domainAssemblies )
		{
			this._domainAssemblies = domainAssemblies;
		}

		public DomainContext()
		{

		}

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			if( this._domainAssemblies != null && this._domainAssemblies.Any() )
			{
				var aggregates = this._domainAssemblies.SelectMany( a => a.DefinedTypes ).Where( t => t.Is<IAggregate>() );

				foreach( var item in aggregates )
				{
					modelBuilder.RegisterEntityType( item );
				}

				modelBuilder.RegisterEntityType( typeof( DomainEventCommit ) );

				modelBuilder
					.Types()
					.Configure( c =>
					{
						var nonPublicProperties = GetAllProperties( c.ClrType );

						foreach( var p in nonPublicProperties )
						{
							c.Property( p ).HasColumnName( p.Name );
							if( p.Name == "Id" )
							{
								c.Property( p ).IsKey();
							}
						}
					} );
			}
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

			var props = all.SelectMany( t => t.GetProperties( BindingFlags.NonPublic | BindingFlags.Instance ) )
				.Where( p => !p.IsAttributeDefined<NotMappedAttribute>() );

			return props;

		}


	}
}

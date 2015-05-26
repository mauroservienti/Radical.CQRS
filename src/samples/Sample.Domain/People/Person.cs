using System;
using Radical.CQRS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Domain.People
{
	public class Person : Aggregate
	{
		internal string Name { get; set; }

		internal BornInfo Info { get; set; }

		internal ISet<Address> Addresses { get; set; }

		internal Person()
		{
			this.Addresses = new HashSet<Address>();
		}

		public static Person CreateNew( string nome )
		{
			var p= new Person
			{
				Name = nome,
				Info = new BornInfo()
				{
					When = DateTimeOffset.Now,
					Where = "Rome"
				}
			};

			p.Addresses.Add( new Address()
			{
				Id = Guid.NewGuid(),
				PersonId = p.Id,
				Street = "djhfbvdjkfbvkjdfh"
			} );

			return p.SetupCompleted();
		}

		private Person SetupCompleted()
		{
			this.RaiseEvent<IPersonCreated>( e => e.Name = this.Name );

			return this;
		}

		public void ChangeName( string newName )
		{
			this.Name = newName;
			this.RaiseEvent<IPersonNameChanged>( e => e.NewName = newName );
		}
	}
}

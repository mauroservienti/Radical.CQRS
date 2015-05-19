using System;
using Radical.CQRS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Domain.People
{
	public class Person : Aggregate
	{
		protected override Guid Id { get; set; }

		internal string Name { get; set; }

		internal BornInfo Info { get; set; }

		internal List<Address> Addresses { get; set; }

		internal Person()
		{
			this.Addresses = new List<Address>();
			this.Addresses.Add( new Address()
			{
				Id = Guid.NewGuid(),
				PersonId = this.Id,
				Street = "djhfbvdjkfbvkjdfh"
			} );
		}

		public static Person CreateNew( string nome )
		{
			return new Person
			{
				Name = nome,
				Info = new BornInfo()
				{
					When = DateTimeOffset.Now,
					Where = "Rome"
				}
			}.SetupCompleted();
		}

		private Person SetupCompleted()
		{
			this.RaiseEvent<IPersonCreated>( e => e.Name = this.Name );

			return this;
		}

		public void CambiaNome( string nome )
		{
			this.Name = nome;
			this.RaiseEvent<IPersonNameChanged>( e => e.NewName = nome );
		}
	}
}

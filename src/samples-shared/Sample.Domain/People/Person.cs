using System;
using Radical.CQRS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Domain.People
{
	public class Person : Aggregate
	{
		protected override Guid Id { get; set; }

		private string Name { get; set; }

		internal virtual List<Address> Addresses { get; set; }

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
				Name = nome
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

using System;
using Radical.CQRS;

namespace Sample.Domain.People
{
	public class Person : Aggregate
	{
		protected override Guid Id { get; set; }

		internal Person()
		{

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

		private string Name { get; set; }

		public void CambiaNome( string nome )
		{
			this.Name = nome;
			this.RaiseEvent<IPersonNameChanged>( e => e.NewName = nome );
		}
	}
}

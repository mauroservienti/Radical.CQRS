using System;
using Jason.Handlers.Commands;
using Radical.CQRS;
using Sample.Domain.People;
using Sample.Messages.Commands;

namespace Sample.Domain.Handlers
{
	class CreateNewPersonCommandHandler : AbstractCommandHandler<CreateNewPerson>
	{
		public IRepositoryFactory RepositoryFactory { get; set; }

		protected override object OnExecute( CreateNewPerson command )
		{
			using( var repository = RepositoryFactory.OpenSession() )
			{
				var aPerson = Person.CreateNew( command.Name );

				repository.Add( aPerson );
				repository.CommitChanges();

				return aPerson.GetKey();
			}
		}
	}
}

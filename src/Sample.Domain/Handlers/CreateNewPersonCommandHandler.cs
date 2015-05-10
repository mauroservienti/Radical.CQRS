using Jason.Handlers.Commands;
using Sample.Domain.People;
using Sample.Messages.Commands;
using Radical.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

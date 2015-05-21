using System;
using Jason.Handlers.Commands;
using Radical.CQRS;
using Sample.Domain.People;
using Sample.Messages.Commands;

namespace Sample.Domain.Handlers
{
	class TouchPersonHandler : AbstractCommandHandler<TouchPerson>
	{
		public IRepositoryFactory RepositoryFactory { get; set; }

		protected override object OnExecute( TouchPerson command )
		{
			using( var repository = RepositoryFactory.OpenSession() )
			{
				var aPerson = repository.GetById<Person>( command.Id );

				repository.CommitChanges();

				return ( ( IAggregate )aPerson ).Id;
			}
		}
	}
}

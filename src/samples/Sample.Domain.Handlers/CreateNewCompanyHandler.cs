using Jason.Handlers.Commands;
using Radical.CQRS;
using Sample.Domain.Companies;
using Sample.Messages.Commands;

namespace Sample.Domain.Handlers
{
	class CreateNewCompanyHandler : AbstractCommandHandler<CreateNewCompany>
	{
		public IRepositoryFactory RepositoryFactory { get; set; }
		public Company.Factory CompanyFactory { get; set; }

		protected override object OnExecute( CreateNewCompany command )
		{
			using( var repository = this.RepositoryFactory.OpenSession() )
			{
				var company = this.CompanyFactory.CreateNew( command.Name );

				repository.Add( company );
				repository.CommitChanges();

				return company.Id;
			}
		}
	}
}

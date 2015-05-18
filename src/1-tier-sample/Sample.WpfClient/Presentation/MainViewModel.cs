using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Sample.ViewModels;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Services.Validation;
using Radical.CQRS;
using Sample.Domain.People;
using System.Data.Entity;

namespace Sample.WpfClient.Presentation
{
	class MainViewModel : AbstractViewModel, ICanBeValidated, IExpectViewLoadedCallback
	{
		readonly IRepositoryFactory repositoryFactory;

		public MainViewModel( IRepositoryFactory repositoryFactory )
		{
			this.repositoryFactory = repositoryFactory;
			this.People = new ObservableCollection<PersonView>();
		}

		protected override IValidationService GetValidationService()
		{
			return new DataAnnotationValidationService<MainViewModel>( this );
		}

		[Required( AllowEmptyStrings = false )]
		public String Name
		{
			get { return this.GetPropertyValue( () => this.Name ); }
			set { this.SetInitialPropertyValue( () => this.Name, value ); }
		}

		public ObservableCollection<PersonView> People { get; private set; }

		public async Task CreateNewPerson()
		{
			if( !this.Validate() )
			{
				this.TriggerValidation();
				return;
			}

			Guid key;

			using( var repository = this.repositoryFactory.OpenAsyncSession() )
			{
				var aPerson = Person.CreateNew( this.Name );

				repository.Add( aPerson );
				await repository.CommitChangesAsync();

				key = aPerson.GetKey();
			}

			using( var db = new PeopleViewDbContext() )
			{
				var result = await db.PeopleView.SingleAsync( p => p.Id == key );
				this.People.Insert( 0, result );
			}
		}

		async Task PopulatePeople()
		{
			using( var db = new PeopleViewDbContext() )
			{
				foreach( var item in await db.PeopleView.ToListAsync() )
				{
					this.People.Add( item );
				}
			}
		}

		public void OnViewLoaded()
		{
			this.PopulatePeople();
		}
	}
}
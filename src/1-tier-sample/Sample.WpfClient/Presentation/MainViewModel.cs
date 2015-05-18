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
using Sample.Messages.Commands;
using Jason.Client.ComponentModel;

namespace Sample.WpfClient.Presentation
{
	class MainViewModel : AbstractViewModel, ICanBeValidated, IExpectViewLoadedCallback
	{
		readonly IWorkerServiceClientFactory _clientFactory;

		public MainViewModel( IWorkerServiceClientFactory clientFactory )
		{
			this._clientFactory = clientFactory;
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

			using( var client = this._clientFactory.CreateClient() )
			{
				var key = ( Guid )client.Execute( new CreateNewPerson()
				{
					Name = this.Name
				} );

				using( var db = new PeopleViewDbContext() )
				{
					var result = await db.PeopleView.SingleAsync( p => p.Id == key );
					this.People.Insert( 0, result );
				}
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
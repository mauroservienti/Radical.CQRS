using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Radical.CQRS.Client;
using Sample.Messages.Commands;
using Sample.ViewModels;
using Simple.OData.Client;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Services.Validation;

namespace Sample.WpfClient.Presentation
{
	class MainViewModel : AbstractViewModel, ICanBeValidated, IExpectViewLoadedCallback
	{
		readonly CommandClient jason;
		readonly ODataClient odata;

		public MainViewModel( CommandClient jason, ODataClient odata )
		{
			this.jason = jason;
			this.odata = odata;

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

			var key = await this.jason.ExecuteAsync<Guid>( new CreateNewPerson()
			{
				Name = this.Name
			} );

			var result = await odata.For<PersonView>( "PeopleView" )
					.Key( key )
					.FindEntryAsync();

			this.People.Insert( 0, result );
		}

		async Task PopulatePeople() 
		{
			var result = await odata.For<PersonView>( "PeopleView" )
					.FindEntriesAsync();

			foreach( var item in result )
			{
				this.People.Add( item );
			}
		}

		public void OnViewLoaded()
		{
			this.PopulatePeople();
		}
	}
}
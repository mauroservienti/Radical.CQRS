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
using Sample.Messages.Commands;
using Jason.Client.ComponentModel;
using NHibernate;

namespace Sample.WpfClient.Presentation
{
	class MainViewModel : AbstractViewModel, ICanBeValidated, IExpectViewLoadedCallback
	{
		readonly IWorkerServiceClientFactory clientFactory;
		readonly Func<IStatelessSession> sessionFactory;

		public MainViewModel( IWorkerServiceClientFactory clientFactory, Func<IStatelessSession> sessionFactory )
		{
			this.clientFactory = clientFactory;
			this.sessionFactory = sessionFactory;
			this.People = new ObservableCollection<PersonView>();

			this.GetPropertyMetadata( () => this.SelectedPerson )
				.AddCascadeChangeNotifications( () => this.CanTouchSelectedPerson );
		}

		protected override IValidationService GetValidationService()
		{
			return new DataAnnotationValidationService<MainViewModel>( this );
		}

		[Required( AllowEmptyStrings = false )]
		public String Name
		{
			get { return this.GetPropertyValue( () => this.Name ); }
			set { this.SetPropertyValue( () => this.Name, value ); }
		}

		public ObservableCollection<PersonView> People { get; private set; }

		public PersonView SelectedPerson
		{
			get { return this.GetPropertyValue( () => this.SelectedPerson ); }
			set { this.SetPropertyValue( () => this.SelectedPerson, value ); }
		}

		public Boolean CanTouchSelectedPerson { get { return this.SelectedPerson != null; } }

		public void TouchSelectedPerson()
		{
			using( var client = this.clientFactory.CreateClient() )
			{
				var key = ( Guid )client.Execute( new TouchPerson()
				{
					Id = this.SelectedPerson.Id
				} );
			}
		}

		public void CreateCompany()
		{
			using( var client = this.clientFactory.CreateClient() )
			{
				var key = ( Guid )client.Execute( new CreateNewCompany()
				{
					Name = "FooBar"
				} );
			}
		}

		public Task CreateNewPerson()
		{
			if( !this.Validate() )
			{
				this.TriggerValidation();
				return Task.FromResult( false );
			}

			var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			return Task.Run( () =>
			{
				using( var client = this.clientFactory.CreateClient() )
				{
					var key = ( Guid )client.Execute( new CreateNewPerson()
					{
						Name = this.Name
					} );

					using( var db = this.sessionFactory() )
					{
						var p = db.Get<PersonView>( key );

						return p;
					}
				}
			} )
			.ContinueWith( t =>
			{
				if( t.IsFaulted )
				{
					throw t.Exception;
				}

				this.People.Insert( 0, t.Result );
			}, scheduler );
		}

		Task PopulatePeople()
		{
			var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			return Task.Run( () =>
			{
				using( var db = this.sessionFactory() )
				{
					var all = db.QueryOver<PersonView>()
						.List();

					return all;
				}
			} )
			.ContinueWith( t =>
			{
				if( t.IsFaulted )
				{
					throw t.Exception;
				}

				foreach( var item in t.Result )
				{
					this.People.Add( item );
				}
			}, scheduler );
		}

		public void OnViewLoaded()
		{
			this.PopulatePeople();
		}
	}
}
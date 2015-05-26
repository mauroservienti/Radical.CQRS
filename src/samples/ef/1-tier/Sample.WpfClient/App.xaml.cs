using System.Windows;
using System.Linq;
using Sample.WpfClient.Presentation;
using Topics.Radical.Windows.Presentation.Boot;
using System.Net;
using Castle;
using Jason.Configuration;
using Jason.Windsor;
using Topics.Radical.Windows.Presentation.Helpers;

namespace Sample.WpfClient
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			ServicePointManager.DefaultConnectionLimit = 10;

			new WindsorApplicationBootstrapper<MainView>()
				.OnBeforeInstall( bootConventions =>
				{
					bootConventions.AssemblyFileScanPatterns = entryAssembly =>
					{
						return bootConventions
							.DefaultAssemblyFileScanPatterns( entryAssembly )
							.Union( new[] { "Sample.*.dll" } );
					};
				} )
				.OnBoot( container =>
				{
					var probeDirectory = EnvironmentHelper.GetCurrentDirectory();
					var wrapper = ( ServiceProviderWrapper )container;

					new DefaultJasonServerConfiguration( probeDirectory )
					{
						Container = new WindsorJasonContainerProxy( wrapper.Container ),
						//TypeFilter = t => !t.Is<ShopperFallbackCommandHandler>()
					}
					.AddEndpoint( new Jason.Client.JasonInProcessEndpoint() )
					.Initialize();

					//.UsingAsFallbackCommandValidator<ObjectDataAnnotationValidator>();
				} );
		}
	}
}

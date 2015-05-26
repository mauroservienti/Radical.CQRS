using System.Windows;
using Sample.WpfClient.Presentation;
using Topics.Radical.Windows.Presentation.Boot;
using System.Net;

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

			var bootstrapper = new WindsorApplicationBootstrapper<MainView>();
		}
	}
}

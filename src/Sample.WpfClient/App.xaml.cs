using System.Windows;
using Sample.WpfClient.Presentation;
using Topics.Radical.Windows.Presentation.Boot;

namespace Sample.WpfClient
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			var bootstrapper = new WindsorApplicationBootstrapper<MainView>();
		}
	}
}

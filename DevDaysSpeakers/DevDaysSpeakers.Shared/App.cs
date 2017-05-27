using Xamarin.Forms;
using Xamvvm;

namespace DevDaysSpeakers
{
	public class App : Application
	{
		public App()
		{
			var factory = new XamvvmFormsRxUIFactory(this);
			XamvvmCore.SetCurrentFactory(factory);
			factory.RegisterNavigationPage<MainNavigationViewModel>(() => this.GetPageFromCache<Features.Speakers.SpeakersViewModel>());

			MainPage = XamvvmCore.CurrentFactory.GetPageFromCache<MainNavigationViewModel>() as Page;
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

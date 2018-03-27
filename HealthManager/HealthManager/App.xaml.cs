using CommonServiceLocator;
using HealthManager.Common.Other;
using HealthManager.View;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Unity.ServiceLocation;
using Xamarin.Forms;

namespace HealthManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ApplicationIinitializer.InitDatabase();

            var tabbedPage = new MainTabbedView();
            MainPage = tabbedPage;
            var unityServiceLocator = new UnityServiceLocator(ContainerInitializer.InitializeContainer());
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("android=75267511-edba-4c0b-9dcb-a82769c18d84;" +
                            "uwp={Your UWP App secret here};" +
                            "ios=8bcf8409-9380-414a-b0af-9d4b325d2926;",
                typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public void ChangeScreen(Page page)
        {
            MainPage = page;
        }
    }
}
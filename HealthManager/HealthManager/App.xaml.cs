using CommonServiceLocator;
using HealthManager.Common.Other;
using HealthManager.Model.Service.Implement;
using HealthManager.Model.Service.Interface;
using HealthManager.View;
using HealthManager.ViewModel;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Unity;
using Unity.ServiceLocation;

namespace HealthManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IBasicDataService, BasicDataService>();
            unityContainer.RegisterInstance(typeof(InputBasicDataViewModel));

            var unityServiceLocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);

            ApplicationIinitializer. InitDatabase();

            var basicDataService = new BasicDataService();

            if (basicDataService.GetBasicData() != null)
            {
                MainPage = new MainTabbedView();
            }
            else
            {
                MainPage = new InputBasicDataView();
            }
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

using HealthManager.Common.Other;
using HealthManager.Model.Service;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ApplicationIinitializer. InitDatabase();

            if (BasicDataService.GetBasicData() != null)
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

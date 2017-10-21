using HealthManager.DependencyInterface;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.View;
using SQLite;
using Xamarin.Forms;

namespace HealthManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath());

            try
            {

                db.DropTable<BodyImageModel>();
                db.DropTable<BasicDataModel>();
            }
            catch (NotNullConstraintViolationException e)
            {

            }

            try
            {

                db.CreateTable<BodyImageModel>();
                db.CreateTable<BasicDataModel>();
            }
            catch (NotNullConstraintViolationException e)
            {
                
            }

            if (BasicDataService.GetBasicData() != null)
            {
                MainPage = new HomeView();
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

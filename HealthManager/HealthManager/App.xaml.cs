using HealthManager.DependencyInterface;
using HealthManager.Model;
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

            MainPage = new InputBasicDataView();

            var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath());

            try
            {

                //db.DropTable<BodyImageModel>();
                //db.DropTable<BasicDataModel>();
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

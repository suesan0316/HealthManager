using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.Common
{
    class ViewModelCommonUtil
    {
        public static void BackHome()
        {
            ((App)Application.Current).ChangeScreen(new HomeView());
        }
    }
}

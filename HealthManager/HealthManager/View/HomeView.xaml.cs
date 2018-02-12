using HealthManager.Common;
using HealthManager.Common.Constant;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        public HomeView()
        {
            InitializeComponent();
            ViewModelConst.PageNavigation = Navigation;
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
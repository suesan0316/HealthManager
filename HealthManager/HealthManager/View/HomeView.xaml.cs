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
            MessagingCenter.Subscribe<ViewModelCommonUtil>(this, ViewModelConst.MessagingHomeReload,
                (sender) => { ControlScroll.ScrollToAsync(ControlScroll, ScrollToPosition.Start, false); });
            ViewModelConst.DataPageNavigation = Navigation;
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
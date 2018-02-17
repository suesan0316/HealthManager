using HealthManager.Common.Constant;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingHomeView : ContentPage
    {
        public TrainingHomeView()
        {
            InitializeComponent();
            ViewModelConst.TrainingPageNavigation = Navigation;
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
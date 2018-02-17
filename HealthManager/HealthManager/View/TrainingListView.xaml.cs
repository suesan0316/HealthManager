using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingListView : ContentPage
    {
        public TrainingListView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
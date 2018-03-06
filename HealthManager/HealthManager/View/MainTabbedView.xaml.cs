using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedView : TabbedPage
    {
        public MainTabbedView()
        {

            var navigationPageHome = new NavigationPage(new HomeView())
            {
                Icon = "home_icon.png"
            };
            
            var navigationPageTraining = new NavigationPage(new TrainingHomeView())
            {
                Icon = "training_icon.png"
            };
            Children.Add(navigationPageHome);
            Children[0].Title = "データ";
            Children.Add(navigationPageTraining);
            Children[1].Title = "トレーニング";
        }
    }
}
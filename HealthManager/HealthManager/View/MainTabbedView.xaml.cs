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
                //Title = "Schedule"
            };
            
            var navigationPageTraining = new NavigationPage(new TrainingHomeView())
            {
                //Title = "Schedule"
            };
            Children.Add(navigationPageHome);
            Children[0].Title = "データ";
            Children.Add(navigationPageTraining);
            Children[1].Title = "トレーニング";
        }
    }
}
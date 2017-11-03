using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedView : TabbedPage
    {
        public MainTabbedView()
        {

            var navigationPage = new NavigationPage(new HomeView())
            {
                Title = "Schedule"
            };
            Children.Add(new HomeView());
            Children.Add(new TrainingHomeView());
            
        }
    }
}
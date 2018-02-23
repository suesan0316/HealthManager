using HealthManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsWebView : ContentPage
    {
        public NewsWebView()
        {
            InitializeComponent();
            BindingContext = new NewsWebViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public NewsWebView(string url,INavigation navigation)
        {
            InitializeComponent();
            WebView.Source = url;
            BindingContext = new NewsWebViewModel(url, navigation);
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
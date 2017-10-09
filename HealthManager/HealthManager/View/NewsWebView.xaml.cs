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
        }

        public NewsWebView(string url)
        {
            InitializeComponent();
            WebView.Source = url;
            BindingContext = new NewsWebViewModel(url);
        }

    }
}
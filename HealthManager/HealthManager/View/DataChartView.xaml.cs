using HealthManager.Common.Enum;
using HealthManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataChartView : ContentPage
    {

        public DataChartView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new DataChartViewModel();
        }

        public DataChartView(BasicDataEnum target)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new DataChartViewModel(target);
        }
    }  
}
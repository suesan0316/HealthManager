using HealthManager.Common;
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
            BindingContext = new DataChartViewModel();
        }

        public DataChartView(BasicDataEnum target)
        {
            InitializeComponent();
            BindingContext = new DataChartViewModel(target);
        }
    }  
}
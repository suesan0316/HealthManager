using System.Linq;
using HealthManager.Model.Service;
using HealthManager.ViewModel;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataChartView : ContentPage
    {
        public DataChartView()
        {
            InitializeComponent();

            BindingContext = new DataChartViewModel();

            var list = BasicDataService.GetBasicDataList();

            var entries = list.Select(value => new Entry(value.BodyFatPercentage)
                {
                    Color = SKColor.Parse("#00CED1"),
                    Label = value.RegistedDate.ToString(),
                    ValueLabel = value.BodyFatPercentage.ToString()
                })
                .ToList();

            DataChart.Chart = new LineChart { Entries = entries };
        }
    }
}
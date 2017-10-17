using System;
using System.Collections.Generic;
using System.Linq;
using HealthManager.Common;
using HealthManager.Extention;
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
        }

        public DataChartView(BasicDataEnum target)
        {
            InitializeComponent();
            BindingContext = new DataChartViewModel();

            var list = BasicDataService.GetBasicDataList();
            var entries = new List<Entry>();

            switch (target)
            {
                case BasicDataEnum.BodyWeight:
                    entries = list.Select(value => CreateNewEntry(value.BodyWeight, value.RegistedDate))
                        .ToList();
                    break;

                case BasicDataEnum.BodyFatPercentage:
                    entries = list.Select(value => CreateNewEntry(value.BodyFatPercentage,value.RegistedDate))
                        .ToList();
                    break;
            }

            Term.Text = "期間 : " + entries.Min(data => data.Label) + "~" + entries.Max(data => data.Label);
            TermMin.Text = "期間中最小 : " + entries.Min(data=> data.Value) + target.DisplayUnit();
            TermMax.Text = "期間中最大 : " + entries.Max(data => data.Value) + target.DisplayUnit();
            TermAverage.Text = "期間中平均 : " + entries.Average(data=>data.Value)+ target.DisplayUnit();

            DataChart.Chart = new LineChart { Entries = entries };
            DataList.ItemsSource = entries.Select(data => data.Label + "  " + data.Value + target.DisplayUnit() );
        }

        private Entry CreateNewEntry(float value, DateTime registedDateTime)
        {
            return new Entry(value)
            {
                Color = SKColor.Parse("#00CED1"),
                Label = registedDateTime.ToString(),
                ValueLabel = value.ToString()
            };
        }
    }  
}
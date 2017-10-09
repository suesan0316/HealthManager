﻿using System.Collections.Generic;
using System.Linq;
using HealthManager.Common;
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

            List<Entry> entries = null;

            switch (target)
            {
                case BasicDataEnum.BodyWeight:
                    entries = list.Select(value => new Entry(value.BodyWeight)
                        {
                            Color = SKColor.Parse("#00CED1"),
                            Label = value.RegistedDate.ToString(),
                            ValueLabel = value.BodyWeight.ToString()
                        })
                        .ToList();
                    break;

                case BasicDataEnum.BodyFatPercentage:
                    entries = list.Select(value => new Entry(value.BodyFatPercentage)
                        {
                            Color = SKColor.Parse("#00CED1"),
                            Label = value.RegistedDate.ToString(),
                            ValueLabel = value.BodyFatPercentage.ToString()
                        })
                        .ToList();
                    break;

                    default: break;
            }

            DataChart.Chart = new LineChart { Entries = entries };
        }
    }
}
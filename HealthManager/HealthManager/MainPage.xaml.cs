using System.Collections.Generic;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace HealthManager
{
    public partial class MainPage : ContentPage
    {
        private List<Entry> entries = new List<Entry>
        {
            new Entry(200)
            {
                Color = SKColor.Parse("#FF1493"),
                Label = "January",
                ValueLabel = "200"
            },
            new Entry(400)
            {
                Color = SKColor.Parse("#00BFFF"),
                Label = "February",
                ValueLabel = "400"
            },
            new Entry(-100)
            {
                Color = SKColor.Parse("#00CED1"),
                Label = "March",
                ValueLabel = "-100"
            }
        };

        public MainPage()
        {
            InitializeComponent();
            Chart1.Chart = new RadialGaugeChart { Entries = entries };
        }
    }
}

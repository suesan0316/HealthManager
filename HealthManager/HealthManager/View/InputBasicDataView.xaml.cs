using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputBasicDataView : ContentPage
    {
        public InputBasicDataView()
        {
            InitializeComponent();
            BindingContext = new InputBasicDataViewModel();
        }
    }
}
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
    public partial class RegistBodyImageView : ContentPage
    {
        public RegistBodyImageView()
        {
            InitializeComponent();
            BindingContext = new RegistBodyImageViewModel();
        }
    }
}
using System;
using CommonServiceLocator;
using HealthManager.ViewModel;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputBasicDataView : ContentPage
    {
        public InputBasicDataView()
        {
            try
            {
                InitializeComponent();
                //BindingContext = BindingContext = ServiceLocator.Current.GetInstance(typeof(InputBasicDataViewModel));
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
                NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
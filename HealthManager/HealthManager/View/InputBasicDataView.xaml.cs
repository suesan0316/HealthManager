using HealthManager.Common;
using HealthManager.Common.Constant;
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
            MessagingCenter.Subscribe<ViewModelCommonUtil>(this, ViewModelConst.MessagingSelfScroll,
                (sender) => { ControlScroll.ScrollToAsync(ControlScroll, ScrollToPosition.Start, false); });
            if (Navigation != null)
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }
            var vm = new InputBasicDataViewModel {ErrorStack = ErrorStack.Children};
            BindingContext = vm;
        }
    }
}
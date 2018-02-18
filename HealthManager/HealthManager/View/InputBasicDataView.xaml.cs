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
            if (Navigation != null)
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }
            var vm = new InputBasicDataViewModel();
            vm.ErrorStack = ErrorStack.Children;            
            BindingContext = vm;
        }
    }
}
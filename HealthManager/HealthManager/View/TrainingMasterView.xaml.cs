using HealthManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TrainingMasterView : ContentPage
	{
	    public TrainingMasterView()
	    {
	        InitializeComponent();
	        NavigationPage.SetHasNavigationBar(this, false);
            var vm = new TrainingMasterViewModel(partStack:PartStack, loadStack:LoadStack) { ErrorStack = ErrorStack.Children };
	        BindingContext = vm;
	    }

	    public TrainingMasterView(int id)
	    {
	        InitializeComponent();
	        NavigationPage.SetHasNavigationBar(this, false);
	        var vm = new TrainingMasterViewModel(id:id, partStack: PartStack, loadStack: LoadStack) { ErrorStack = ErrorStack.Children };
            BindingContext = vm;
        }
    }
}
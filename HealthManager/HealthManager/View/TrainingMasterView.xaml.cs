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
	        BindingContext = new TrainingMasterViewModel();
	    }

	    public TrainingMasterView(int id)
	    {
	        InitializeComponent();
	        BindingContext = new TrainingMasterViewModel(id);
	    }
    }
}
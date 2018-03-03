using HealthManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditTrainingScheduleView : ContentPage
	{
		public EditTrainingScheduleView (int week)
		{
		    InitializeComponent();
		    NavigationPage.SetHasNavigationBar(this, false);
		    var vm = new EditTrainingScheduleViewModel(week: week, trainingStack: TrainingStack) { ErrorStack = ErrorStack.Children };
		    BindingContext = vm;
        }
	}
}
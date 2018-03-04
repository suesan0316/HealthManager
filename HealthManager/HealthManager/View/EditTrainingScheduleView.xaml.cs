using HealthManager.Common;
using HealthManager.Common.Constant;
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
		    MessagingCenter.Subscribe<ViewModelCommonUtil>(this, ViewModelConst.MessagingTrainingSelfScroll,
		        (sender) => { ControlScroll.ScrollToAsync(ControlScroll, ScrollToPosition.Start, false); });
            NavigationPage.SetHasNavigationBar(this, false);
		    var vm = new EditTrainingScheduleViewModel(week: week, trainingStack: TrainingStack) { ErrorStack = ErrorStack.Children };
		    BindingContext = vm;
        }
	}
}
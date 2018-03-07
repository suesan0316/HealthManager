using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TrainingReportListView : ContentPage
	{
		public TrainingReportListView ()
		{
			InitializeComponent ();
		    NavigationPage.SetHasNavigationBar(this, false);
        }
	}
}
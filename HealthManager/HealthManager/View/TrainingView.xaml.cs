using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TrainingView : ContentPage
	{
		public TrainingView ()
		{
			InitializeComponent ();
		    NavigationPage.SetHasNavigationBar(this, false);
        }
	}
}
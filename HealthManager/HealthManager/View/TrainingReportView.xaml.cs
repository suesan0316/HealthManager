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
	public partial class TrainingReportView : ContentPage
	{
		public TrainingReportView (int id)
		{
			InitializeComponent ();
		    NavigationPage.SetHasNavigationBar(this, false);
		    var vm = new TrainingReportViewModel(id);
		    BindingContext = vm;
        }
	}
}
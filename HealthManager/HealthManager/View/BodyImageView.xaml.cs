using HealthManager.Common.Constant;
using HealthManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BodyImageView : ContentPage
    {
        public BodyImageView()
        {
            InitializeComponent();
            MessagingCenter.Send(this, ViewModelConst.MessagingPassingView, this.ImageStack.Children);
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
using System.IO;
using HealthManager.DependencyInterface;
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
            BindingContext = new InputBasicDataViewModel();

            //テスト
            pickPictureButton.Clicked += async (sender, e) => DependencyService.Get<ICameraDependencyService>().BringUpCamera();

            takePictureButton.Clicked += async (sender, e) =>
                DependencyService.Get<ICameraDependencyService>().BringUpPhotoGallery();

            Image imageView = new Image();

            MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Set the source of the image view with the byte array
                    testImage.Source = ImageSource.FromStream(() => new MemoryStream((byte[])args));
                    int i = 0;
                });
            });
        }
    }
}
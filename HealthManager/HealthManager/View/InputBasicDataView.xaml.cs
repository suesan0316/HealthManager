using System;
using System.IO;
using HealthManager.DependencyInterface;
using HealthManager.Model.Service;
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
                    string base64String = Convert.ToBase64String((byte[]) args);
                    byte[] imageAsBytes =  Convert.FromBase64String(base64String);
                    testImage.Source = ImageSource.FromStream(() => new MemoryStream((byte[])imageAsBytes));
                    BodyImageService.RegistBodyImage(base64String);
                    int i = 0;
                });
            });
        }
    }
}
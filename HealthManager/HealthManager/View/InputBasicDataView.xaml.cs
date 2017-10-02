using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            pickPictureButton.Clicked += async (sender, e) =>
            {
                pickPictureButton.IsEnabled = false;
                Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

                if (stream != null)
                {
                    Image image = new Image
                    {
                        Source = ImageSource.FromStream(() => stream),
                        BackgroundColor = Color.Gray
                    };

                    TapGestureRecognizer recognizer = new TapGestureRecognizer();
                    /*recognizer.Tapped += (sender2, args) =>
                    {
                        (MainPage as ContentPage).Content = stack;
                        pickPictureButton.IsEnabled = true;
                    };
                    image.GestureRecognizers.Add(recognizer);

                    (MainPage as ContentPage).Content = image;*/
                }
                else
                {
                    pickPictureButton.IsEnabled = true;
                }
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthManager.Model.Service;
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

            Content.BindingContext = new BodyImageViewModel();

            var list = BodyImageService.GetBodyImageList();

            foreach (var value in list)
            {

                var stack = new StackLayout();
                var label = new Label{Text = value.RegistedDate.ToString()};

                var imageAsBytes = Convert.FromBase64String(value.ImageBase64String);

                var image = new Image
                {
                    HeightRequest = 400,
                    Source = ImageSource.FromStream(() => new MemoryStream(imageAsBytes))
                };

                stack.Children.Add(image);
                stack.Children.Add(label);

                ImageStack.Children.Add(stack);
            }

        }
    }
}
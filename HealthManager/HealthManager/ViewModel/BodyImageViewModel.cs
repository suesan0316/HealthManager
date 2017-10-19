using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Model.Service;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;

namespace HealthManager.ViewModel
{
    internal class BodyImageViewModel : INotifyPropertyChanged
    {
        public BodyImageViewModel()
        {
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);
            InitImageStackLayout();
        }

        public ICommand BackHomeCommand { get; set; }

        public ObservableCollection<XView> BodyImageContents { set; get; } =
            new ObservableCollection<XView>();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitImageStackLayout()
        {
            var bodyImageModels = BodyImageService.GetBodyImageList();
            foreach (var value in bodyImageModels)
            {
                var childStackLayout = new StackLayout();

                var imageAsBytes = Convert.FromBase64String(value.ImageBase64String);
                var bodyImage = new Image
                {
                    HeightRequest = 400,
                    Source = ImageSource.FromStream(() => new MemoryStream(imageAsBytes))
                };
                /*var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) => {
                    System.Diagnostics.Debug.WriteLine("aaa");
                };
                bodyImage.GestureRecognizers.Add(tapGestureRecognizer);*/
                childStackLayout.Children.Add(bodyImage);

                var registedDateLabel = new Label { Text = value.RegistedDate.ToString() };
                childStackLayout.Children.Add(registedDateLabel);

                BodyImageContents.Add(childStackLayout);
            }
        }
    }
}
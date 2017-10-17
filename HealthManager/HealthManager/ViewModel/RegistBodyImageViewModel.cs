using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.DependencyInterface;
using HealthManager.Model.Service;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class RegistBodyImageViewModel : INotifyPropertyChanged
    {
        private string _base64String;

        private ImageSource _bodyImage;

        public RegistBodyImageViewModel()
        {
            TakeImageCameraCommand = new Command(TakeImageCamera);
            TakeImageLibraryaCommand = new Command(TakeImageLibrary);
            RegistBodyImageCommand = new Command(RegistBodyImage);
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);

            MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", args =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _base64String = Convert.ToBase64String(args);
                    var imageAsBytes = Convert.FromBase64String(_base64String);
                    BodyImage = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                });
            });
        }

        public ICommand TakeImageCameraCommand { get; set; }
        public ICommand TakeImageLibraryaCommand { get; set; }
        public ICommand RegistBodyImageCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }

        public ImageSource BodyImage
        {
            get => _bodyImage;
            set
            {
                _bodyImage = value;
                RegistButtonIsVisible = true;
                OnPropertyChanged(nameof(BodyImage));
                OnPropertyChanged(nameof(RegistButtonIsVisible));
            }
        }

        public bool RegistButtonIsVisible { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TakeImageCamera()
        {
            DependencyService.Get<ICameraDependencyService>().BringUpCamera();
        }

        private void TakeImageLibrary()
        {
            DependencyService.Get<ICameraDependencyService>().BringUpPhotoGallery();
        }

        private void RegistBodyImage()
        {
            BodyImageService.RegistBodyImage(_base64String);
            ViewModelCommonUtil.BackHome();
        }
    }
}
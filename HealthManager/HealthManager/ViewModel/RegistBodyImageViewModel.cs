using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        private readonly int _id;

        public RegistBodyImageViewModel()
        {
            TakeImageCameraCommand = new Command(TakeImageCamera);
            TakeImageLibraryaCommand = new Command(TakeImageLibrary);
            RegistBodyImageCommand = new Command(async () => await RegistBodyImage());
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);

            var model = BodyImageService.GetBodyImage();
            if (model != null)
                _id = model.Id;
            else
                _id = 0;

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

        private async Task RegistBodyImage()
        {
            if (BodyImageService.CheckExitTargetDayData(DateTime.Now))
            {
                var result =
                    await Application.Current.MainPage.DisplayAlert("確認", "本日は既にデータを登録しています。更新しますか？", "OK", "キャンセル");
                if (result)
                    BodyImageService.UpdateBodyImage(_id, _base64String);
            }
            else
            {
                BodyImageService.RegistBodyImage(_base64String);
            }
            ViewModelCommonUtil.BackHome();
        }
    }
}
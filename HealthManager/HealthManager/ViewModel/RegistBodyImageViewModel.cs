using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.Properties;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class RegistBodyImageViewModel : INotifyPropertyChanged
    {
        private const string BodyImageFileDirectory = "BodyImage";
        private const string BodyImageNameHead = "bodyImage_";
        private const string BodyImageExtension = ".jpg";
        private readonly int _id;

        private string _base64String;

        private ImageSource _bodyImage;
        private bool _takePhotoFromCamera;

        public RegistBodyImageViewModel()
        {
            TakeImageCameraCommand = new Command(async () => await TakeImageCamera());
            TakeImageLibraryaCommand = new Command(async () => await TakeImageLibrary());
            RegistBodyImageCommand = new Command(async () => await RegistBodyImage());
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);

            var model = BodyImageService.GetBodyImage();
            _id = model?.Id ?? 0;
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

        private async Task TakeImageCamera()
        {
            // TODO カメラから写真を登録した場合は撮った写真は削除する処理を追加する
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Error),
                        LanguageUtils.Get(LanguageKeys.CameraNotAvailable), LanguageUtils.Get(LanguageKeys.OK));
                    return;
                }

                var fileName = BodyImageNameHead + ViewModelCommonUtil.FormatDateStringWithoutSymbol(DateTime.Now) +
                               BodyImageExtension;
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    Directory = BodyImageFileDirectory,
                    Name = fileName,
                    SaveToAlbum = false
                });

                if (file == null)
                    return;

                BodyImage = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });

                var imgData = ViewModelCommonUtil.ConvertToByteArrayFromStream(file.GetStream());
                _base64String = Convert.ToBase64String(imgData);

                _takePhotoFromCamera = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private async Task TakeImageLibrary()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Error),
                    LanguageUtils.Get(LanguageKeys.PictureLibraryCanNotOpen), LanguageUtils.Get(LanguageKeys.OK));
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium
            });


            if (file == null)
                return;

            BodyImage = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

            var imgData = ViewModelCommonUtil.ConvertToByteArrayFromStream(file.GetStream());
            _base64String = Convert.ToBase64String(imgData);

            _takePhotoFromCamera = false;
        }

        private async Task RegistBodyImage()
        {
            if (BodyImageService.CheckExitTargetDayData(DateTime.Now))
            {
                var result =
                    await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                        LanguageUtils.Get(LanguageKeys.TodayDataUpdateConfirm), LanguageUtils.Get(LanguageKeys.OK),
                        LanguageUtils.Get(LanguageKeys.Cancel));
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
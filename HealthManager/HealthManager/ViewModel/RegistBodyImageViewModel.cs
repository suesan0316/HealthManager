using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.Properties;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// 体格画像登録画面VMクラス
    /// </summary>
    public class RegistBodyImageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 画像ファイル格納フォルダ
        /// </summary>
        private const string BodyImageFileDirectory = "BodyImage";

        /// <summary>
        /// 画像ファイル名ヘッダ
        /// </summary>
        private const string BodyImageNameHead = "bodyImage_";

        /// <summary>
        /// 画像ファイル拡張子
        /// </summary>
        private const string BodyImageExtension = ".jpg";

        /// <summary>
        /// 体格画像ID
        /// </summary>
        private readonly int _id;

        /// <summary>
        /// 画像Base64変換文字
        /// </summary>
        private string _base64String;

        /// <summary>
        /// カメラで撮影またはライブラリから選択された画像
        /// </summary>
        private ImageSource _bodyImage;

        /// <summary>
        /// カメラ選択フラグ
        /// </summary>
        private bool _takePhotoFromCamera;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RegistBodyImageViewModel()
        {
            TakeImageCameraCommand = new Command(async () => await TakeImageCamera());
            TakeImageLibraryaCommand = new Command(async () => await TakeImageLibrary());
            RegistBodyImageCommand = new Command(async () => await RegistBodyImage());
            BackHomeCommand = new Command(ViewModelCommonUtil.DataBackPage);

            var model = BodyImageService.GetBodyImage();
            _id = model?.Id ?? 0;
        }

        /// <summary>
        /// カメラで撮るボタンコマンド
        /// </summary>
        public ICommand TakeImageCameraCommand { get; set; }

        /// <summary>
        /// ライブラリから選択するボタンコマンド
        /// </summary>
        public ICommand TakeImageLibraryaCommand { get; set; }

        /// <summary>
        /// 登録するボタンコマンド
        /// </summary>
        public ICommand RegistBodyImageCommand { get; set; }

        /// <summary>
        /// キャンセルボタンコマンド
        /// </summary>
        public ICommand BackHomeCommand { get; set; }

        /// <summary>
        /// カメラで撮影またはライブラリから選択された画像
        /// </summary>
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

        /// <summary>
        /// 登録ボタン表示フラグ
        /// </summary>
        public bool RegistButtonIsVisible { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// カメラで取るボタンラベル
        /// </summary>
        public string TakeImageCameraLabel => LanguageUtils.Get(LanguageKeys.SnapFromCamera);

        /// <summary>
        /// ライブラリから選択するボタンラベル
        /// </summary>
        public string TakeImageLibraryLabel => LanguageUtils.Get(LanguageKeys.SelectFromLibrary);

        /// <summary>
        /// 登録するボタンラベル
        /// </summary>
        public string RegistButtonLabel => LanguageUtils.Get(LanguageKeys.Regist);

        /// <summary>
        /// キャンセルボタンラベル
        /// </summary>
        public string CancelButtonLabel => LanguageUtils.Get(LanguageKeys.Cancel);

        /// <summary>
        /// カメラ撮影アクション
        /// </summary>
        /// <returns></returns>
        private async Task TakeImageCamera()
        {
            // TODO カメラから写真を登録した場合は撮った写真は削除する処理を追加する
            try
            {
                // カメラが有効であるか判定
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Error),
                        LanguageUtils.Get(LanguageKeys.CameraNotAvailable), LanguageUtils.Get(LanguageKeys.OK));
                    return;
                }

                // 画像ファイルの名前を決定
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

        /// <summary>
        /// フォトライブラリ選択アクション
        /// </summary>
        /// <returns></returns>
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

            // ホーム画面をリロードする
            MessagingCenter.Send(this, ViewModelConst.MessagingHomeReload);
            ViewModelCommonUtil.DataBackPage();
        }
    }
}
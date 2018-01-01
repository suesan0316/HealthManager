using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.Properties;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// 体格画像表示画面VMクラス
    /// </summary>
    internal class BodyImageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BodyImageViewModel()
        {
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);
            InitImageStackLayout();
        }

        /// <summary>
        /// 戻るボタンコマンド
        /// </summary>
        public ICommand BackHomeCommand { get; set; }

        /// <summary>
        /// 体格画像リスト
        /// </summary>
        public ObservableCollection<XView> BodyImageContents { set; get; } =　new ObservableCollection<XView>();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 戻るボタンラベル
        /// </summary>
        public string BackHomeLabel => LanguageUtils.Get(LanguageKeys.Return);

        /// <summary>
        /// 画面に表示するレイアウトを生成
        /// </summary>
        private void InitImageStackLayout()
        {
            var bodyImageModels = BodyImageService.GetBodyImageList();
            foreach (var value in bodyImageModels)
            {
                var childStackLayout = new StackLayout(){};
                var imageAsBytes = Convert.FromBase64String(value.ImageBase64String);
                var bodyImage = new Image
                {
                    Source = ImageSource.FromStream(() => new MemoryStream(ViewModelCommonUtil.GetResizeImageBytes(imageAsBytes, 500, 625)))
                };
                childStackLayout.Children.Add(bodyImage);

                var registedDateLabel = new Label { Text = value.RegistedDate.ToString(), HorizontalOptions = LayoutOptions.Center };
                childStackLayout.Children.Add(registedDateLabel);

                BodyImageContents.Add(childStackLayout);
            }
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Common;
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
        public BodyImageViewModel()
        {
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);
            InitImageStackLayout();
        }

        public ICommand BackHomeCommand { get; set; }

        public ObservableCollection<XView> BodyImageContents { set; get; } =　new ObservableCollection<XView>();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 画面に表示するレイアウトを生成
        /// </summary>
        private void InitImageStackLayout()
        {
            var bodyImageModels = BodyImageService.GetBodyImageList();
            foreach (var value in bodyImageModels)
            {
                var childStackLayout = new StackLayout();
                var imageAsBytes = Convert.FromBase64String(value.ImageBase64String);
                var bodyImage = new Image
                {
                    // TODO 高さは調整
                    HeightRequest = 400,
                    Source = ImageSource.FromStream(() => new MemoryStream(imageAsBytes))
                };
                childStackLayout.Children.Add(bodyImage);

                var registedDateLabel = new Label { Text = value.RegistedDate.ToString() };
                childStackLayout.Children.Add(registedDateLabel);

                BodyImageContents.Add(childStackLayout);
            }
        }
    }
}
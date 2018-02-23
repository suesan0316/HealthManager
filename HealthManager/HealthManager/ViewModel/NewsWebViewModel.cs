using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Language;
using HealthManager.Properties;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///  ニュース画面VMクラス
    /// </summary>
    internal class NewsWebViewModel : INotifyPropertyChanged

    {
        /// <summary>
        /// 表示するサイトのURL
        /// </summary>
        private string _webSource;

        private readonly INavigation _navigation;

        /// <summary>
        /// デフォルトのコンストラクタは使用しない想定
        /// </summary>
        public NewsWebViewModel()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="url"></param>
        /// <param name="navigation"></param>
        public NewsWebViewModel(string url,INavigation navigation)
        {
            BackHomeCommand = new Command(ReturnHome);
            this._navigation = navigation;
            WebSource = url;
        }

        /// <summary>
        /// 戻るボタンコマンド
        /// </summary>
        public ICommand BackHomeCommand { get; set; }

        /// <summary>
        /// 表示するサイトのURL
        /// </summary>
        public string WebSource
        {
            get => _webSource;
            set
            {
                _webSource = value;
                OnPropertyChanged(nameof(WebSource));
            }
        }

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

        private void ReturnHome()
        {
            _navigation.PopAsync();
        }
    }
}
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
        public NewsWebViewModel(string url)
        {
            BackHomeCommand = new Command(ViewModelCommonUtil.DataBackPage);
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
    }
}
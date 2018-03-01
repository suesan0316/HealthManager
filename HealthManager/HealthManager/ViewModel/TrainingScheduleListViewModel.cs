using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Language;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///  トレーニングスケジュール一覧画面VMクラス
    /// </summary>
    public class TrainingScheduleListViewModel : INotifyPropertyChanged
    {
        public TrainingScheduleListViewModel()
        {
            BackPageCommand = new Command(ViewModelCommonUtil.TrainingBackPage);
        }

        /// <summary>
        /// 戻るボタンコマンド
        /// </summary>
        public ICommand BackPageCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 戻るボタンラベル
        /// </summary>
        public string BackPageLabel => LanguageUtils.Get(LanguageKeys.Return);

    }
}

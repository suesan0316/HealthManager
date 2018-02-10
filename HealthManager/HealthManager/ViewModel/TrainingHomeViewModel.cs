using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Common.Language;
using HealthManager.Properties;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     トレーニングホーム画面VMクラス
    /// </summary>
    public class TrainingHomeViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TrainingHomeViewModel()
        {
            AddTraningCommand = new Command(MoveToTrainingMaster);
            EditTrainingScheduleCommand = new Command(MoveToTrainingSchedule);
            StartTrainingCommand = new Command(MoveToTraining);
            EditTrainingCommand = new Command(MoveToTrainingList);
        }

        /// <summary>
        ///     トレーニングを追加するコマンド
        /// </summary>
        public ICommand AddTraningCommand { get; set; }

        /// <summary>
        ///     トレーニングを編集するコマンド
        /// </summary>
        public ICommand EditTrainingCommand { get; set; }

        /// <summary>
        ///     トレーニングスケジュールを編集するコマンド
        /// </summary>
        public ICommand EditTrainingScheduleCommand { get; set; }

        /// <summary>
        ///     トレーニングを開始するコマンド
        /// </summary>
        public ICommand StartTrainingCommand { get; set; }

        /// <summary>
        ///     トレーニングを追加するボタンラベル
        /// </summary>
        public string AddTrainingLabel => LanguageUtils.Get(LanguageKeys.AddTraining);

        /// <summary>
        ///     トレーニングスケジュールを編集するボタンラベル
        /// </summary>
        public string EditTrainingScheduleLabel => LanguageUtils.Get(LanguageKeys.EditTrainingSchedule);

        /// <summary>
        ///     トレーニングスケジュールを編集するボタンラベル
        /// </summary>
        public string EditTrainingLabel => LanguageUtils.Get(LanguageKeys.EditTraining);

        /// <summary>
        ///     トレーニングを開始するボタンラベル
        /// </summary>
        public string StrartTrainingLabel => LanguageUtils.Get(LanguageKeys.StartTraining);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     トレーニングマスター画面遷移
        /// </summary>
        private static void MoveToTrainingMaster()
        {
            ((App) Application.Current).ChangeScreen(new TrainingMasterView());
        }

        private static void MoveToTrainingList()
        {
            ((App)Application.Current).ChangeScreen(new TrainingListView());
        }

        /// <summary>
        ///     トレーニングスケジュール画面遷移
        /// </summary>
        private static void MoveToTrainingSchedule()
        {
            ((App) Application.Current).ChangeScreen(new EditTrainingScheduleView());
        }

        /// <summary>
        ///     トレーニング画面遷移
        /// </summary>
        private static void MoveToTraining()
        {
            ((App) Application.Current).ChangeScreen(new TrainingView());
        }
    }
}
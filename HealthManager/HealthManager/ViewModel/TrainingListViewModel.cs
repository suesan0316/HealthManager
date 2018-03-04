using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// トレーニング一覧画面VMクラス
    /// </summary>
    public class TrainingListViewModel : INotifyPropertyChanged
    {
        public TrainingListViewModel()
        {
            // 遷移先画面から戻ってきた際に情報をリロードする
            MessagingCenter.Subscribe<ViewModelCommonUtil>(this, ViewModelConst.MessagingTrainingPrevPageReload,
                (sender) => { ReloadList(); });

            BackPageCommand = new Command(ViewModelCommonUtil.TrainingBackPage);
            TrainingAddCommand = new Command(MoveToTrainingMaster);
            TrainingMasterItemTappedCommand = new Command<TrainingMasterModel>(item =>
            {
                ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingMasterView(item.Id));
            });
            var items = TrainingMasterService.GetTrainingMasterDataList();
            items?.ForEach(data => Items.Add(data));
        }

        /// <summary>
        /// トレーニングリストアイテムソース
        /// </summary>
        public ObservableCollection<TrainingMasterModel> Items { protected set; get; } = new ObservableCollection<TrainingMasterModel>();

        /// <summary>
        /// 戻るボタンコマンド
        /// </summary>
        public ICommand BackPageCommand { get; set; }

        /// <summary>
        /// トレーニングを追加するコマンド
        /// </summary>
        public ICommand TrainingAddCommand { get; set; }

        /// <summary>
        /// トレーニングリストタップコマンド
        /// </summary>
        public ICommand TrainingMasterItemTappedCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// トレーニングを追加するボタンラベル
        /// </summary>
        public string TrainingAddLabel => LanguageUtils.Get(LanguageKeys.AddTraining);

        /// <summary>
        /// 戻るボタンラベル
        /// </summary>
        public string BackPageLabel => LanguageUtils.Get(LanguageKeys.Return);

        /// <summary>
        ///     トレーニングマスター画面遷移
        /// </summary>
        private static void MoveToTrainingMaster()
        {
            ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingMasterView());
//            ((App)Application.Current).ChangeScreen(new TrainingMasterView());
        }

        public void ReloadList()
        {
            Items.Clear();
            var items = TrainingMasterService.GetTrainingMasterDataList();
            items?.ForEach(data => Items.Add(data));
        }
    }
}

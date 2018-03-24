using System.Collections.ObjectModel;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.View;
using PropertyChanged;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     トレーニング一覧画面VMクラス
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class TrainingListViewModel
    {
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Constractor

        public TrainingListViewModel()
        {
            InitMessageSubscribe();
            InitCommands();
            var items = TrainingMasterService.GetTrainingMasterDataList();
            items?.ForEach(data => Items.Add(data));
        }

        #endregion Constractor

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Binding Variables

        /// <summary>
        ///     トレーニングリストアイテムソース
        /// </summary>
        public ObservableCollection<TrainingMasterModel> Items { protected set; get; } =
            new ObservableCollection<TrainingMasterModel>();

        #endregion Binding Variables

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Command Actions

        /// <summary>
        ///     トレーニングマスター画面遷移
        /// </summary>
        private static void CommandTrainingAddAction()
        {
            ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingMasterView());
        }

        #endregion Command Actions

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Init Commands

        private void InitCommands()
        {
            CommandReturn = new Command(ViewModelCommonUtil.TrainingBackPage);
            CommandTrainingAdd = new Command(CommandTrainingAddAction);
            CommandTrainingMasterItemTapped = new Command<TrainingMasterModel>(item =>
            {
                ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingMasterView(item.Id));
            });
        }

        #endregion Init Commands

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region ViewModel Logic

        public void ReloadList()
        {
            Items.Clear();
            var items = TrainingMasterService.GetTrainingMasterDataList();
            items?.ForEach(data => Items.Add(data));
        }

        #endregion ViewModel Logic

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init MessageSubscribe
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Init MessageSubscribe

        private void InitMessageSubscribe()
        {
            MessagingCenter.Subscribe<ViewModelCommonUtil>(this, ViewModelConst.MessagingTrainingPrevPageReload,
                sender => { ReloadList(); });
        }

        #endregion Init MessageSubscribe

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Class Variable
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Class Variable

        #endregion Class Variable

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Instance Private Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Instance Private Variables

        #endregion Instance Private Variables

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Binding DisplayLabels

        public string DisplayLabelReturn => LanguageUtils.Get(LanguageKeys.Return);
        public string DisplayLabelTrainingAdd => LanguageUtils.Get(LanguageKeys.AddTraining);

        #endregion Binding DisplayLabels

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Binding Commands

        /// <summary>
        ///     戻るボタンコマンド
        /// </summary>
        public ICommand CommandReturn { get; set; }

        /// <summary>
        ///     トレーニングを追加するコマンド
        /// </summary>
        public ICommand CommandTrainingAdd { get; set; }

        /// <summary>
        ///     トレーニングリストタップコマンド
        /// </summary>
        public ICommand CommandTrainingMasterItemTapped { get; set; }

        #endregion Binding Commands

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Default 
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Default

        #endregion Default
    }
}
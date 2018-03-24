using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Language;
using HealthManager.View;
using HealthManager.ViewModel.Structure;
using PropertyChanged;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///  トレーニングスケジュール一覧画面VMクラス
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class TrainingScheduleListViewModel
    {
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

        private List<WeekEnum> _weekList;

        #endregion Instance Private Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Constractor

        public TrainingScheduleListViewModel()
        {           
            InitMessageSubscribe();
            InitCommands();
            LoadList();
        }

        #endregion Constractor
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Variables

        public ObservableCollection<TrainingScheduleSViewtructure> Items { protected set; get; } =
            new ObservableCollection<TrainingScheduleSViewtructure>();

        #endregion Binding Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding DisplayLabels

        public string DisplayLabelReturn => LanguageUtils.Get(LanguageKeys.Return);

        #endregion Binding DisplayLabels
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Commands

        public ICommand CommandReturn { get; set; }
        public ICommand CommandTrainingScheduleListItemTapped { get; set; }

        #endregion Binding Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Command Actions

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
            CommandTrainingScheduleListItemTapped = new Command<TrainingScheduleSViewtructure>(item =>
            {
                ViewModelConst.TrainingPageNavigation.PushAsync(new EditTrainingScheduleView(item.Week));
            });
        }

        #endregion Init Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region ViewModel Logic

        public void LoadList()
        {
            Items.Clear();
            _weekList = new List<WeekEnum>();
            foreach (var week in Enum.GetValues(typeof(WeekEnum)))
            {
                _weekList.Add((WeekEnum)week);
                Items.Add(ViewModelCommonUtil.CreateTrainingScheduleSViewtructure((WeekEnum)week));
            }
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
            // 遷移先画面から戻ってきた際に情報をリロードする
            MessagingCenter.Subscribe<ViewModelCommonUtil>(this, ViewModelConst.MessagingTrainingPrevPageReload,
                (sender) => { LoadList(); });
        }

        #endregion Init MessageSubscribe
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Default 
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Default

        #endregion Default       
    }
}
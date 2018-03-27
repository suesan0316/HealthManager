using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.ViewModel.Structure;
using HealthManager.ViewModel.Util;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// トレーニング画面VMクラス
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class TrainingViewModel
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

        private readonly TrainingScheduleSViewtructure _trainingScheduleSViewtructure;
        private readonly DateTime _pageOpenDateTime;
        private DateTime _trainingStartDateTime;
        private DateTime _trainingEndDateTime;
        private readonly Timer _timer;
        private TimeSpan _totalSeconds = new TimeSpan();

        #endregion Instance Private Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Constractor

        public TrainingViewModel()
        {
            InitCommands();
            _pageOpenDateTime = DateTime.Now;
            var week = (int)DateTime.Now.DayOfWeek;
            _timer = new Timer(TimeSpan.FromSeconds(1), CountDown);
            TotalSeconds = _totalSeconds;
            _trainingScheduleSViewtructure = ViewModelCommonUtil.CreateTrainingScheduleSViewtructure((WeekEnum)week);
            TrainingMenu = _trainingScheduleSViewtructure.ToString();
            Items = _trainingScheduleSViewtructure.TrainingContentList;
            WeekLabel = _trainingScheduleSViewtructure.WeekName;
            TrainingStartButtonVisible = true;
        }

        #endregion Constractor
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Variables

        public TimeSpan TotalSeconds { get; set; }
        public string WeekLabel { get; set; }
        public string TrainingMenu { get; set; }
        public List<TrainingListViewStructure> Items { protected set; get; }
        public string TimeCount => TotalSeconds.ToString();
        public bool TimeCountVisible { get; set; }
        public bool TrainingStartButtonVisible { get; set; }
        public bool TrainingCompleteButtonVisible { get; set; }

        #endregion Binding Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding DisplayLabels

        public string DisplayLabelTrainingMenu => LanguageUtils.Get(LanguageKeys.TodayTrainingMenu);
        public string DisplayLabelTimeCount => LanguageUtils.Get(LanguageKeys.TrainingTime);
        public string DisplayLabelTrainingStart => LanguageUtils.Get(LanguageKeys.StartTraining);
        public string DisplayLabelCancel => LanguageUtils.Get(LanguageKeys.Cancel);
        public string DisplayLabelTrainingComplete => LanguageUtils.Get(LanguageKeys.CompleteTraining);

        #endregion Binding DisplayLabels
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Commands

        public ICommand CommandTrainingStart { get; set; }
        public ICommand CommandCacel { get; set; }
        public ICommand CommandTrainingComplete { get; set; }

        #endregion Binding Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Command Actions

        public void CommandCancelAction()
        {
            // 遷移元画面をリロードする
            ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingTrainingHomeReload);
            ViewModelConst.TrainingPageNavigation.PopAsync();
        }

        public void CommandTrainingStartAction()
        {
            TrainingStartButtonVisible = false;
            TrainingCompleteButtonVisible = true;
            TimeCountVisible = true;
            _trainingStartDateTime = DateTime.Now;
            _timer.Start();
        }

        private async Task CommandCompleteTrainingAction()
        {
            PauseTimer();

            var result =
                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                    LanguageUtils.Get(LanguageKeys.IsDoneTraining), LanguageUtils.Get(LanguageKeys.OK),
                    LanguageUtils.Get(LanguageKeys.Cancel));
            if (result)
            {
                _trainingEndDateTime = DateTime.Now;
                TrainingResultService.RegistTrainingResult(trainingContent: JsonConvert.SerializeObject(_trainingScheduleSViewtructure), weather: null,
                    targetDate: _pageOpenDateTime, startDate: _trainingStartDateTime, endDate: _trainingEndDateTime);

                // 遷移元画面をリロードする
                ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingTrainingHomeReload);
                await ViewModelConst.TrainingPageNavigation.PopAsync();
            }
            else
            {
                _timer.Start();
            }

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
            CommandCacel = new Command(CommandCancelAction);
            CommandTrainingStart = new Command(CommandTrainingStartAction);
            CommandTrainingComplete = new Command(async () => await CommandCompleteTrainingAction());
        }

        #endregion Init Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region ViewModel Logic

        private void CountDown()
        {
            TotalSeconds = _totalSeconds.Add(new TimeSpan(0, 0, 0, 1));
        }

        /// <summary>
        /// Pauses the timer
        /// </summary>
        private void PauseTimer()
        {
            _timer.Stop();
        }

        #endregion ViewModel Logic
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init MessageSubscribe
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Init MessageSubscribe

        //private void InitMessageSubscribe()
        //{

        //}

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


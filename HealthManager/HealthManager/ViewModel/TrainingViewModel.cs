using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.ViewModel.Structure;
using HealthManager.ViewModel.Util;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// トレーニング画面VMクラス
    /// </summary>
    public class TrainingViewModel : INotifyPropertyChanged
    {

        private int _week;

        private TrainingScheduleSViewtructure trainingScheduleSViewtructure;

        private DateTime _pageOpenDateTime;

        private DateTime _trainingStartDateTime;

        private DateTime _trainingEndDateTime;

        private Timer _timer;

        private TimeSpan _totalSeconds = new TimeSpan();

        public TimeSpan TotalSeconds
        {
            get { return _totalSeconds; }
            set
            {
                _totalSeconds = value;
                OnPropertyChanged(nameof(TotalSeconds));
                OnPropertyChanged(nameof(TimeCount));
            }
        }

        public TrainingViewModel()
        {
            CancleCommand = new Command(CancelAction);
            TrainingStartCommand = new Command(TrainingStart);
            TrainingCompleteCommand = new Command(async()=>await  CompleteTraining());
            _pageOpenDateTime = DateTime.Now;
            _week = (int) DateTime.Now.DayOfWeek;
            _timer = new Timer(TimeSpan.FromSeconds(1), CountDown);
            TotalSeconds = _totalSeconds;
            trainingScheduleSViewtructure = ViewModelCommonUtil.CreateTrainingScheduleSViewtructure((WeekEnum) _week);
            TrainingMenu = trainingScheduleSViewtructure.ToString();
            Items = trainingScheduleSViewtructure.TrainingContentList;
            WeekLabel = trainingScheduleSViewtructure.WeekName;
            TrainingStartButtonVisible = true;
        }

        public string TrainingMenuLabel => LanguageUtils.Get(LanguageKeys.TodayTrainingMenu);

        public string WeekLabel { get; set; }

        public string TrainingMenu { get; set; }

        public string TimeCountLabel => LanguageUtils.Get(LanguageKeys.TrainingTime);

        public string TimeCount => TotalSeconds.ToString();

        public string TrainingStartButtonLabel => LanguageUtils.Get(LanguageKeys.StartTraining);

        public string CancelButtonLabel => LanguageUtils.Get(LanguageKeys.Cancel);

        public string TrainingCompleteButtonLabel => LanguageUtils.Get(LanguageKeys.CompleteTraining);

        public bool TimeCountVisible { get; set; }

        public bool TrainingStartButtonVisible { get; set; }

        public bool TrainingCompleteButtonVisible { get; set; }

        public ICommand TrainingStartCommand { get; set; }

        public ICommand CancleCommand { get; set; }

        public ICommand TrainingCompleteCommand { get; set; }

        public List<TrainingListViewStructure> Items { protected set; get; } = new List<TrainingListViewStructure>();


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void TrainingStart()
        {
            TrainingStartButtonVisible = false;
            TrainingCompleteButtonVisible = true;
            TimeCountVisible = true;
            _trainingStartDateTime = DateTime.Now;
            OnPropertyChanged(nameof(TrainingStartButtonVisible));
            OnPropertyChanged(nameof(TrainingCompleteButtonVisible));
            OnPropertyChanged(nameof(TimeCountVisible));
            _timer.Start();
        }

        public void CancelAction()
        {
            // 遷移元画面をリロードする
            ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingTrainingHomeReload);
            ViewModelConst.TrainingPageNavigation.PopAsync();
        }

        private void CountDown()
        {
                TotalSeconds = _totalSeconds.Add(new TimeSpan(0, 0, 0, 1));
        }

        private async Task CompleteTraining()
        {
            _timer.Stop();

            var result =
                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                    LanguageUtils.Get(LanguageKeys.IsDoneTraining), LanguageUtils.Get(LanguageKeys.OK),
                    LanguageUtils.Get(LanguageKeys.Cancel));
            if (result)
            {
                _trainingEndDateTime = DateTime.Now;
                TrainingResultService.RegistTrainingResult(trainingContent: JsonConvert.SerializeObject(trainingScheduleSViewtructure), weather: null,
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

        /// <summary>
        /// Pauses the timer
        /// </summary>
        private void PauseTimerCommand()
        {
            _timer.Stop();
        }
    }
}


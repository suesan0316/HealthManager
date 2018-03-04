using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Language;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// トレーニング画面VMクラス
    /// </summary>
    public class TrainingViewModel : INotifyPropertyChanged
    {

        private int _week;

        public TrainingViewModel()
        {
            CancleCommand = new Command(CancelAction);
            _week = (int) DateTime.Now.DayOfWeek;

            TrainingMenu = ViewModelCommonUtil.CreateTrainingScheduleSViewtructure((WeekEnum) _week).ToString();
        }

        public string TrainingMenuLabel { get; set; }

        public string TrainingMenu { get; set; }

        public string TimeCountLabel { get; set; }

        public string TimeCount { get; set; }

        public string TrainingStartButtonLabel => LanguageUtils.Get(LanguageKeys.StartTraining);

        public string CancelButtonLabel => LanguageUtils.Get(LanguageKeys.Cancel);

        public ICommand TrainingStartCommand { get; set; }

        public ICommand CancleCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void CancelAction()
        {
            // 遷移元画面をリロードする
            MessagingCenter.Send(this, ViewModelConst.MessagingHomeReload);
            ViewModelConst.TrainingPageNavigation.PopAsync();
        }
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using HealthManager.Annotations;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// トレーニングスケジュール編集画面VM
    /// </summary>
    public class EditTrainingScheduleViewModel :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

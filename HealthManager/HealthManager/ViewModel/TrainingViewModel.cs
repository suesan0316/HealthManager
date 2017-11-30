using System.ComponentModel;
using System.Runtime.CompilerServices;
using HealthManager.Annotations;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// トレーニング画面VMクラス
    /// </summary>
    public class TrainingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

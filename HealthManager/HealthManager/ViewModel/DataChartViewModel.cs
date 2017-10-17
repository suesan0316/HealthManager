using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    class DataChartViewModel : INotifyPropertyChanged
    {

        public ICommand BackHomeCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DataChartViewModel()
        {
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common.Extention;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class TrainingListViewModel : INotifyPropertyChanged
    {
        public TrainingListViewModel()
        {
            TrainingMasterItemTappedCommand = new Command<TrainingMasterModel>(item =>
            {
                ((App)Application.Current).ChangeScreen(new TrainingMasterView(item.Id));
            });
            TrainingMasterService.GetTrainingMasterDataList().ForEach(data=>Items.Add(data));
        }

        public ObservableCollection<TrainingMasterModel> Items { protected set; get; } = new ObservableCollection<TrainingMasterModel>();

        public ICommand TrainingMasterItemTappedCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

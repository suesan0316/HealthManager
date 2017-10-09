using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    class BodyImageViewModel : INotifyPropertyChanged
    {

        public ICommand BackHomeCommand { get; set; }

        public BodyImageViewModel()
        {
            BackHomeCommand = new Command(BackHome);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BackHome()
        {
            ((App)Application.Current).ChangeScreen(new HomeView());
        }

    }
}

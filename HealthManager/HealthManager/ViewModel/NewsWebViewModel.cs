using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    internal class NewsWebViewModel : INotifyPropertyChanged

    {
        private string _webSource;

        public NewsWebViewModel()
        {
            BackHomeCommand = new Command(BackHome);
        }

        public NewsWebViewModel(string url)
        {
            BackHomeCommand = new Command(BackHome);
            WebSource = url;
        }

        public ICommand BackHomeCommand { get; set; }

        public string WebSource
        {
            get => _webSource;
            set
            {
                _webSource = value;
                OnPropertyChanged(nameof(WebSource));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BackHome()
        {
            ((App) Application.Current).ChangeScreen(new HomeView());
        }
    }
}
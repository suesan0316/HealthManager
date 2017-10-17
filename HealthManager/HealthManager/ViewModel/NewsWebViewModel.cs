using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    internal class NewsWebViewModel : INotifyPropertyChanged

    {
        private string _webSource;

        public NewsWebViewModel()
        {
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);
        }

        public NewsWebViewModel(string url)
        {
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);
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
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Extention;
using HealthManager.Logic.News.Factory;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<string> Items { protected set; get; } = new ObservableCollection<string>();

        public Dictionary<string, string> ItemsDictionary;

        public ICommand ItemTappedCommand { get; set; }

        public HomeViewModel()
        {
            
            ItemTappedCommand = new Command<string>((item) =>
            {
                Device.OpenUri(new Uri(ItemsDictionary[item]));
            });

            SetNewsSourceTask();
        }

        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 読み込み中フラグ
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private async Task SetNewsSourceTask()
        {
            IsLoading = true;

            var service = NewsServiceFactory.CreateYomiuriNewsService();

            ItemsDictionary = await service.GetNewsDictionary();

            ItemsDictionary.ForEach(data => Items.Add(data.Key));

            IsLoading = false;

        }
    }
}


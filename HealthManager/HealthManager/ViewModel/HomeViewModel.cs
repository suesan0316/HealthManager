using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common.Html;
using HealthManager.Extention;
using HealthManager.Logic.News.Implement;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<string> Items { protected set; get; } = new ObservableCollection<string>();

        public Dictionary<string, string> ItemsDictionary;

        public ICommand ItemTappedCommand { get; set; }

        public ICommand TestCommnad { get; set; }

        public HomeViewModel()
        {

            // SettingDictionary();
            TestCommnad = new Command(async () => await SetTask());

            ItemTappedCommand = new Command<string>((item) =>
            {
                Device.OpenUri(new Uri(ItemsDictionary[item]));
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SetTask()
        {
            var service = new YomiuriNewsService();

            var diction = await service.GetNewsDictionary();

            ItemsDictionary = diction;

            ItemsDictionary.ForEach(data => Items.Add(data.Key));

        }

        async Task SettingDictionary()
        {
            var httpClient = new HttpClient();

            var stream =
                await httpClient.GetStreamAsync("https://yomidr.yomiuri.co.jp/news-kaisetsu/news/kenko-news/");

            var sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                var text = sr.ReadLine();
                if (!text.Contains("articles-thumbList-intro")) continue;
                while (!sr.EndOfStream)
                {
                    var text2 = sr.ReadLine();
                    if (!text2.Contains(HtmlConst.A_TAG_SEARCH_STRING)) continue;
                    while (!sr.EndOfStream)
                    {
                        var text3 = sr.ReadLine();
                        if (!text3.Contains(HtmlConst.H2_TAG_SEARCH_STRING)) continue;
                        ItemsDictionary.Add(HtmlTagUtil.GetH2TagValue(text3),
                            HtmlTagUtil.GetATagHrefValue(text2));
                        break;
                    }
                }
            }
        }
    }
}


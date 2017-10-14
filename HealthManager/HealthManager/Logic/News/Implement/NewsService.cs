using System.Collections.Generic;
using System.Threading.Tasks;
using HealthManager.Logic.News.Service;
using HealthManager.Extention;

namespace HealthManager.Logic.News.Implement
{
    public class NewsService : INewsService
    {
        public async Task<Dictionary<string, string>> GetNewsDictionary()
        {
            var yomiuriNewsService = new YomiuriNewsService();
            var zaikeiNewsService = new ZaikeiNewsService();

            var newsDictionary = new Dictionary<string,string>();

            var yomiuriNewsDictionary = await yomiuriNewsService.GetNewsSourceDictionary();
            var zaikeiNewsDictionary = await  zaikeiNewsService.GetNewsSourceDictionary();

            yomiuriNewsDictionary.ForEach(data=> newsDictionary.Add(data.Key,data.Value));
            zaikeiNewsDictionary.ForEach(data => newsDictionary.Add(data.Key, data.Value));


            return newsDictionary;

        }
    }
}

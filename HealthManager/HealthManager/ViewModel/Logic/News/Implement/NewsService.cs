using System.Collections.Generic;
using System.Threading.Tasks;
using HealthManager.Common.Extention;
using HealthManager.ViewModel.Logic.News.Service;
using HealthManager.ViewModel.Structure;

namespace HealthManager.ViewModel.Logic.News.Implement
{
    public class NewsService : INewsService
    {
        public async Task<List<NewsStructure>> GetNewsData()
        {
            var yomiuriNewsService = new YomiuriNewsService();
            var zaikeiNewsService = new ZaikeiNewsService();

            var newsStructures = new List<NewsStructure>();

            var yomiuriNewsDictionary = await yomiuriNewsService.GetNewsSourceDictionary();
            var zaikeiNewsDictionary = await zaikeiNewsService.GetNewsSourceDictionary();

            yomiuriNewsDictionary.ForEach(data => newsStructures.Add(new NewsStructure(){NewsTitle = data.Key,NewsUrl = data.Value}));
            zaikeiNewsDictionary.ForEach(data => newsStructures.Add(new NewsStructure() { NewsTitle = data.Key, NewsUrl = data.Value }));

            return newsStructures;
        }
    }
}
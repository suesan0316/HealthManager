using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HealthManager.Common.Html;
using HealthManager.Logic.News.Service;

namespace HealthManager.Logic.News.Implement
{
    class YomiuriNewsService : INewsSourceService
    {
        public async Task<Dictionary<string, string>> GetNewsSourceDictionary()
        {
            var httpClient = new HttpClient();

            var stream =
                await httpClient.GetStreamAsync("https://yomidr.yomiuri.co.jp/news-kaisetsu/news/kenko-news/");

            var sr = new StreamReader(stream);

            var itemsDictionary = new Dictionary<string, string>();

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
                        itemsDictionary.Add(HtmlTagUtil.GetH2TagValue(text3),
                            HtmlTagUtil.GetATagHrefValue(text2));
                        break;
                    }
                }
            }
            return itemsDictionary;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using HealthManager.ViewModel.Logic.News.Service;

namespace HealthManager.ViewModel.Logic.News.Implement
{
    class YomiuriNewsService : INewsSourceService
    {

        private const string Url = "https://yomidr.yomiuri.co.jp/news-kaisetsu/news/kenko-news/";

        public async Task<Dictionary<string, string>> GetNewsSourceDictionary()
        {
            var doc = default(IHtmlDocument);
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(new Uri(Url)))
            {
                var parser = new HtmlParser();
                doc = await parser.ParseAsync(stream);
            }

            var query = doc.QuerySelectorAll("div.articles-thumbList-intro > article");
            return query.ToDictionary(value => value.QuerySelector("h2").Text(), value => value.QuerySelector("a").GetAttribute("href"));
        }
    }
}

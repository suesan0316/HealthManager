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
   public class YahooTrainingNewsService : INewsSourceService
    {
        private const string Url = "https://follow.yahoo.co.jp/themes/0f48eec4f4d571e65034";

        public async Task<Dictionary<string, string>> GetNewsSourceDictionary()
        {
            var doc = default(IHtmlDocument);
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(new Uri(Url)))
            {
                var parser = new HtmlParser();
                doc = await parser.ParseAsync(stream);
            }

            var query = doc.QuerySelectorAll("li.detailFeed__item");
            return query.ToDictionary(value => value.QuerySelector("p.detailFeed__ttl").Text(), value => value.QuerySelector("a.detailFeed__wrap").GetAttribute("href"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using HealthManager.Logic.News.Service;

namespace HealthManager.Logic.News.Implement
{
    internal class ZaikeiNewsService : INewsSourceService
    {
        private const string Home = "http://www.zaikei.co.jp";
        private const string Url = Home + "/news/topics/363/";

        public async Task<Dictionary<string, string>> GetNewsSourceDictionary()
        {
            IHtmlDocument doc;
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync(new Uri(Url)))
            {
                var parser = new HtmlParser();
                doc = await parser.ParseAsync(stream);
            }

            var itemsDictionary = new Dictionary<string, string>();
            var article02Query = doc.QuerySelectorAll("div.news_list > div.article_02");
            foreach (var value in article02Query)
            {
                itemsDictionary.Add(value.QuerySelector("p.link > a").Text(), Home + value.QuerySelector("p.link > a").GetAttribute("href"));
            }

            var article02NoImgQuery = doc.QuerySelectorAll("div.news_list > div.article_02_no_img");
            foreach (var value in article02NoImgQuery)
            {
                itemsDictionary.Add(value.QuerySelector("a").Text(), Home + value.QuerySelector("a").GetAttribute("href"));
            }
            return itemsDictionary;
        }
    }
}
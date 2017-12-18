using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeTester
{
    [TestClass]
    public class UnitTest1
    {
        private const string URL = "https://yomidr.yomiuri.co.jp/news-kaisetsu/news/kenko-news/";
        private const string URL2 = "http://www.zaikei.co.jp/news/topics/363/";
        private const string URL3 = "http://futamitc.jp/blog-entry-573.html#title4";
        [TestMethod]
        public async Task TestMethod1()
        {
            var value = await GetNewsSourceDictionaryZ();
            Assert.AreEqual(true, true);
        }

        public async Task<Dictionary<string, string>> GetNewsSourceDictionary()
        {
            try
            {
                var itemsDictionary = new Dictionary<string, string>();
                var doc = default(IHtmlDocument);
                using (var client = new HttpClient())
                using (var stream = await client.GetStreamAsync(new Uri(URL)))
                {
                    // AngleSharp.Parser.Html.HtmlParserオブジェクトにHTMLをパースさせる
                    var parser = new HtmlParser();
                    doc = await parser.ParseAsync(stream);
                }

                var query = doc.QuerySelectorAll("div.articles-thumbList-intro > article");
                //var child = query.QuerySelector("a");
                var list = new Dictionary<string, string>();
                foreach (var value in query)
                {
                    list.Add(value.QuerySelector("h2").Text(), value.QuerySelector("a").GetAttribute("href"));
                }

                return itemsDictionary;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }


        public async Task<Dictionary<string, string>> GetNewsSourceDictionaryZ()
        {
            try
            {
                var itemsDictionary = new Dictionary<string, string>();
                IHtmlDocument doc;
                using (var client = new HttpClient())
                using (var stream = await client.GetStreamAsync(new Uri(URL2)))
                {
                    var parser = new HtmlParser();
                    doc = await parser.ParseAsync(stream);
                }
                var article02Query = doc.QuerySelectorAll("div.news_list > div.article_02");
                foreach (var value in article02Query)
                {
                    itemsDictionary.Add(value.QuerySelector("p.link > a").Text(), "http://www.zaikei.co.jp" + value.QuerySelector("p.link > a").GetAttribute("href"));
                }

                var article02NoImgQuery = doc.QuerySelectorAll("div.news_list > div.article_02_no_img");
                foreach (var value in article02NoImgQuery)
                {
                    itemsDictionary.Add(value.QuerySelector("a").Text(), "http://www.zaikei.co.jp" + value.QuerySelector("a").GetAttribute("href"));
                }
                return itemsDictionary;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

    }
}

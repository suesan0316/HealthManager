using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HealthManager.Common.Html;
using HealthManager.Logic.News.Service;

namespace HealthManager.Logic.News.Implement
{
    internal class ZaikeiNewsService : INewsSourceService
    {
        public async Task<Dictionary<string, string>> GetNewsSourceDictionary()
        {
            var httpClient = new HttpClient();

            var stream =
                await httpClient.GetStreamAsync("http://www.zaikei.co.jp/news/topics/363/");

            var sr = new StreamReader(stream);

            var itemsDictionary = new Dictionary<string, string>();

            while (!sr.EndOfStream)
            {
                var text = sr.ReadLine();

                var endFlg = false;

                if (!text.Contains("<div class=\"news_list\">")) continue;
                while (!sr.EndOfStream)
                {
                    var text2 = sr.ReadLine();
                    if (text2.Contains("<p class=\"link\">"))
                    {
                        var r = new Regex(
                            @"<p class=""link""><a href=""(.*?)"">(.*?)</a></p>",
                            RegexOptions.IgnoreCase);

                        var m = r.Match(text2);

                        var keyText = "";

                        if (m.Success) keyText = m.Groups[2].Value;

                        itemsDictionary.Add(HtmlTagUtil.GetATagLabelValue(keyText),
                            "http://www.zaikei.co.jp" + HtmlTagUtil.GetATagHrefValue(text2));
                    }
                    else if (text2.Contains("<div class=\"sub\">"))
                    {
                        endFlg = true;
                        break;
                    }
                }
                if (endFlg) break;
            }
            return itemsDictionary;
        }
    }
}
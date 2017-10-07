

using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HealthManager.Common.Html;

namespace HealthManager.Common
{
    class CommonUtil
    {
       public static async Task SetWebLinkDictionary(Dictionary<string, string> dictionary)
        {

            HttpClient httpClient = new HttpClient();
            // GET
            Stream stream =
                await httpClient.GetStreamAsync("https://yomidr.yomiuri.co.jp/news-kaisetsu/news/kenko-news/");
            // Streamで処理
            StreamReader sr = new StreamReader(stream);
            bool flg = false;
            while (!sr.EndOfStream)
            {
               string text = sr.ReadLine();
                if (text.Contains("articles-thumbList-intro"))
                {
                    while (!sr.EndOfStream)
                    {
                        string text2 = sr.ReadLine();
                        if (text2.Contains("<a href"))
                        {
                            string value = text2;
                            while (!sr.EndOfStream)
                            {
                                string text3 = sr.ReadLine();
                                if (text3.Contains("<h2>"))
                                {
                                    dictionary.Add(HtmlTagUtil.GetH2TagValue(text3),HtmlTagUtil.GetATagHrefValue(text2));
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

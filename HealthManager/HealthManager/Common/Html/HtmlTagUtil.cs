using System.Text.RegularExpressions;

namespace HealthManager.Common.Html
{
    internal class HtmlTagUtil
    {
        public static string GetH2TagValue(string text)
        {
            return GetRegexMatchString(text, @"<h2>(.*)</h2>", 1);
        }

        public static string GetATagHrefValue(string text)
        {
            return GetRegexMatchString(text, @"<a href=""(.*?)"">", 1);
        }

        public static string GetATagLabelValue(string text)
        {
            return GetRegexMatchString(text, @"<a href=""(.*?)"">(.*?)</a>", 2);
        }

        private static string GetRegexMatchString(string text, string regex, int index)
        {
            var r = new Regex(
                regex,
                RegexOptions.IgnoreCase);

            var m = r.Match(text);

            while (m.Success)
                return m.Groups[index].Value;
            return text;
        }
    }
}
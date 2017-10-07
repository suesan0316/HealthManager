namespace HealthManager.Common.Html
{
    class HtmlTagUtil
    {
        public static string GetH2TagValue(string text)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(
                @"<h2>(.*)</h2>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            System.Text.RegularExpressions.Match m = r.Match(text);

            while (m.Success)
            {
                return m.Groups[1].Value;
            }
            return text;
        }

        public static string GetATagHrefValue(string text)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(
                @"<a href=""(.*?)"">",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var m = r.Match(text);

            while (m.Success)
            {
                return m.Groups[1].Value;
            }
            return text;
        }

    }
}

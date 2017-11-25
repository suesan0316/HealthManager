using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HealthManager.Common.Constant;

namespace HealthManager.Common.Language
{
   public static class LanguageUtils
    {
        static  Dictionary<string,string> _languageMap;

        static LanguageUtils()
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            // TODO 国によって読み込むプロパティを変更する
            var stream = assembly.GetManifestResourceStream(typeof(LanguageUtils).Namespace+"Language.properties");
            using (var reader = new System.IO.StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var row = reader.ReadLine();
                    _languageMap.Add(row.Split(CharConst.Equal)[0], string.Join(StringConst.Equal, row.Split(CharConst.Equal).Skip(1).ToArray()));
                }
            }            
        }

        /// <summary>
        /// キーに対応するメッセージを取得
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return _languageMap[key];
        }
    }
}

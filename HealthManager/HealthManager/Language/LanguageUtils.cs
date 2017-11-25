using System;
using System.Collections.Generic;
using System.Diagnostics;
using HealthManager.DependencyInterface;
using Xamarin.Forms;

namespace HealthManager.Common
{
   public static class LanguageUtils
    {
        public static  Dictionary<string,string> LanguageMap;

        public static string Get(string key)
        {
            return LanguageMap[key];
        }
    }
}

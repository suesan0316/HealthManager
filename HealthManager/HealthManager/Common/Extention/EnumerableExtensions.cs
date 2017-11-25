using System;
using System.Collections.Generic;

namespace HealthManager.Common.Extention
{
    /// <summary>
    /// MonoでForEachを使用するための各兆クラス
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// ForEach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }
    }
}

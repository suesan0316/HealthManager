using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
            {
                await func(value);
            }
        }
    }
}

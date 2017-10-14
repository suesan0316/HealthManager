using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthManager.Logic.News.Service
{
    interface INewsSourceService
    {
        Task<Dictionary<string, string>> GetNewsSourceDictionary();
    }
}

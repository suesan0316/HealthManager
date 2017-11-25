using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthManager.ViewModel.Logic.News.Service
{
    interface INewsSourceService
    {
        Task<Dictionary<string, string>> GetNewsSourceDictionary();
    }
}

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HealthManager.Logic.News.Service
{
    interface INewsService
    {
        Task<Dictionary<string, string>> GetNewsDictionary();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthManager.ViewModel.Logic.News.Service
{
    public interface INewsService
    {
        Task<Dictionary<string, string>> GetNewsDictionary();
    }
}

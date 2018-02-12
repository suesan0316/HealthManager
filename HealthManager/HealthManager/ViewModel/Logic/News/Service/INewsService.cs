using System.Collections.Generic;
using System.Threading.Tasks;
using HealthManager.ViewModel.Structure;

namespace HealthManager.ViewModel.Logic.News.Service
{
    public interface INewsService
    {
        Task<List<NewsStructure>> GetNewsData();
    }
}

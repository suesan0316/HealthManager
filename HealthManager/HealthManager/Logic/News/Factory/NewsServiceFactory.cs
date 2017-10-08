using HealthManager.Logic.News.Implement;
using HealthManager.Logic.News.Service;

namespace HealthManager.Logic.News.Factory
{
    public class NewsServiceFactory
    {
        public static INewsService CreateYomiuriNewsService()
        {
            return new YomiuriNewsService();
        }
    }
}
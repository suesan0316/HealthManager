using HealthManager.Logic.News.Implement;

namespace HealthManager.Logic.News.Factory
{
    public class NewsServiceFactory
    {
        public static NewsService CreateNewsService()
        {
            return new NewsService();
        }
    }
}
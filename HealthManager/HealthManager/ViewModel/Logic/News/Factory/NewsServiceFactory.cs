using HealthManager.ViewModel.Logic.News.Implement;

namespace HealthManager.ViewModel.Logic.News.Factory
{
    public class NewsServiceFactory
    {
        public static NewsService CreateNewsService()
        {
            return new NewsService();
        }
    }
}
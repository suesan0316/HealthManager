using System.IO;
using System.Threading.Tasks;

namespace HealthManager.DependencyInterface
{
    public interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync();
    }
}

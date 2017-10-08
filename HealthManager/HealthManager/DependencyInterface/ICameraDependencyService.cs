using System.IO;
using System.Threading.Tasks;

namespace HealthManager.DependencyInterface
{
    public interface ICameraDependencyService
    {
        void BringUpPhotoGallery();
        void BringUpCamera();
    }
}

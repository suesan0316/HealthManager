namespace HealthManager.DependencyInterface
{
    public interface IImageService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
        void DeleteImageFile(string filePath);
    }
}

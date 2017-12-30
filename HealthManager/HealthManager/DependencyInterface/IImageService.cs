namespace HealthManager.DependencyInterface
{
    public interface IImageService
   {
        void ResizeImage(string sourceFile, string targetFile, float maxWidth, float maxHeight);
   }
}

namespace HealthManager.Model.Service.Interface
{
    public interface ITrainingMenuService
    {
        bool RegistTrainingMenu(int trainingId, string menuName, string load);
        bool UpdateTrainingMenu(int id, int trainingId, string menuName, string load);
    }
}
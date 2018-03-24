using System.Collections.Generic;

namespace HealthManager.Model.Service.Interface
{
    public interface ILoadService
    {
        bool RegistLoad(string loadName);
        List<LoadModel> GetLoadDataList();
        LoadModel GetLoad(int id);
        bool UpdateLoad(int id, string loadName);
    }
}
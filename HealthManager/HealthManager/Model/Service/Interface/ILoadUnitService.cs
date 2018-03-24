using System.Collections.Generic;

namespace HealthManager.Model.Service.Interface
{
    public interface ILoadUnitService
    {
        List<LoadUnitModel> GetLoadUnitList(int loadId);
        LoadUnitModel GetLoadUnit(int id);
    }
}
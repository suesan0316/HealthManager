using System.Collections.Generic;

namespace HealthManager.Model.Service.Interface
{
    public interface ILocationService
    {
        bool RegistLocation(string locationName, string cityId);
        List<LocationModel> GetLoactionDataList();
        LocationModel GetLocation(int id);
    }
}
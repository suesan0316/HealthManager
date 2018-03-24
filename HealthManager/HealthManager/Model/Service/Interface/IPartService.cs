using System.Collections.Generic;

namespace HealthManager.Model.Service.Interface
{
    public interface IPartService
    {
        bool RegistPart(string partName);
        List<PartModel> GetPartDataList();
        bool UpdatePart(int id, string partName);
    }
}
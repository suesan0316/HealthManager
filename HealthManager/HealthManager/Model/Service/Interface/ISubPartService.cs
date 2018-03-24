using System.Collections.Generic;

namespace HealthManager.Model.Service.Interface
{
    public interface ISubPartService
    {
        bool RegistSubPart(int parentPartId, string subPartName);
        List<SubPartModel> GetSubPartDataList();
        List<SubPartModel> GetSubPartDataList(int parentPartId);
        bool UpdateSubPart(int id, int parentPartId, string subPartName);
    }
}
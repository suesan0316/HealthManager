using System.Collections.Generic;

namespace HealthManager.Model.Service.Interface
{
    public interface ITrainingMasterService
    {
        bool RegistTrainingMaster(string trainingName, string load, string part);
        bool UpdateTrainingMaster(int id, string trainingName, string load, string part);
        TrainingMasterModel GetTrainingMasterData(int id);
        List<TrainingMasterModel> GetTrainingMasterDataList();
    }
}
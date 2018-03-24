using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager.Model.Service.Interface
{
    public interface  ITrainingScheduleService
    {
        bool RegistTrainingSchedule(string trainingMenu, int week);
        TrainingScheduleModel GetTrainingSchedule(int week);
        bool UpdateTrainingSchedule(int id, string trainingMenu, int week);
    }
}

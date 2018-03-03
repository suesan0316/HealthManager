using System.Collections.Generic;

namespace HealthManager.Model.Structure
{
    public class TrainingScheduleStructure
    {
        public int Week { get; set; }
        public bool Off { get; set; } 
        public List<TrainingListStructure> TrainingContentList { get; set; }
    }
}

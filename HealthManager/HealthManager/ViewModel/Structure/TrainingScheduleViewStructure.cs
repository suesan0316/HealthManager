using System.Collections.Generic;

namespace HealthManager.ViewModel.Structure
{
    public class TrainingScheduleSViewtructure
    {
        public int Week { get; set; }
        public string WeekName { get; set; }
        public bool Off { get; set; } 
        public List<TrainingListViewStructure> TrainingContentList { get; set; }
    }
}

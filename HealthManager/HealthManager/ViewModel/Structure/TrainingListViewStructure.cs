using System.Collections.Generic;

namespace HealthManager.ViewModel.Structure
{
    public class TrainingListViewStructure
    {
        public int TrainingNo { get; set; }
        public int TrainingId { get; set; }
        public string TrainingName { get; set; }
        public int TrainingSetCount { get; set; }
        public List<LoadContentViewStructure> LoadContentList { get; set; }
    }
}

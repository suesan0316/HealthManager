using System.Collections.Generic;

namespace HealthManager.Model.Structure
{
    public class TrainingListStructure
    {
        public int TrainingNo { get; set; }
        public int TrainingId { get; set; }
        public int TrainingSetCount { get; set; }
        public List<LoadContentStructure> LoadContentList { get; set; }
    }
}

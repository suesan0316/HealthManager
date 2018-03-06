using System.Collections.Generic;
using System.Text;
using AngleSharp.Attributes;
using HealthManager.Common.Language;

namespace HealthManager.ViewModel.Structure
{
    public class TrainingListViewStructure
    {
        public int TrainingNo { get; set; }
        public int TrainingId { get; set; }
        public string TrainingName { get; set; }
        public int TrainingSetCount { get; set; }
        public List<LoadContentViewStructure> LoadContentList { get; set; }
        public string DisplayText => this.ToString();

        public override string ToString()
        {
            var sb = new StringBuilder();
                    sb.Append("\t\t" + LanguageUtils.Get(LanguageKeys.TrainingName) + " : " + TrainingName);
                    sb.Append("\n");
                    sb.Append("\t\t" + LanguageUtils.Get(LanguageKeys.SetCount) + " : " + TrainingSetCount);
                    sb.Append("\n");
                    sb.Append("\t\t" + LanguageUtils.Get(LanguageKeys.LoadMethod));
                    sb.Append("\n");
                    foreach (var loadContentViewStructure in LoadContentList)
                    {
                        sb.Append("\t\t\t" + loadContentViewStructure.LoadName);
                        sb.Append(" : ");
                        sb.Append("" + loadContentViewStructure.Nums + " " + loadContentViewStructure.LoadUnitName);
                        sb.Append("\n");
                    }

            return sb.ToString();
        }
    }
}

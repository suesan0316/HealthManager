using System.Collections.Generic;
using System.Text;
using HealthManager.Common.Enum;
using HealthManager.Common.Language;

namespace HealthManager.ViewModel.Structure
{
    public class TrainingScheduleSViewtructure
    {
        public int Week { get; set; }
        public string WeekName { get; set; }
        public bool Off { get; set; } 
        public List<TrainingListViewStructure> TrainingContentList { get; set; }
        public string DisplayText => this.ToString();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append((WeekEnum)Week);
            sb.Append("\n");
            if (Off)
            {
                sb.Append(LanguageUtils.Get(LanguageKeys.Rest));
            }
            else
            {

                if (TrainingContentList == null || TrainingContentList.Count == 0)
                {
                    sb.Append("\t" + "未設定");
                    return sb.ToString();
                }

                foreach (var trainingListViewStructure in TrainingContentList)
                {
                    sb.Append("\t" + LanguageUtils.Get(LanguageKeys.No) + trainingListViewStructure.TrainingNo);
                    sb.Append("\n");
                    sb.Append("\t\t" + LanguageUtils.Get(LanguageKeys.TrainingName) + " : " + trainingListViewStructure.TrainingName);
                    sb.Append("\n");
                    sb.Append("\t\t" + LanguageUtils.Get(LanguageKeys.SetCount) + " : " + trainingListViewStructure.TrainingSetCount);
                    sb.Append("\n");
                    sb.Append("\t\t" + LanguageUtils.Get(LanguageKeys.LoadMethod));
                    sb.Append("\n");
                    foreach (var loadContentViewStructure in trainingListViewStructure.LoadContentList)
                    {
                        sb.Append("\t\t\t" + loadContentViewStructure.LoadName);
                        sb.Append(" : ");
                        sb.Append("" + loadContentViewStructure.Nums + " " + loadContentViewStructure.LoadUnitName);
                        sb.Append("\n");
                    }
                }
            }
            return sb.ToString();
        }
    }
}

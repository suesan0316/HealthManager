using System;
using System.Text;
using HealthManager.Common;
using HealthManager.Common.Language;
using HealthManager.Model.Service.Interface;
using HealthManager.ViewModel.Logic.Analysis.Service;

namespace HealthManager.ViewModel.Logic.Analysis.Implement
{
    internal class BodyWeightAnalysisServiceImpl : IAnalysisService
    {
        private readonly IBasicDataService _basicDataService;

        public BodyWeightAnalysisServiceImpl(IBasicDataService basicDataService)
        {
            _basicDataService = basicDataService;
        }
        public string Analy()
        {
            var model = _basicDataService.GetBasicData();

            var currentWeight = model.BodyWeight;
            var currentHeight = model.Height;
            var bmi = float.Parse(ViewModelCommonUtil.CulculateBmi(currentHeight,currentWeight).ToString("0.0000"));
            var appropriateWeight = float.Parse(CulculateAppropriate(currentHeight, currentWeight).ToString("0.0000"));
            var difference = currentWeight - float.Parse(appropriateWeight.ToString("0.0000"));

            var sb = new StringBuilder();
            sb.AppendLine(LanguageUtils.Get(LanguageKeys.BodyWeightAnalysisFormat1, model.Height.ToString(), appropriateWeight));
            sb.Append(LanguageUtils.Get(LanguageKeys.BodyWeightAnalysisFormat2, difference, bmi));
            sb.Append(LanguageUtils.Get(LanguageKeys.BodyWeightAnalysisFormat3));
            if (difference > 0)
            {
                sb.Append(LanguageUtils.Get(LanguageKeys.BodyWeightAnalysisFormat4));
                sb.Append(LanguageUtils.Get(LanguageKeys.BodyWeightAnalysisFormat8));
            }
            else
            {
                if (bmi < 18.5)
                {
                    sb.Append(LanguageUtils.Get(LanguageKeys.BodyWeightAnalysisFormat7));
                }
                else
                {
                    sb.Append(LanguageUtils.Get(LanguageKeys.BodyWeightAnalysisFormat5));
                }
            }

            return sb.ToString();
        }

        public double CulculateAppropriate(float height,float bodyWeight)
        {
            return Math.Pow((height / 100),2) * 22 ;
        }
    }
}

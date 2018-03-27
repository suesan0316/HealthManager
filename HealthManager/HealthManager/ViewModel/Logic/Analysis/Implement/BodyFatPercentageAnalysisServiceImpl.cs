using HealthManager.Common;
using HealthManager.Model.Service.Interface;
using HealthManager.ViewModel.Logic.Analysis.Service;

namespace HealthManager.ViewModel.Logic.Analysis.Implement
{
    internal class BodyFatPercentageAnalysisServiceImpl : IAnalysisService
    {
        private IBasicDataService _basicDataService;

        public BodyFatPercentageAnalysisServiceImpl(IBasicDataService basicDataService)
        {
            _basicDataService = basicDataService;
        }

        public string Analy()
        {
            return StringUtils.Empty;
        }
    }
}

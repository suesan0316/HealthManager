using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.ViewModel.Structure;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class TrainingReportViewModel
    {

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Class Variable
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Class Variable

        #endregion Class Variable
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Instance Private Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Instance Private Variables

        #endregion Instance Private Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Constractor

        public TrainingReportViewModel(int id)
        {
            InitCommands();
            var trainingResultModel = TrainingResultService.GeTrainingResultDataList().First(data => data.Id == id);
            var trainingScheduleSViewtructure = JsonConvert.DeserializeObject<TrainingScheduleSViewtructure>(trainingResultModel.TrainingContent);
            Items = trainingScheduleSViewtructure.TrainingContentList;
            TrainingStart = trainingResultModel.StartDate.ToString(ViewModelCommonUtil.DateTimeContainFormatString);
            TrainingEnd = trainingResultModel.EndDate.ToString(ViewModelCommonUtil.DateTimeContainFormatString);
            TrainingTimel = (trainingResultModel.EndDate - trainingResultModel.StartDate).ToString("hh':'mm':'ss");
        }

        #endregion Constractor
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Variables

        public string TrainingStart { get; set; }
        public string TrainingEnd { get; set; }
        public string TrainingTimel { get; set; }
        public List<TrainingListViewStructure> Items { protected set; get; }

        #endregion Binding Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding DisplayLabels

        public string DisplayLabelReturn => LanguageUtils.Get(LanguageKeys.Return);
        public string DisplayLabelTrainingStart => LanguageUtils.Get(LanguageKeys.TrainingStartTime);
        public string DisplayLabelTrainingEnd => LanguageUtils.Get(LanguageKeys.TrainingEndTime);
        public string DisplayLabelTrainingTime => LanguageUtils.Get(LanguageKeys.TrainingSpendTime);
        public string DisplayLabelTrainingMenu => LanguageUtils.Get(LanguageKeys.ExecutedTrainingMenu);

        #endregion Binding DisplayLabels
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Commands

        #endregion Binding Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Command Actions

        public ICommand CommandReturn { get; set; }

        #endregion Command Actions
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Init Commands

        private void InitCommands()
        {
            CommandReturn = new Command(ViewModelCommonUtil.TrainingBackPage);
        }

        #endregion Init Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region ViewModel Logic

        #endregion ViewModel Logic
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init MessageSubscribe
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Init MessageSubscribe

        //private void InitMessageSubscribe()
        //{

        //}

        #endregion Init MessageSubscribe
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Default 
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Default

        #endregion Default
    }
}

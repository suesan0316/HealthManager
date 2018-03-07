using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.ViewModel.Structure;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    class TrainingReportViewModel : INotifyPropertyChanged
    {

        private TrainingScheduleSViewtructure trainingScheduleSViewtructure;

        private TrainingResultModel trainingResultModel;

        public TrainingReportViewModel(int id)
        {
            BackPageCommand = new Command(ViewModelCommonUtil.TrainingBackPage);

            trainingResultModel = TrainingResultService.GeTrainingResultDataList().First(data => data.Id == id);

            trainingScheduleSViewtructure =
                JsonConvert.DeserializeObject<TrainingScheduleSViewtructure>(trainingResultModel.TrainingContent);
            Items = trainingScheduleSViewtructure.TrainingContentList;
            TrainingStart = trainingResultModel.StartDate.ToString(ViewModelCommonUtil.DateTimeContainFormatString);
            TrainingEnd = trainingResultModel.EndDate.ToString(ViewModelCommonUtil.DateTimeContainFormatString);
            TimeSpan ts = (trainingResultModel.EndDate - trainingResultModel.StartDate);
            TrainingTimel = (trainingResultModel.EndDate - trainingResultModel.StartDate).ToString("hh':'mm':'ss");
        }

        /// <summary>
        /// 戻るボタンコマンド
        /// </summary>
        public ICommand BackPageCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        /// <summary>
        /// 戻るボタンラベル
        /// </summary>
        public string BackPageLabel => LanguageUtils.Get(LanguageKeys.Return);

        public string TrainingStartLabel => LanguageUtils.Get(LanguageKeys.TrainingStartTime);

        public string TrainingEndLabel => LanguageUtils.Get(LanguageKeys.TrainingEndTime);

        public string TrainingTimeLabel => LanguageUtils.Get(LanguageKeys.TrainingSpendTime);

        public string TrainingMenuLabel => LanguageUtils.Get(LanguageKeys.ExecutedTrainingMenu);

        public string TrainingStart { get; set; }

        public string TrainingEnd { get; set; }

        public string TrainingTimel { get; set; }

        public List<TrainingListViewStructure> Items { protected set; get; } = new List<TrainingListViewStructure>();

    }
}

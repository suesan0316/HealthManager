using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class TrainingReportListViewModel : INotifyPropertyChanged
    {

        public TrainingReportListViewModel()
        {
            BackPageCommand = new Command(ViewModelCommonUtil.TrainingBackPage);
            TrainingResultItemTappedCommand = new Command<TrainingResultModel>(item =>
            {
                ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingReportView(item.Id));
            });
            var items = TrainingResultService.GeTrainingResultDataList();
            items?.ForEach(data => Items.Add(data));
        }

        /// <summary>
        /// トレーニングリストアイテムソース
        /// </summary>
        public ObservableCollection<TrainingResultModel> Items { protected set; get; } = new ObservableCollection<TrainingResultModel>();


        /// <summary>
        /// 戻るボタンコマンド
        /// </summary>
        public ICommand BackPageCommand { get; set; }

        /// <summary>
        /// 戻るボタンラベル
        /// </summary>
        public string BackPageLabel => LanguageUtils.Get(LanguageKeys.Return);

        /// <summary>
        /// トレーニングリストタップコマンド
        /// </summary>
        public ICommand TrainingResultItemTappedCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

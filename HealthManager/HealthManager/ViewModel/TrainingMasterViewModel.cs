using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// トレーニングマスター画面VMクラス
    /// </summary>
    public class TrainingMasterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TrainingMasterViewModel()
        {
            CancleCommand = new Command(ViewModelCommonUtil.BackTrainingHome);

            PartItemSrouce = PartService.GetPartDataList();
            SubPartService.GetSubPartDataList().ForEach(data => SubPartItemSrouce.Add(data));
            Part = PartItemSrouce[0];
            SubPart = SubPartItemSrouce[0];
        }

        /// <summary>
        /// キャンセルボタンコマンド
        /// </summary>
        public ICommand CancleCommand { get; }

        PartModel _part;
        public PartModel Part
        {
            get { return _part; }
            set
            {
                _part = value;
                SubPartItemSrouce = new ObservableCollection<SubPartModel>();
                SubPartService.GetSubPartDataList(value.Id).ForEach(data => SubPartItemSrouce.Add(data));
                SubPart = SubPartItemSrouce[0];
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Part)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubPartItemSrouce)));
            }
        }

        SubPartModel _subPart;
        public SubPartModel SubPart
        {
            get { return _subPart; }
            set
            {
                if (value == null)
                {
                    _subPart = SubPartItemSrouce[0];
                }
                else
                {
                    _subPart = value;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubPart)));
            }
        }

        /// <summary>
        /// 部位リストボックスアイテム
        /// </summary>
        public List<PartModel> PartItemSrouce { get; }

        /// <summary>
        /// 部位リストボックスアイテム
        /// </summary>
        public ObservableCollection<SubPartModel> SubPartItemSrouce { protected set; get; } = new ObservableCollection<SubPartModel>();

        /// <summary>
        /// キャンセルボタンラベル
        /// </summary>
        public string CancelButtonLabel => LanguageUtils.Get(LanguageKeys.Cancel);

    }
}

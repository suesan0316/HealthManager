using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.Properties;
using HealthManager.View;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// 体格画像表示画面VMクラス
    /// </summary>
    internal class BodyImageViewModel : INotifyPropertyChanged
    {
        private static int PageSize = 10;

        private List<BodyImageModel> _bodyImageList = BodyImageService.GetBodyImageList();

        private int _pageCount;

        private int _nowPage = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BodyImageViewModel(IList<XView> imageStack)
        {
            BackHomeCommand = new Command(ViewModelCommonUtil.DataBackPage);
            NexPageCommand = new Command(NextPage);
            PreviousPageCommand = new Command(PreviousPage);
            ImageStack = imageStack;
            _pageCount = _bodyImageList.Count;
           InitImageStackLayout();
        }

        /// <summary>
        /// 戻るボタンコマンド
        /// </summary>
        public ICommand BackHomeCommand { get; set; }

        /// <summary>
        /// 戻るボタンコマンド
        /// </summary>
        public ICommand NexPageCommand { get; set; }

        /// <summary>
        /// 戻るボタンコマンド
        /// </summary>
        public ICommand PreviousPageCommand { get; set; }

        /// <summary>
        /// 体格画像リスト
        /// </summary>
        public ObservableCollection<XView> BodyImageContents { set; get; } =　new ObservableCollection<XView>();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// エラーラベルをスタックするレイアウトのChildren
        /// </summary>
        public IList<XView> ImageStack { get; set; }

        /// <summary>
        /// 戻るボタンラベル
        /// </summary>
        public string BackHomeLabel => LanguageUtils.Get(LanguageKeys.Return);

        /// <summary>
        /// 戻るボタンラベル
        /// </summary>
        public string NextPageButtonLabel => LanguageUtils.Get(LanguageKeys.NextPage,new string[]{"10"});

        /// <summary>
        /// 戻るボタンラベル
        /// </summary>
        public string PreviousPageButtonLabel => LanguageUtils.Get(LanguageKeys.PreviousPage, new string[] { "10" });

        /// <summary>
        /// 件数ラベル
        /// </summary>
        public string PageCountLabel
        {
            get
            {
                var numStart = _nowPage * 10;
                if (numStart == 0) numStart++;
                var numEnd = _nowPage * PageSize + PageSize < _pageCount ? _nowPage * PageSize + PageSize : _pageCount;
                var returnString = LanguageUtils.Get(LanguageKeys.PageDisplayCount, _pageCount.ToString(), numStart.ToString(),numEnd.ToString());
                return returnString;
            }
        }

        public bool PreviousPageButtonEnable => _nowPage != 0;

        public bool NextPageButtonEnable => _nowPage * PageSize + PageSize < _pageCount;

        public void NextPage()
        {
            _nowPage++;
            InitImageStackLayout();
        }

        public void PreviousPage()
        {
            _nowPage--;
            InitImageStackLayout();
        }

        /// <summary>
        /// 画面に表示するレイアウトを生成
        /// </summary>
        private  void InitImageStackLayout()
        {
            ImageStack.Clear();

            var target = (from a in _bodyImageList
                select a).Skip(_nowPage * PageSize).Take(PageSize);

            foreach (var value in target)
            {
                var childStackLayout = new StackLayout();
                var imageAsBytes = Convert.FromBase64String(value.ImageBase64String);
                var bodyImage = new Image
                {
                    Source = ImageSource.FromStream(() => new MemoryStream(ViewModelCommonUtil.GetResizeImageBytes(imageAsBytes, 500, 625)))
                };
                childStackLayout.Children.Add(bodyImage);

                var registedDateLabel = new Label { Text = value.RegistedDate.ToString(ViewModelCommonUtil.DateTimeFormatString), HorizontalOptions = LayoutOptions.Center };
                childStackLayout.Children.Add(registedDateLabel);

                OnPropertyChanged(nameof(PageCountLabel));
                OnPropertyChanged(nameof(PreviousPageButtonEnable));
                OnPropertyChanged(nameof(NextPageButtonEnable));

                ImageStack.Add(childStackLayout);
            }
        }
    }
}
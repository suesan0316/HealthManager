using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using PropertyChanged;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     体格画像表示画面VMクラス
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    internal class BodyImageViewModel
    {
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Class Variable
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Class Variable

        /// <summary>
        ///     一度に表示する画像数
        /// </summary>
        private const int PageSize = 10;

        #endregion Class Variable

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Constractor

        public BodyImageViewModel(IList<XView> imageStack)
        {
            InitCommands();
            _pageCount = _bodyImageList.Count;
            ImageStack = imageStack;
            InitImageStackLayout();
        }

        #endregion Constractor

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Init Commands

        private void InitCommands()
        {
            CommandReturn = new Command(ViewModelCommonUtil.DataBackPage);
            CommandNexPage = new Command(CommandNextPageAction);
            CommandPreviousPage = new Command(CommandPreviousPageAction);
        }

        #endregion Init Commands

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region ViewModel Logic

        private void InitImageStackLayout()
        {
            ImageStack.Clear();
            var target = (from a in _bodyImageList
                select a).Skip(NowPage * PageSize).Take(PageSize);

            foreach (var value in target)
            {
                var childStackLayout = new StackLayout();
                var imageAsBytes = Convert.FromBase64String(value.ImageBase64String);
                var bodyImage = new Image
                {
                    Source = ImageSource.FromStream(() =>
                        new MemoryStream(ViewModelCommonUtil.GetResizeImageBytes(imageAsBytes, 500, 625)))
                };
                childStackLayout.Children.Add(bodyImage);

                var registedDateLabel = new Label
                {
                    Text = value.RegistedDate.ToString(ViewModelCommonUtil.DateTimeFormatString),
                    HorizontalOptions = LayoutOptions.Center
                };
                childStackLayout.Children.Add(registedDateLabel);
                ImageStack.Add(childStackLayout);
            }
        }

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
        // Instance Private Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Instance Private Variables

        private readonly List<BodyImageModel> _bodyImageList = BodyImageService.GetBodyImageList();
        private readonly int _pageCount;

        #endregion Instance Private Variables

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Binding Variables

        public IList<XView> ImageStack { get; set; }
        public int NowPage { get; set; }
        public bool PreviousPageButtonEnable => NowPage != 0;
        public bool NextPageButtonEnable => NowPage * PageSize + PageSize < _pageCount;

        #endregion Binding Variables

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Binding DisplayLabels

        public string DisplayLabelReturn => LanguageUtils.Get(LanguageKeys.Return);
        public string DisplayLabelNextPage => LanguageUtils.Get(LanguageKeys.NextPage, PageSize.ToString());

        public string DisplayLabelPageCount
        {
            get
            {
                var numStart = NowPage * PageSize;
                if (numStart == 0) numStart++;
                var numEnd = NowPage * PageSize + PageSize < _pageCount ? NowPage * PageSize + PageSize : _pageCount;
                var returnString = LanguageUtils.Get(LanguageKeys.PageDisplayCount, _pageCount.ToString(),
                    numStart.ToString(), numEnd.ToString());
                return returnString;
            }
        }

        public string DisplayLabelPreviousPage => LanguageUtils.Get(LanguageKeys.PreviousPage, PageSize.ToString());

        #endregion Binding DisplayLabels

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Binding Commands

        public ICommand CommandReturn { get; set; }
        public ICommand CommandNexPage { get; set; }
        public ICommand CommandPreviousPage { get; set; }

        #endregion Binding Commands

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Command Actions

        public void CommandNextPageAction()
        {
            NowPage++;
            InitImageStackLayout();
        }

        public void CommandPreviousPageAction()
        {
            NowPage--;
            InitImageStackLayout();
        }

        #endregion Command Actions

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Default 
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Default

        #endregion Default
    }
}
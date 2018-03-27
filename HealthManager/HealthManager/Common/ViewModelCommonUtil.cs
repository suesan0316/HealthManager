using System;
using System.Collections.Generic;
using System.IO;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.DependencyInterface;
using HealthManager.Model.Service;
using HealthManager.Model.Structure;
using HealthManager.View;
using HealthManager.ViewModel.Structure;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HealthManager.Common
{
    /// <summary>
    /// ViewModelで使用するユーティリティクラス
    /// </summary>
    internal class ViewModelCommonUtil
    {
        private static  readonly ViewModelCommonUtil _instance = new ViewModelCommonUtil();

        /// <summary>日付記号あり規定フォーマット</summary>
	    public static string DateTimeFormatString = "yyyy/MM/dd";
        /// <summary>日付記号なし規定フォーマット</summary>
        public static string DateTimeFormatWithoutSymbolString = "yyyyMMdd";
        /// <summary>日付記号あり規定フォーマット</summary>
        public static string DateTimeContainFormatString = "yyyy/MM/dd hh:mm:ss";

        /// <summary>
        /// ホーム画面遷移の共通処理
        /// </summary>
        public static void BackHome()
        {
            ((App)Application.Current).ChangeScreen(new MainTabbedView());
        }

        /// <summary>
        /// 日付を記号ありの規定フォーマットに変換
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
	    public static string FormatDateString(DateTime dateTime)
	    {
		    return  dateTime.ToString(DateTimeFormatString);
	    }

        /// <summary>
        /// 日付文字列を記号ありの規定フォーマットに変換
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns></returns>
	    public static string FormatDateString(string dateTimeString)
	    {
		    return DateTime.Parse(dateTimeString).ToString(DateTimeFormatString);
	    }

        /// <summary>
        /// 日付を記号なしの規定フォーマットに変換
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
		public static string FormatDateStringWithoutSymbol(DateTime dateTime)
	    {
		    return dateTime.ToString(DateTimeFormatWithoutSymbolString);
	    }

        /// <summary>
        /// 日付文字列を記号なしの規定フォーマットに変換
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns></returns>
	    public static string FormatDateStringWithoutSymbol(string dateTimeString)
	    {
		    return DateTime.Parse(dateTimeString).ToString(DateTimeFormatWithoutSymbolString);
	    }

        /// <summary>
        /// ストリームをバイト配列に変換
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		public static byte[] ConvertToByteArrayFromStream(Stream input)
	    {
		    var buffer = new byte[16 * 1024];
		    using (var ms = new MemoryStream())
		    {
			    int read;
			    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
			    {
				    ms.Write(buffer, 0, read);
			    }
			    return ms.ToArray();
		    }
	    }

        /// <summary>
        /// トレーニングホーム画面遷移の共通処理
        /// </summary>
        public static void BackTrainingHome()
        {
            ((App)Application.Current).ChangeScreen(new MainTabbedView());
        }

        public static byte[] GetResizeImageBytes(byte[] originBytes, float width, float height)
        {
            return DependencyService.Get<IImageService>().ResizeImage(originBytes,width,height);
        }

        /// <summary>
        /// データ系画面で使用する戻る共通処理
        /// </summary>
        public static void DataBackPage()
        {
            ViewModelConst.DataPageNavigation.PopAsync();
        }

        /// <summary>
        ///トレーニング系画面で使用する戻る共通処理
        /// </summary>
        public static void TrainingBackPage()
        {
            ViewModelConst.TrainingPageNavigation.PopAsync();
        }

        public static void SendMessage(string messageKey)
        {
            MessagingCenter.Send(_instance, messageKey);
        }

        public static double CulculateBmi(float height, float bodyWeight)
        {
            var bmi = bodyWeight / Math.Pow(height / 100f, 2);
            return bmi;
        }

        public static string GetBmiValueString(float height, float bodyWeight)
        {
            var bmi = CulculateBmi(height, bodyWeight);
            return CommonUtil.GetDecimalFormatString(double.IsNaN(bmi) ? 0 : bmi);
        }

        public static TrainingScheduleSViewtructure CreateTrainingScheduleSViewtructure(WeekEnum week)
        {

            var model = TrainingScheduleService.GetTrainingSchedule((int)week);

            if (model == null)
            {
                var empty = new TrainingScheduleSViewtructure
                {
                    Week = (int)week,
                    WeekName = week.ToString(),
                    Off = false
                };
                return empty;
            }

            var trainingScheduleStructure =
                JsonConvert.DeserializeObject<TrainingScheduleStructure>(model.TrainingMenu);

            var trainingScheduleViewStructure = new TrainingScheduleSViewtructure
            {
                Week = (int)week,
                WeekName = week.ToString(),
                Off = trainingScheduleStructure.Off
            };

            var trainingListViewStructureList = new List<TrainingListViewStructure>();

            int count = 1;
            foreach (var training in trainingScheduleStructure.TrainingContentList)
            {
                var trainingListViewStructure = new TrainingListViewStructure
                {
                    TrainingId = training.TrainingId,
                    TrainingNo = count,
                    TrainingName = TrainingMasterService.GetTrainingMasterData(training.TrainingId).TrainingName,
                    TrainingSetCount = training.TrainingSetCount
                };
                var loadContentViewStructureList = new List<LoadContentViewStructure>();

                foreach (var load in training.LoadContentList)
                {
                    var loadContentViewStructure = new LoadContentViewStructure
                    {
                        LoadId = load.LoadId,
                        LoadName = LoadService.GetLoad(load.LoadId).LoadName,
                        Nums = load.Nums.ToString(),
                        LoadUnitId = load.LoadUnitId,
                        LoadUnitName = LoadUnitService.GetLoadUnit(load.LoadUnitId).UnitName
                    };
                    loadContentViewStructureList.Add(loadContentViewStructure);
                }

                trainingListViewStructure.LoadContentList = loadContentViewStructureList;

                trainingListViewStructureList.Add(trainingListViewStructure);
                count++;
            }

            trainingScheduleViewStructure.TrainingContentList = trainingListViewStructureList;

            return trainingScheduleViewStructure;
        }

    }
}

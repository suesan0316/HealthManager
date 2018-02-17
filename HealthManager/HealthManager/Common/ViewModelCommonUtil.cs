using System;
using System.IO;
using HealthManager.Common.Constant;
using HealthManager.DependencyInterface;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.Common
{
    /// <summary>
    /// ViewModelで使用するユーティリティクラス
    /// </summary>
    internal class ViewModelCommonUtil
    {

        /// <summary>日付記号あり規定フォーマット</summary>
	    public static string DateTimeFormatString = "yyyy/MM/dd";
        /// <summary>日付記号なし規定フォーマット</summary>
        public static string DateTimeFormatWithoutSymbolString = "yyyyMMdd";

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

    }
}

using System;
using System.IO;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.Common
{
    class ViewModelCommonUtil
    {

	    public static string DateTimeFormatString = "yyyy/MM/DD";
	    public static string DateTimeFormatWithoutSymbolString = "yyyyMMDD";

        public static void BackHome()
        {
            ((App)Application.Current).ChangeScreen(new MainTabbedView());
        }

	    public static string FormatDateString(DateTime dateTime)
	    {
		    return  dateTime.ToString(DateTimeFormatString);
	    }

	    public static string FormatDateString(string dateTimeString)
	    {
		    return DateTime.Parse(dateTimeString).ToString(DateTimeFormatString);
	    }

		public static string FormatDateStringWithoutSymbol(DateTime dateTime)
	    {
		    return dateTime.ToString(DateTimeFormatWithoutSymbolString);
	    }

	    public static string FormatDateStringWithoutSymbol(string dateTimeString)
	    {
		    return DateTime.Parse(dateTimeString).ToString(DateTimeFormatWithoutSymbolString);
	    }

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

	}
}

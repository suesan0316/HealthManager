using System;
using System.IO;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.Common
{
    class ViewModelCommonUtil
    {
        public static void BackHome()
        {
            ((App)Application.Current).ChangeScreen(new MainTabbedView());
        }

	    public static string FormatDateString(DateTime dateTime)
	    {
		    return  dateTime.ToString("yyyy/MM/dd");
	    }

	    public static string FormatDateStringWithoutSymbol(DateTime dateTime)
	    {
		    return dateTime.ToString("yyyyMMdd");
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

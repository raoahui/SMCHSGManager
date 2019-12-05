using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace SMCHSGManager.Helper
{
	public class CultureHelper
	{
		public static String GetCulture()
		{
			if (System.Web.HttpContext.Current.Request.Cookies["Culture"] != null)
			{
				String Culture = System.Web.HttpContext.Current.Request.Cookies["Culture"].Value;
				return Culture;
			}
			else
			{
				return String.Empty;
			}
		}

		public static void SetCulture(CultureInfo culture)
		{
			SetCulture(culture.ToString());
		}


		public static void SetCulture(String culturestring)
		{
			HttpCookie cookie = new HttpCookie("Culture");
			cookie.Value = culturestring;
			cookie.Expires = DateTime.Now.AddDays(2);
			System.Web.HttpContext.Current.Response.SetCookie(cookie);
		}

		public static CultureInfo GetCultureInfo()
		{
			if (GetCulture() == String.Empty)
			{
				return null;
			}
			else
			{
				return new CultureInfo(GetCulture());
			}
		}


	}
}
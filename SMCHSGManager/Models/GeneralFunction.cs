using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMCHSGManager.Models
{
	public class GeneralFunction
	{
		public static string RemoveHtmlFormat(string aString)
		{
			string des = aString;
			while (!string.IsNullOrEmpty(des))
			{
				des = des.Replace("&nbsp;", " ");
				des = des.Replace("  ", " ");
				int ii = des.IndexOf('<');
				if (ii< 0)
				{
					break;
				}
				int j = des.IndexOf('>', ii + 1 );
				if (j > 0)
				{
					des = des.Remove(ii, (j + 1) - ii);
				}
				else
				{
					des = des.Remove(ii, des.Length - ii);
				}
			}
			return des;
		}



	}
}
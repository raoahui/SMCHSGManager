using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Text;

namespace System.Web.Mvc.Html
{
    public class CheckBoxListhelper
    {
    }

    public static partial class HtmlHelperExtensions
    {
        public static string CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<string> values, object htmlAttributes)
        {
            return CheckBoxList(htmlHelper, name, values, values, htmlAttributes);
        }
        public static string CheckBoxList1(this HtmlHelper htmlHelper, string name, IEnumerable<string> values, object htmlAttributes)
        {
            return CheckBoxList1(htmlHelper, name, values, values, htmlAttributes);
        }

        //For MealBooking
        public static string CheckBoxList1(this HtmlHelper htmlHelper, string name, IEnumerable<string> values, IEnumerable<string> labels, object htmlAttributes)
        {
            // No creamos ningun CheckBox si no hay valores
            if (values == null)
            {
                return "";
            }

            if (labels == null)
            {
                labels = new List<string>();
            }

            RouteValueDictionary attributes = htmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes);
            attributes.Remove("checked");

            StringBuilder sb = new StringBuilder();

            string[] modelValues = new string[] { };

            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                modelValues = ((string[])modelState.Value.RawValue);
            }

            // Por cada valor pasado generamos un CheckBox

           // List<string> Titles = new List<string> ();
            TagBuilder divTag = new TagBuilder("div");
          
            IEnumerator<string> labelEnumerator = labels.GetEnumerator();
            labelEnumerator.MoveNext();
            string cTitle = labelEnumerator.Current.Substring(0, labelEnumerator.Current.IndexOf('/'));
            appendTitle1(sb, cTitle, values.Count());

            foreach (string s in values)
            {
                string tTitle = labelEnumerator.Current.Substring(0, labelEnumerator.Current.IndexOf('/'));
                if (tTitle != cTitle)
                {
                    cTitle = tTitle;
                    sb.Append("</td></tr>");
                    //sb.Append("</tr></Table>");
                    appendTitle1(sb, cTitle, values.Count());
                }
                
                if (labelEnumerator.Current != null)
                {
                    string temp = labelEnumerator.Current;
                    if(temp.Contains("/"))
                    {
                        int spacePos = temp.IndexOf('/');
                        temp = temp.Substring(spacePos + 1);
                    }
                    if (values.First() == s)
                    {
                        sb.Append("<tr tyle=\" border:0\">");
                        sb.Append("<td  align=\"center\" colspan= 4 style=\"border:0; margin-top:8px; font-family:Arial;  \">");
                     }
                     sb.AppendLine(temp);
                }

                bool isChecked = modelValues.Contains(s);
                sb.Append(CrearCheckBox(name, s, isChecked, attributes)); 
                sb.Append("&nbsp; &nbsp");
                //sb.Append("</td>");
 
                labelEnumerator.MoveNext();
            }
            sb.Append("</td></tr>");

            // Creamos el div contenedor
            //TagBuilder divTag = new TagBuilder("div");
            divTag.InnerHtml = sb.ToString();

            // No nos olvidemos de indicar si hay un error en alguno de los checks
            if (modelState != null && modelState.Errors.Count > 0)
            {
                divTag.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            return divTag.ToString(TagRenderMode.Normal);
        }

        private static void appendTitle1(StringBuilder sb, string cTitle, int count)
        {
            //sb.Append("<td colspan= " + count + " style=\"border:0; margin-top:8px; font-family:Arial;  \">");
            sb.Append("<td  align=\"center\" colspan= 4 style=\"border:0; margin-top:8px; font-family:Arial;  \">");
            sb.Append(cTitle);
            sb.Append("</td></tr>");
        }

        // For VolunteerJobBooking
        public static string CheckBoxList2(this HtmlHelper htmlHelper, string name, IEnumerable<string> values, IEnumerable<string> labels, object htmlAttributes)
        {
            // No creamos ningun CheckBox si no hay valores
            if (values == null)
            {
                return "";
            }

            if (labels == null)
            {
                labels = new List<string>();
            }

            RouteValueDictionary attributes = htmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes);
            attributes.Remove("checked");

            StringBuilder sb = new StringBuilder();

            string[] modelValues = new string[] { };

            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                modelValues = ((string[])modelState.Value.RawValue);
            }

            // Por cada valor pasado generamos un CheckBox

            // List<string> Titles = new List<string> ();
            TagBuilder divTag = new TagBuilder("div");

            IEnumerator<string> labelEnumerator = labels.GetEnumerator();
            labelEnumerator.MoveNext();
            string cTitle = labelEnumerator.Current.Substring(0, labelEnumerator.Current.IndexOf('/'));
            appendTitle2(sb, cTitle);

            foreach (string s in values)
            {
                string tTitle = labelEnumerator.Current.Substring(0, labelEnumerator.Current.IndexOf('/'));
                if (tTitle != cTitle)
                {
                    cTitle = tTitle;
                    sb.Append("</Table></td>");
                    appendTitle2(sb, cTitle);
                }

                if (labelEnumerator.Current != null)
                {
                    string temp = labelEnumerator.Current;
                    if (temp.Contains("/"))
                    {
                        int spacePos = temp.IndexOf('/');
                        temp = temp.Substring(spacePos + 1);
                    }
                    sb.Append("<tr border=\"0\"><td tyle=\" border:0\" nowrap= nowrap>");
                    sb.AppendLine(temp);
                    //sb.Append("</td>");
                    //sb.AppendLine(labelEnumerator.Current);
                }

                //sb.Append("<td  border=\"0\">");
                bool isChecked = modelValues.Contains(s);
                sb.Append(CrearCheckBox(name, s, isChecked, attributes));
                sb.Append("</td></tr>");

                labelEnumerator.MoveNext();
            }
            sb.Append("</Table></td>");

            // Creamos el div contenedor
            //TagBuilder divTag = new TagBuilder("div");
            divTag.InnerHtml = sb.ToString();

            // No nos olvidemos de indicar si hay un error en alguno de los checks
            if (modelState != null && modelState.Errors.Count > 0)
            {
                divTag.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            return divTag.ToString(TagRenderMode.Normal);
        }

        private static void appendTitle2(StringBuilder sb, string cTitle)
        {
            //sb.Append("<td><Table vligh=\"top\" ><tr><td  colspan= 2 style=\"margin-top:8px; font-family:Arial; color=Green; font-size:medium; font-weight:600; \">");
            sb.Append("<td style=\" border:0\"><Table vligh=\"top\" tyle=\" border:0\" ><tr tyle=\" border:0\" ><td nowrap= nowrap colspan= 2 style=\"margin-top:8px; border:0\">");
            sb.Append(cTitle);
            sb.Append("</td></tr>");
        }

        
        public static string CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<string> values, IEnumerable<string> labels, object htmlAttributes)
        {
            // No creamos ningun CheckBox si no hay valores
            if (values == null)
            {
                return "";
            }

            if (labels == null)
            {
                labels = new List<string>();
            }

            RouteValueDictionary attributes = htmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes);
            attributes.Remove("checked");

            StringBuilder sb = new StringBuilder();

            string[] modelValues = new string[] { };

            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                modelValues = ((string[])modelState.Value.RawValue);
            }

            // Por cada valor pasado generamos un CheckBox

            // List<string> Titles = new List<string> ();
            TagBuilder divTag = new TagBuilder("div");

            IEnumerator<string> labelEnumerator = labels.GetEnumerator();
            labelEnumerator.MoveNext();
 
            string cTitle = labelEnumerator.Current;
            if (cTitle.Contains("/"))
            {
                cTitle = labelEnumerator.Current.Substring(0, labelEnumerator.Current.IndexOf('/'));
            }
 
            appendTitle(sb, cTitle);

            foreach (string s in values)
            {
                string tTitle = labelEnumerator.Current;
                if (tTitle.Contains("/"))
                {
                    tTitle = labelEnumerator.Current.Substring(0, labelEnumerator.Current.IndexOf('/'));
                }
                if (tTitle != cTitle)
                {
                    cTitle = tTitle;
                    sb.Append("<p></p>");
                    appendTitle(sb, cTitle);

                }
                // Si el array contiene el valor correspondiente a este checkbox, entonces fue chequeado
                bool isChecked = modelValues.Contains(s);
                sb.Append(CrearCheckBox(name, s, isChecked, attributes));

                if (labelEnumerator.Current != null)
                {
                    string temp = labelEnumerator.Current;
                    if (temp.Contains("/"))
                    {
                        int spacePos = temp.IndexOf('/');
                        temp = temp.Substring(spacePos + 1);
                    }
                    sb.Append("<font face=Arial color=Purple size=3 Weight=600>");
                    sb.AppendLine(temp);
                    sb.Append("</font>");
                    //sb.AppendLine(labelEnumerator.Current);
                }
                labelEnumerator.MoveNext();
            }
            sb.Append("<p></p>");

            // Creamos el div contenedor
            //TagBuilder divTag = new TagBuilder("div");
            divTag.InnerHtml = sb.ToString();

            // No nos olvidemos de indicar si hay un error en alguno de los checks
            if (modelState != null && modelState.Errors.Count > 0)
            {
                divTag.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            return divTag.ToString(TagRenderMode.Normal);
        }

        private static void appendTitle(StringBuilder sb, string cTitle)
        {
            sb.Append("<div style=\"margin-top:8px; font-family:Arial; color=Green; font-size:medium; font-weight:600; \">");
            sb.Append(cTitle);
            sb.Append("</div>");
        }
 
         

        private static string CrearCheckBox(string name, string value, bool isChecked, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "checkbox");
            tagBuilder.MergeAttribute("name", name, true);

            tagBuilder.GenerateId(name);

            if (isChecked)
            {
                tagBuilder.MergeAttribute("checked", "checked");
            }

            if (value != null)
            {
                tagBuilder.MergeAttribute("value", value, true);
            }

            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }



		//public static string GetMealTitles(this HtmlHelper htmlHelper, IEnumerable<string> labels)
		//{

		//    IEnumerator<string> labelEnumerator = labels.GetEnumerator();
		//    labelEnumerator.MoveNext();
		//    StringBuilder sb = new StringBuilder();
  
		//   foreach (string s in labels)
		//    {
		//        if (labelEnumerator.Current != null)
		//        {
		//            string temp = labelEnumerator.Current;
		//            if (temp.Contains("/"))
		//            {
		//                int spacePos = temp.IndexOf('/');
		//                temp = temp.Substring(spacePos + 1);
		//            }
  
		//            sb.Append("<th><font size = \"1\">");
		//            sb.AppendLine(temp);
		//            sb.Append("</font></th>");
		//        }
		//        labelEnumerator.MoveNext();
		//    }

		//    return sb.ToString();
		//}

		//private static void appendTitle3(StringBuilder sb, string cTitle, int count)
		//{
		//    sb.Append("<th colspan= " + count + ">");
		//    sb.Append(cTitle);
		//    sb.Append("</th>");
		//}


		//public static string GetMealDateTitles(this HtmlHelper htmlHelper, IEnumerable<string> labels)
		//{

		//    IEnumerator<string> labelEnumerator = labels.GetEnumerator();
		//    labelEnumerator.MoveNext();
		//    StringBuilder sb = new StringBuilder();

		//    int i = 0; 
		//    int count = 0;
		//    string cTitle = null;
		//    for(i=0; i<labels.Count(); i++)
		//    {
		//        string tTitle = labelEnumerator.Current.Substring(0, labelEnumerator.Current.IndexOf('/'));
		//        if (tTitle != cTitle)
		//        {
		//             if (cTitle != null)
		//             {
		//                 count = i - count;
		//                 appendTitle3(sb, cTitle, count);
		//             }
		//             cTitle = tTitle;
		//        }
		//        labelEnumerator.MoveNext();
		//    }
		//    appendTitle3(sb, cTitle, i-count);
 
		//   return sb.ToString();
		//}

        // For Roles checkboxlist only
        public static string CheckBoxList4(this HtmlHelper htmlHelper, string name, IEnumerable<string> values, IEnumerable<string> labels, object htmlAttributes)
        {
            // No creamos ningun CheckBox si no hay valores
            if (values == null)
            {
                return "";
            }

            if (labels == null)
            {
                labels = new List<string>();
            }

            RouteValueDictionary attributes = htmlAttributes == null ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes);
            attributes.Remove("checked");

            StringBuilder sb = new StringBuilder();

            string[] modelValues = new string[] { };

            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                modelValues = ((string[])modelState.Value.RawValue);
            }

            // Por cada valor pasado generamos un CheckBox

            // List<string> Titles = new List<string> ();
            TagBuilder divTag = new TagBuilder("div");

            IEnumerator<string> labelEnumerator = labels.GetEnumerator();
 
            foreach (string s in values)
            {
                //sb.Append("<p></p>");
                labelEnumerator.MoveNext();

                // Si el array contiene el valor correspondiente a este checkbox, entonces fue chequeado
                bool isChecked = modelValues.Contains(s);
                //if (labelEnumerator.Current == "Member")
                //    isChecked = true;
                sb.Append(CrearCheckBox(name, s, isChecked, attributes));

                if (labelEnumerator.Current != null)
                {
                    sb.AppendLine(labelEnumerator.Current);
                    sb.AppendLine("<br />");
                }
            }
            //sb.Append("<p></p>");

            // Creamos el div contenedor
            //TagBuilder divTag = new TagBuilder("div");
            divTag.InnerHtml = sb.ToString();

            // No nos olvidemos de indicar si hay un error en alguno de los checks
            if (modelState != null && modelState.Errors.Count > 0)
            {
                divTag.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            return divTag.ToString(TagRenderMode.Normal);
        }



    }

}
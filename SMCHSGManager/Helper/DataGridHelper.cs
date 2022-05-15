using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using SMCHSGManager.Models;

namespace System.Web.Mvc.Html
{
    public static class DataGridHelper
    {
        public static string DataGrid<T>(this HtmlHelper helper)
        {
            return DataGrid<T>(helper, null, null);
        }

        public static string DataGrid<T>(this HtmlHelper helper, object data)
        {
            return DataGrid<T>(helper, data, null);
        }

        public static string DataGrid<T>(this HtmlHelper helper, object data, string[] columns)
        {
            //Get items
            var items = (IEnumerable<T>)data;
            if (items == null)
                items = (IEnumerable<T>)helper.ViewData.Model;

            //Get column names 
            if (columns == null)
            {
                columns = typeof(T).GetProperties().Select(p => p.Name).ToArray();
                List<string> temp = columns.ToList();
				if (typeof(T).FullName.EndsWith("PublicMemberInfo"))
				{
					temp.Remove("InitiateStatus");
					//temp.Remove("Email");
					temp.Remove("UserName");
					temp.Remove("GenderID");
					temp.Remove("InitiateTypeID");
					temp.Remove("DateOfBirth");
					temp.Remove("CountryOfBirth");
					temp.Remove("PassportNo");
					temp.Remove("Remark");
				}
                else if (typeof(T).FullName.EndsWith("PublicMemberShortInfo"))
                {
                    temp.Remove("DateOfBirth");
                }
                columns = temp.ToArray();
            }

            //Create HtmlTextWriter 
            var writer = new HtmlTextWriter(new StringWriter());

            //Open table tag 
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            //Render Table Header 
            writer.RenderBeginTag(HtmlTextWriterTag.Thead);
            RenderHeader<T>(helper, writer, columns);
            writer.RenderEndTag();

            // Render table body 
            writer.RenderBeginTag(HtmlTextWriterTag.Tbody);

            int _pageSize = (int)helper.ViewDataContainer.ViewData["PageSize"];
            int i = ((int)helper.ViewDataContainer.ViewData["CurrentPage"] - 1) * _pageSize;

            foreach (var item in items)
            {
                if (item != null)
                    RenderRow<T>(helper, writer, columns, item, ++i);
            }
            writer.RenderEndTag();

            //Close  table tag 
            writer.RenderEndTag();

            //return the string 
            return writer.InnerWriter.ToString();
        }

		 //add first <th></th> for edit and delete link
		private static void RenderHeader<T>(HtmlHelper helper, HtmlTextWriter writer, string[] columns)
		{
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			if (typeof(T).FullName.EndsWith("PublicMemberInfo"))
			{
				int memberColumns = GetMemberColumnCount(columns);
				int i = 0;
				int index = 0;
				foreach (var columnName in columns)
				{
					if (columnName.StartsWith("Member"))
					{
						if (columnName == "MemberNo")
						{
							string temp = "th colspan=\"" + memberColumns.ToString() + " \"";
							writer.RenderBeginTag(temp);
							writer.Write("Member");
							writer.RenderEndTag();
							index = i;
						}
					}
					else
					{
						string temp1 = "th rowspan=\"2\"";
						writer.RenderBeginTag(temp1);
						var currentAction = (string)helper.ViewContext.RouteData.Values["action"];

						if (columnName == "IsActive")
						{
							List<SelectListItem> si = GetYouTubeVideoIDSelectList(helper);
							var link = columnName + helper.DropDownList("IsActive", si, new { onChange = "this.form.submit()" });
							writer.Write(link);
						}
						else
						{
							var head = ReplaceHeader(columnName);
							var link = helper.ActionLink(head, currentAction, new { sort = columnName, searchContent = helper.ViewDataContainer.ViewData["searchContent"] });
							writer.Write(link);
						}
						writer.RenderEndTag();
					}
					i++;
				}
				writer.RenderEndTag();

				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				for (i = index; i < index + memberColumns; i++)
				{
					writer.RenderBeginTag(HtmlTextWriterTag.Th);
					var head = ReplaceHeader(columns[i]).Substring(6);
					//if (i == index)
					//{
					//    List<SelectListItem> si = GetMemberTypeSelectList(helper);
					//    var link = head + helper.DropDownList("memberTypeID", si, new { onChange = "this.form.submit()" });
					//    writer.Write(link);
					//}
					//else
					{
						var currentAction = (string)helper.ViewContext.RouteData.Values["action"];
						var link = helper.ActionLink(head, currentAction, new { sort = columns[i], searchContent = helper.ViewDataContainer.ViewData["searchContent"] });
						writer.Write(link);
					}
					writer.RenderEndTag();
				}

			}
            else if (typeof(T).FullName.EndsWith("wirelessMonitorViewModel"))
            {
                //int memberColumns = GetMemberColumnCount(columns);
                int locSpaCols = 2;
                int speedTestSpanCols = 4;
                int i = 0;
                int index1 = 0;
                int index2 = 0;
                foreach (var columnName in columns)
                {
                    if (columnName == "building" || columnName == "level")
                    {
                        if (columnName == "building")
                        {
                            string temp = "th colspan=\"" + locSpaCols.ToString() + " \"";
                            writer.RenderBeginTag(temp);
                            writer.Write("Location");
                            writer.RenderEndTag();
                            index1 = i;
                        }
                    }
                    else if (columnName == "ping" || columnName == "upload" ||
                             columnName == "download" || columnName == "jitter")
                    {
                        if (columnName == "ping")
                        {
                            string temp = "th colspan=\"" + speedTestSpanCols.ToString() + " \"";
                            writer.RenderBeginTag(temp);
                            writer.Write("Speed Test");
                            writer.RenderEndTag();
                            index2 = i;
                        }
                    }
                    else
                    {
                        string temp1 = "th rowspan=\"2\"";
                        writer.RenderBeginTag(temp1);
                        var currentAction = (string)helper.ViewContext.RouteData.Values["action"];

                        if (columnName == "IsActive")
                        {
                            List<SelectListItem> si = GetYouTubeVideoIDSelectList(helper);
                            var link = columnName + helper.DropDownList("IsActive", si, new { onChange = "this.form.submit()" });
                            writer.Write(link);
                        }
                        else
                        {
                            var head = ReplaceHeader(columnName);
                            var link = helper.ActionLink(head, currentAction, new { sort = columnName, searchContent = helper.ViewDataContainer.ViewData["searchContent"] });
                            writer.Write(link);
                        }
                        writer.RenderEndTag();
                    }
                    i++;
                }
                writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                for (i = index1; i < index1 + locSpaCols ; i++)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Th);
                    var head = columns[i];// ReplaceHeader(columns[i]).Substring(6);
                    var currentAction = (string)helper.ViewContext.RouteData.Values["action"];
                    var link = helper.ActionLink(head, currentAction, new { sort = columns[i], searchContent = helper.ViewDataContainer.ViewData["searchContent"] });
                    writer.Write(link);
                    writer.RenderEndTag();
                }
                for (i = index2; i < index2 + speedTestSpanCols; i++)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Th);
                    var head = columns[i];// ReplaceHeader(columns[i]).Substring(6);
                    var currentAction = (string)helper.ViewContext.RouteData.Values["action"];
                    var link = helper.ActionLink(head, currentAction, new { sort = columns[i], searchContent = helper.ViewDataContainer.ViewData["searchContent"] });
                    writer.Write(link);
                    writer.RenderEndTag();
                }
            }
            else
			{
				foreach (var columnName in columns)
				{
					string temp1 = "th rowspan=\"2\" ";
					writer.RenderBeginTag(temp1);

					if (columnName == "InitiateStatus")
					{
						List<SelectListItem> si = GetStatusSelectList(helper);
						var link = columnName + helper.DropDownList("initiateTypeID", si, new { onChange = "this.form.submit()" });
						writer.Write(link);
					}
					else if (columnName == "IsActive")
					{
						List<SelectListItem> si = GetYouTubeVideoIDSelectList(helper);
						var link = columnName + helper.DropDownList("IsActive", si, new { onChange = "this.form.submit()" });
						writer.Write(link);
					}
					else
					{
						var currentAction = (string)helper.ViewContext.RouteData.Values["action"];
						var head = ReplaceHeader(columnName);
						var link = helper.ActionLink(head, currentAction, new { sort = columnName, searchContent = helper.ViewDataContainer.ViewData["searchContent"] });
						writer.Write(link);
					}
					writer.RenderEndTag();
				}
			}

			writer.RenderEndTag();

		}

        private static int GetMemberColumnCount(string[] columns)
        {
            int memberColumns = 0;
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].StartsWith("Member"))
                {
                    memberColumns++;
                }
            }
            return memberColumns;
        }

		private static List<SelectListItem> GetStatusSelectList(HtmlHelper helper)
		{
			SMCHSGManager.Models.SMCHDBEntities _entities = new SMCHSGManager.Models.SMCHDBEntities();
			int initiateTypeID = (int)helper.ViewDataContainer.ViewData["initiateTypeID"];
			int allID = _entities.InitiateTypes.Count() + 1;

			List<SelectListItem> si = new List<SelectListItem>();
			if (initiateTypeID == allID)
			{
				si.Add(new SelectListItem { Text = "All", Value = allID.ToString(), Selected = true });
			}
			else
			{
				si.Add(new SelectListItem { Text = "All", Value = allID.ToString() });
			}

			int j = 1;

			foreach (InitiateType status in _entities.InitiateTypes.ToList())
			{
				SelectListItem item = new SelectListItem { Text = status.Name, Value = j.ToString() };
				if (initiateTypeID == j)
				{
					item.Selected = true;
				}
				si.Add(item);
				j++;
			}
			return si;
		}

		private static List<SelectListItem> GetYouTubeVideoIDSelectList(HtmlHelper helper)
		{
			int selectID = (int)helper.ViewDataContainer.ViewData["IsActive"];

			var dict = new Dictionary<int, string>();
			dict.Add(1, "Yes");
			dict.Add(2, "No");
			dict.Add(3, "All");

			int allID = 3;

			List<SelectListItem> si = new List<SelectListItem>();
			for (int i = 1; i <= allID; i++)
			{
				SelectListItem item = new SelectListItem { Text = dict[i], Value = i.ToString() };
				if (selectID == i)
				{
					item.Selected = true;
				}
				si.Add(item);
			}
			return si;
		}

        public static string ReplaceHeader(string columnName)
        {
            string newStr = columnName;
			if (columnName == "DateOfInitiation")
			{
				newStr = "Date of Initiation";
			}
			else if (columnName == "DateOfBirth")
			{
				newStr = "Date of Birth";
			}
			else if (columnName == "ID" || columnName == "MemberID" || columnName == "IMemberID")
            {
                newStr = "S/N";
            }
            else if (columnName == "MemberFeeExpiredDate")
            {
                newStr = "MemberFee Expired by";
            }
            else if (columnName == "IDCardNo")
            {
                newStr = "IDCardNo";
            }
			else if (columnName == "Gender")
			{
				newStr = "Gender";
			}
            else if (columnName == "ContactNo")
            {
                newStr = "Contact No";
            }
			else if (columnName == "ICOrPassportNo")
			{
				newStr = "IC/Passport No";
			}
			else if (columnName == "MemberNo")
			{
				newStr = "Member No";
			}
			return newStr;
        }

        //  <td>
        //    <%: Html.ActionLink("Edit", "Edit", new { id=item.ID }) %> |
        //    <%: Html.ActionLink("Delete", "Delete", new { id=item.ID })%>
        //  </td>
        public static void RenderRow<T>(HtmlHelper helper, HtmlTextWriter write, string[] columns, T item, int i)
        {
            write.RenderBeginTag(HtmlTextWriterTag.Tr);
            Guid valueID = new Guid();

			foreach (var columnName in columns)
            {
				if (columnName != "IsOnline")
				{
					write.RenderBeginTag("td nowrap=\"nowrap\"");
				}

                var value = typeof(T).GetProperty(columnName).GetValue(item, null) ?? String.Empty;
                if (columnName == "ID" || columnName == "IMemberID")
				{
					valueID = (Guid)value;
					var link = MvcHtmlString.Create(i.ToString());
					write.Write(link);
					if (typeof(T).FullName.EndsWith("PublicMemberInfo")) //&& (bool)helper.ViewDataContainer.ViewData["IsFromLocalHost"])
					{
						write.Write("&nbsp;");
						link = helper.ActionLink("Edit", "Edit", "OrdinaryMember", new { id = valueID }, new { @style = "color: green;" });
						write.Write(link);
					}
					else if (typeof(T).FullName.EndsWith("MemberFeePaymentListViewModel"))
					{
						write.Write("&nbsp;");
                        var fromDate = typeof(T).GetProperty("FromDate").GetValue(item, null) ?? String.Empty;
                        var toDate = typeof(T).GetProperty("ToDate").GetValue(item, null) ?? String.Empty;
                        link = helper.ActionLink("Delete", "Delete", "MemberFeePayment", new { IMemberID = valueID, FromDate = fromDate, ToDate = toDate }, new { @style = "color: green;" });
						write.Write(link);
                    }
				}
                else if (columnName == "Name" && helper.ViewDataContainer.ViewData["EventID"] != null)
                {
                    var link = helper.ActionLink(value.ToString(), "Signature", new { ID = valueID, eventID = (int)helper.ViewDataContainer.ViewData["EventID"] });
                    if (checkEventSigned(helper, valueID))
                    {
                        link = MvcHtmlString.Create(value.ToString());
                    }
                    write.Write(link);
                }
                else if (columnName == "Name" && !typeof(T).FullName.EndsWith("PublicMemberShortInfo") || columnName == "UserName")
				{
					var link = helper.ActionLink(value.ToString(), "Details", new { id = valueID });
					if (typeof(T).FullName.EndsWith("MemberOnlineInfo"))
					{
						link = helper.ActionLink(value.ToString(), "Details", "UserAdministration", new { Area = "UserAdministration", id = valueID }, new { title = "Details" });
					}
                    else if (typeof(T).FullName.EndsWith("MemberFeePaymentListViewModel"))
                    {
                        var fromDate = typeof(T).GetProperty("FromDate").GetValue(item, null) ?? String.Empty;
                        var toDate = typeof(T).GetProperty("ToDate").GetValue(item, null) ?? String.Empty;
                        link = helper.ActionLink(value.ToString(), "Edit", "MemberFeePayment", new { IMemberID = valueID, FromDate = fromDate, ToDate = toDate }, new { title = "Edit" });
                    }
					write.Write(link);
				}
                else if (columnName == "IsOnline")
                {
                    if ((bool)value)
                    {
                        write.RenderBeginTag("td style=\"color:Green; font-weight:600; \"");
                        write.Write(helper.Encode("Online"));
                    }
                    else
                    {
                         write.RenderBeginTag("td style=\"color:#999\"");
                         var link = "Offline";
                         write.Write(link);
                    }
                }
                else if (columnName == "MemberFeeExpiredDate" || columnName == "DateOfBirth" || columnName == "DateOfInitiation" || columnName == "FromDate" || columnName == "ToDate" || columnName == "ReceivedDate")
				{
					//if (columnName == "MemberFeeExpiredDate" && !string.IsNullOrEmpty((String)value) && (DateTime)value == new DateTime(2020, 12, 31))
                    if (columnName == "MemberFeeExpiredDate" && (value != String.Empty) && (DateTime)value == MemberFeePayment.ToDateGiro)
					{
						write.Write("Giro");
                    }
					else
					{
						write.Write(helper.Encode(String.Format("{0:d MMM yyyy}", value)));
					}
				}
                else if (typeof(T).GetProperty(columnName).PropertyType == typeof(System.Boolean))
                {
					if ((bool)value)
					{
						write.Write("Yes");
					}
					else
					{
						write.Write("No");
					}
	            }
                else
                {
                    write.Write(helper.Encode(value.ToString()));
                }
                write.RenderEndTag();
             }
             write.RenderEndTag();
        }

        private static bool checkEventSigned(HtmlHelper helper, Guid valueID)
        {
            if (helper.ViewDataContainer.ViewData["eventSigedMemberIDs"] != null && ((System.Collections.Generic.List<System.Guid>)helper.ViewDataContainer.ViewData["eventSigedMemberIDs"]).Contains(valueID))
            {
                return true;
            }
            return false;
        }

    }
}



 

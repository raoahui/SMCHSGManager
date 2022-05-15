using System;
using System.Web.Mvc;
using System.Data.Linq;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Collections.Generic;
using SMCHSGManager.ViewModel;
using SMCHSGManager.Models;

namespace SMCHSGManager.Controllers
{
    public class ExcelResult : ActionResult
    {
        private DataContext _dataContext;
        private string _fileName;
        private IQueryable _rows;
        private string[][] _headers = null;

        private TableStyle _tableStyle;
        private TableItemStyle _headerStyle;
        private TableItemStyle _itemStyle;

        public string FileName
        {
            get { return _fileName; }
        }

        public IQueryable Rows
        {
            get { return _rows; }
        }


        public ExcelResult(DataContext dataContext, IQueryable rows, string fileName)
            :this(dataContext, rows, fileName, null, null, null, null)
        {
        }

        public ExcelResult(DataContext dataContext, string fileName, IQueryable rows, string[][] headers)
            : this(dataContext, rows, fileName, headers, null, null, null)
        {
        }

        public ExcelResult(DataContext dataContext, IQueryable rows, string fileName, string[][] headers, TableStyle tableStyle, TableItemStyle headerStyle, TableItemStyle itemStyle)
        {
            _dataContext = dataContext;
            _rows = rows;
            _fileName = fileName;
            _headers = headers;
            _tableStyle = tableStyle;
            _headerStyle = headerStyle;
            _itemStyle = itemStyle;

            // provide defaults
            if (_tableStyle == null)
            {
                _tableStyle = new TableStyle();
				_tableStyle.CellSpacing = 10;
                _tableStyle.BorderStyle = BorderStyle.Solid;
				_tableStyle.BorderColor = Color.LightGray;
                _tableStyle.BorderWidth = Unit.Parse("1px");
            }
			if (_headerStyle == null)
			{
				_headerStyle = new TableItemStyle();
				//_headerStyle.BackColor = Color.LightGray;
				//_headerStyle.ForeColor = Color.WhiteSmoke;
				//_headerStyle.Font.Bold = true;
				_headerStyle.BorderColor = Color.LightGray;
				_headerStyle.BorderWidth = Unit.Parse("1px");
				_headerStyle.BorderStyle = BorderStyle.Solid;
				_headerStyle.Wrap = true;
			}
			if (_itemStyle == null)
			{
				_itemStyle = new TableItemStyle();
				_itemStyle.BorderColor = Color.LightGray;
				_itemStyle.BorderWidth = Unit.Parse("1px");
				_itemStyle.BorderStyle = BorderStyle.Solid;
				_itemStyle.HorizontalAlign = HorizontalAlign.Left;
				_itemStyle.Font.Size = 11;
                //_itemStyle.Wrap = true;
			}
        }

        public override void ExecuteResult(ControllerContext context)
        {
            // Create HtmlTextWriter
            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);

            // Build HTML Table from Items
            if (_tableStyle != null)
                _tableStyle.AddAttributesToRender(tw);
            tw.RenderBeginTag(HtmlTextWriterTag.Table);

			for (int j=0; j< _headers.Length; j++) 
			{
				tw.RenderBeginTag(HtmlTextWriterTag.Tr);
				for (int i = 0; i < _headers[j].Length; i++)
				{
					if (_headerStyle != null)
						_headerStyle.AddAttributesToRender(tw);
					
					if (_headers.Length == 3 && j == 0)
					{
						int beginSpace = 3;
						int colSpan = _headers[1].Count();
						if (_fileName == "LocalRetreatRegistrationTable.xls")
						{
							beginSpace = 5;
							colSpan = _headers[1].Count() * _headers[2].Count();
						}
						if (i < beginSpace || i > beginSpace && beginSpace == 5)
						{
							tw.RenderBeginTag("th rowspan = 3");
						}
						else
						{
							tw.RenderBeginTag("th colspan = " + colSpan.ToString());
						}
					}
					else
					{
							tw.RenderBeginTag(HtmlTextWriterTag.Th);
					}
					string Header = ReplaceHeader(_headers[j][i]);
					tw.Write(Header);
					tw.RenderEndTag();
				}
				tw.RenderEndTag();
			}
		
			// Create Data Rows
			tw.RenderBeginTag(HtmlTextWriterTag.Tbody);
			int iRow = 0;
			foreach (Object row in _rows)
			{
				tw.RenderBeginTag(HtmlTextWriterTag.Tr);
				iRow++;
				string[] Header = _headers[0];
				if (_headers.Length > 1 && !_fileName.StartsWith( "LocalRetreatRegistrationTable"))
				{
					List<string> headerList = Header.ToList();
					for (int i = 1; i < _headers[1].Length; i++)
					{
						headerList.Add(i.ToString());
					}
					Header = headerList.ToArray();
				}

				int iColumn = 0;
				foreach (string header in Header)
				{
					if (_itemStyle != null)
					{
						_itemStyle.AddAttributesToRender(tw);
					}
					if ((iRow % 2 == 0) && (_fileName.StartsWith("AttendanceTable") || _fileName.StartsWith("OrdinaryMemberInfo")))
					{
                        tw.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#D8D8D8");
					}
					tw.RenderBeginTag(HtmlTextWriterTag.Td);
                    if (_fileName.StartsWith("LocalRetreatRegistrationTable"))
                    {
                        if (iColumn == 4)
                        {
                            tw.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                        }
                        string item = ((List<string>)row)[iColumn];
                        if (!string.IsNullOrEmpty(item))
                        {
                            string[] volunterJobs = item.Split('\n');
                            for (int vi = 0; vi < volunterJobs.Count(); vi++)
                            {
                                tw.Write(volunterJobs[vi]);
                                if (vi < volunterJobs.Count())
                                {
                                    tw.WriteBreak();
                                }
                            }
                        }
                    }
                    else
                    {
                        WriteItem(tw, row, header);
                    }
                    iColumn++;
					tw.RenderEndTag();
				}
				tw.RenderEndTag();
			}

			tw.RenderEndTag(); // tbody
            tw.RenderEndTag(); // table
            WriteFile(_fileName, "application/ms-excel", sw.ToString());            
        }

        private static void WriteItem(HtmlTextWriter tw, Object row, string header)
        {
            string strValue = string.Empty;
            System.Reflection.PropertyInfo propertyInfo = row.GetType().GetProperty(header);
            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(row, null);
                if (value != null)
                {
                    strValue = value.ToString();
                    strValue = ReplaceSpecialCharacters(strValue);
                }
				if (header == "MemberFeeExpiredDate")
				{
					if (value != null)
					{
                        if ((DateTime)value == MemberFeePayment.ToDateGiro)
						{
							tw.Write("Giro");
						}
						else
						{
							tw.Write(HttpUtility.HtmlEncode(String.Format("{0:MMM-yy}", value)));
						}
					}
				}
				else if (header == "DateOfInitiation" || header.EndsWith("Date"))
                {
					tw.Write(HttpUtility.HtmlEncode(String.Format("{0:dd MMM yyyy}", value)));
                }
                else if (header.EndsWith("DateTime"))
                {
                    tw.Write(HttpUtility.HtmlEncode(String.Format("{0:dd MMM yyyy HH:mm}", value)));
                }
                else if (header == "AccomodationNeeded")
                {
                    if (strValue == "True")
                    {
                        tw.Write("Yes");
                    }
                    else
                    {
                        tw.Write("No");
                    }
                }
                else
                {
                    tw.Write(HttpUtility.HtmlEncode(strValue));
                }
            }
        }

		private string RenderRow(HtmlTextWriter tw, string strValue)
		{
			if (!string.IsNullOrEmpty(strValue))
			{
				strValue = ReplaceSpecialCharacters(strValue);
			}
			if (_itemStyle != null)
				_itemStyle.AddAttributesToRender(tw);
			tw.RenderBeginTag(HtmlTextWriterTag.Td);
			if (!string.IsNullOrEmpty(strValue))
			{
				if (strValue == "true")
				{
					tw.Write("Yes");
				}
				else if (strValue == "false")
				{
					tw.Write("No");
				}
				else
				{
					tw.Write(HttpUtility.HtmlEncode(strValue));
				}
			}
			tw.RenderEndTag();
			return strValue;
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
			else if (columnName == "MemberNo")
			{
				newStr = "No.";
			}
			else if (columnName == "MemberFeeExpiredDate")
			{
				//newStr = "MemberFee Expired Date";
				newStr = "Expiry By";
			}
			else if (columnName == "IDCardNo")
			{
				newStr = "IDCard No";
			}
			else if (columnName == "ContactNo")
			{
				newStr = "Contact No";
			}
			else if (columnName == "MemberNo")
			{
				newStr = "Member No";
			}
			return newStr;
		}

        private static string ReplaceSpecialCharacters(string value)
        {
            value = value.Replace("’", "'");
            value = value.Replace("“", "\"");
            value = value.Replace("”", "\"");
            value = value.Replace("–", "-");
            value = value.Replace("…", "...");
            return value;
        }

        private static void WriteFile(string fileName, string contentType, string content)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            context.Response.Charset = "";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = contentType;
            context.Response.Write(content);
            context.Response.End();
        }
    }
}

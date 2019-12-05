<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.UploadFile>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

<h5>Files uploaded to server</h5>    


      <%--  <%: Html.Editor("&", "ImageUpload") %>--%>

          <% using (Html.BeginForm("Create", "Image", FormMethod.Post, new { enctype = "multipart/form-data" }))
                { %>
                <%: Html.ValidationSummary(true) %>
                
				 <table style="border:0">
                         <tr>
                            <td class="formlabel">Select a File:
                            </td>
                            <td align="left" class="formvalue">
 									&nbsp; <input type="file" name="fileUpload" size="20" style="width: 262px" />
							</td>
							<td align="left" class="formvalue">
								    &nbsp; <input type="submit" value="Upload" class ="buttonsmall" /> 
							</td>
                        </tr>
  
                     <% if (ViewData["CurrentUploadFile"] != null)  {%>
                       <tr>
                            <td colspan=3 class="formlabel">
    								<%	  SMCHSGManager.Models.UploadFile uploadFile = (SMCHSGManager.Models.UploadFile)ViewData["CurrentUploadFile"];
                                      if( uploadFile.ContentType.StartsWith("image"))
                                      {
										  string srcImage = uploadFile.FilePath + uploadFile.Name;  %>
										<img src="<%: srcImage %>"  height="72" alt="" />
                                    <%}
                                      else
                                      {%>
                                          <%: uploadFile.Name %>
                                      <%}%>
  							 </td>
                        </tr>
                    <% } %>


                       <tr>
                            <td colspan=3 class="formlabel">
                                     <%: Html.ActionLink("Back to List", "Index", null, new { @style = "color:white; ", @class = "buttonsmall" })%> 
                            </td>
                        </tr>
	               </table>
		<% } %>

            
</div>

</asp:Content>

        
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.Location>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

    <div class="fullwidth">    
          
            <div class="listitem">
                <div class = "nextlink">
                   <% if (User.IsInRole("Administrator"))
                  {%> 
                     <%: Html.ActionLink("Add New Location", "Create", null, new { @style = "color:white;", @class = "buttonsmall" })%>
                 <%} %>
                  </div>
            </div>

            <h2> Location List</h2>       
                   
                 <% foreach (var item in Model)
                    { %>
                        <div class="listitem">
                            <div class="dashedlineIndex"></div>
 
                             <div class="thumbnail">
                                   <% if(User.IsInRole("Administrator")) {%> 
                                          <%: Html.ActionLink("Edit", "Edit", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>  
                                          <%: Html.ActionLink("Remove", "Delete", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>
                                   <%} %>
                              </div>
                            
                            <div class="thumbnail">
			<% if (item.LocationUploadFiles != null)
                    {

						List<SMCHSGManager.Models.UploadFile> uploadFiles = item.LocationUploadFiles.Select(a => a.UploadFile).ToList();
						foreach (SMCHSGManager.Models.UploadFile uploadFile in uploadFiles)
						{
									//string srcImage = "/Image/ShowPhoto/" + uploadFile.ID.ToString();
									string srcImage = uploadFile.FilePath + uploadFile.Name; 
                                    if (uploadFile.ContentType.StartsWith("image"))
										{%>
                                     <img src="<%: srcImage %>" height="40" alt="" />
                                    <%}
                                    else
                                    {%>
									<p>
										<%: Html.ActionLink(uploadFile.Name, "FileDownload", "Image", new { imageID =uploadFile.ID }, new { style = "color:Olive " })%>
									</p>
                                         <%-- <% Html.RenderPartial("FileDownload"); %>--%>
                                <%} %>
                            <%} %>
               <%} %>
			                               
                            </div>
                         
                            <p><h3> <%: Html.ActionLink(item.Name, "Details", new { id = item.ID })%> </h3>
                            </p>
                             <p> 
								<% string des = SMCHSGManager.Models.GeneralFunction.RemoveHtmlFormat(item.Description); %>
								<%: Html.Truncate(des, 50) %>
								<%: Html.ActionLink("read more", "Details", new { id = item.ID })%> &raquo
                            </p>
                            <div class="clearlist"> </div>

     
                <%} %>
                <div class="dashedline"></div>
 
                <div class="newscrumbs">
                    Page: <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("Index", new { page = p }))%>
                 </div>
  
            </div>
 
            <div class="clear2column"> </div>
</div>
 </div>
</asp:Content>


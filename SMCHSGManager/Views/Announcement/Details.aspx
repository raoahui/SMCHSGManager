<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.AnnouncementViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Announcement Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
      
      <div class="fullwidth">        
            <div class="listitem">
                <div class = "titlenextlink">
                    <%: Html.ActionLink("Announcement List", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
                 </div>
            </div>

            <div class="listitem">

			  <div class="title">
				   <%: Model.Announcement.Name %> 
			  </div>

			  <% if (!string.IsNullOrEmpty(Model.Announcement.StaticURL))
               {%>
                      <a  href="<%: Model.Announcement.StaticURL %>">  URL </a>
              <%} %>
   
                         <div class="dashedline"> </div>

                        <%: String.Format("{0:ddd, d MMM yyyy HH:mm}", Model.Announcement.AnnounceDate)%>  &nbsp;
						<%--<%: String.Format("{0:f}", Model.Announcement.AnnounceDate)%>  &nbsp;--%>
                        <% if (User.IsInRole("Administrator"))
                           {%>
                        Publish By <%:Model.Announcement.MemberInfo.Name%>  &nbsp;
                        <%} %>
                             <% if (Model.Announcement.IsPublic)
                                           { %> (Public) <%}
                                           else
                                           { %><font color="red">For Initiates Only </font> <%} %>
                               
  
                        <div class="dashedline"></div>

                        <div class="itemdetails">
                              <% if (Model.Announcement.Description != null)
                                {%>
                                    <%--Description :  
										<%: Model.Announcement.Description %>--%>
								<%: MvcHtmlString.Create(Model.Announcement.Description)%>
                               <%} %>
                        </div>

                      <%if (Model.Announcement.AnnouncementUploadFiles != null)
                            {
								List < SMCHSGManager.Models.UploadFile> uploadFiles = Model.Announcement.AnnouncementUploadFiles.Select(a => a.UploadFile).ToList();
								foreach (SMCHSGManager.Models.UploadFile uploadFile in uploadFiles)
                                {
									//string srcImage = "/Image/ShowPhoto/" + uploadFile.ID.ToString();
									string srcImage = uploadFile.FilePath + uploadFile.Name;
							
									%>
									<%--<p>
										<%: Html.ActionLink(uploadFile.Name, "FileDownload", "Image", new { imageID =uploadFile.ID }, new { style = "color:Olive " })%>
									</p>--%>
                                <%--    <% Html.RenderPartial("FileDownload"); %>   --%>
	    							
									<% if (uploadFile.ContentType.StartsWith("image"))
										{%>
											<a href="<%: srcImage %>" class="preview"><img src="<%: srcImage %>" alt="gallery thumbnail" width ="200"/></a>  &nbsp; 
											<%-- <img src="<%: srcImage %>"  alt="" width ="200" title = "<%: uploadFile.Name%>">   &nbsp; &nbsp;--%>
									<%} %>
                            <%} %>
                        <%} %>


                      <div class="actionbuttons" >
                      <% if (User.IsInRole("Administrator"))
                          {%> 
                               <%: Html.ActionLink("Edit", "Edit", new { id = Model.Announcement.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                        <%} %>
                      </div>

                        <div class="dashedline"></div>

                        <div class="prenextlink">
                        <% 
                            int current = Model.AnnouncementIDs.IndexOf(Model.Announcement.ID); %>
                        <div class="nextlink">
                           <%if (current < Model.AnnouncementIDs.Count - 1)
                          { %>
                             <%: Html.ActionLink("Next Announcement", "Details", new { id = Model.AnnouncementIDs[current + 1] })%>
                             <%} %>
                        </div>
                        <%if (current > 0)
                          { %>
                        <%: Html.ActionLink("Previous Announcement", "Details", new { id = Model.AnnouncementIDs[current - 1] })%>
                        <%} %>
                        </div>

			  <div class="clear2column"></div>

		</div>	
	</div>

</div>
</asp:Content>

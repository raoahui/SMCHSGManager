<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.LocationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
		SMCH Association Singapore - 	Location Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div id="body">

      <div class="fullwidth">        
            <div class="listitem">
                <div class = "titlenextlink">
              <% if (User.IsInRole("Administrator"))
                 {%> 
                    <%: Html.ActionLink("Location List", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
             <%} %>
                 </div>
            </div>

         <%--   <h2> Location Details</h2>

             <div class="dashedline"></div>--%>

              <div class="listitem">
						<div class="title">
							<%: Model.Location.Name%> 
						</div>
                     
                       <div class="dashedline">    </div>

                       <div class="itemdetails">
                            <p>
                                <%: Model.Location.Address%>
                            </p>
                           <%  if (Model.Location.Description != null)
                                {%>
									<%: MvcHtmlString.Create(Model.Location.Description)%>
                                   <%-- <p> Description : <%: Model.Location.Description%> </p>--%>
                          <%} %> 
                           <%  if (Model.Location.Directions != null)
                                {%>
                                    <p> Directions : <%: Model.Location.Directions%> </p>
                          <%} %> 
                  
                                <a  href="<%: Model.Location.LinkURL %>"> LinkURL</a>
                      
                        </div>
 
                        <%if (Model.Location.LocationUploadFiles != null)
                            {                  
								List < SMCHSGManager.Models.UploadFile> uploadFiles = Model.Location.LocationUploadFiles.Select(a => a.UploadFile).ToList();
								foreach (SMCHSGManager.Models.UploadFile uploadFile in uploadFiles)
                                {
									//string srcImage = "/Image/ShowPhoto/" + uploadFile.ID.ToString();
									string srcImage = uploadFile.FilePath + uploadFile.Name; %>
                               <p> <img src="<%: srcImage %>"  width = "200" alt="" /></p>
                               <%} %>
                        <%} %>

                
                       <div class="actionbuttons" >
                       <% if (User.IsInRole("Administrator"))
                          {%> 
                                 <%: Html.ActionLink("Edit", "Edit", new { id = Model.Location.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                        <%} %>
                        </div>

                        <div class="dashedline"></div>

                         <div class="prenextlink">
                         <%  if (User.IsInRole("Administrator"))
                             {
                                 int current = Model.LocationIDs.IndexOf(Model.Location.ID); %>
                        <div class="nextlink">
                           <%if (current < Model.LocationIDs.Count - 1)
                             { %>
                             <%: Html.ActionLink("Next Location", "Details", new { id = Model.LocationIDs[current + 1] }, new { @style = "color:white;", @class = "buttonsmall" })%>
                             <%} %>
                        </div>
                        <%if (current > 0)
                          { %>
                        <%: Html.ActionLink("Previous Location", "Details", new { id = Model.LocationIDs[current - 1] }, new { @style = "color:white;", @class = "buttonsmall" })%>
                        <%}
                             }%>
                        </div>

            <div class="clear2column"></div>

        </div>
    </div>
</div>

</asp:Content>

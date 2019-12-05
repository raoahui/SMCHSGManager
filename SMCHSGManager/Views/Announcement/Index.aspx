<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.HomeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
SMCH Association Singapore - 	Announcement List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
  
     <div class="fullwidth">    
          
	 <% Html.EnableClientValidation(); %>
     <% using (Html.BeginForm()) {%>
   
        <%: Html.ValidationSummary(true) %>

        <% if (Model.Announcements.Count() == 0)
           { %>
                 There is no announcement currently .
        <%}
           else
           { %>

            <div class="listitem">
                <div class = "nextlink">
                   <% if (ViewData["AnnounceGroupID"] != null && User.IsInRole("SuperAdmin")) {%> 
                     <%: Html.ActionLink("Add New Announcement", "Create", new { announceGroupID = (int)ViewData["AnnounceGroupID"] }, new { @style = "color:white;", @class = "buttonsmall" })%>
                 <%}else{ %>
					 Name/Description : <%: Html.TextBox("searchContent")%>  
							<input type="submit" value="Search" class ="buttonsmall"  />
				  <%}%>
                  </div>
            </div>

            <h2> 
                <% if (ViewData["AnnounceGroupID"] == null) {%>
                    All 
                 <%}else { %>
						<%: Model.Announcements.FirstOrDefault().AnnounceGroup.Name%> 
                <%} %>
                Announcement List</h2>
  
 
                 <%  int i = 0;
                    foreach (var item in Model.Announcements){ %>
                        <div class="listitem">
                             <div class="dashedlineIndex"></div>

                             <div class="thumbnail">
                                 <% if (User.IsInRole("SuperAdmin"))
									{%> 
                                        <%: Html.ActionLink("Edit", "Edit", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>  
                                        <%: Html.ActionLink("Remove", "Delete", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>
                                 <%} %>
                              </div>

                             <div class="thumbnail">
                             <%--  <%: Html.ActionLink(" ", "Details", new { id = item.ID })%> --%>
                              <% if (!string.IsNullOrEmpty(Model.AnnouncementImages[i]))
                                 { %>
                                        <img src="<%: Model.AnnouncementImages[i] %>" height="50" alt="" />
                                  <%}
                                 i++;%>
                             </div>

                        <%-- ddd, d MMM yyyy HH:mm tt--%>
                         <p>
                             <%: String.Format("{0:ddd, d MMM yyyy HH:mm}", item.AnnounceDate)%>
                          
                              <%  if (!string.IsNullOrEmpty(item.StaticURL))
                                  {%>   
                                    <a style="font-weight:bolder" href="<%: item.StaticURL %>" > <%: item.Name%></a>
                              <%}
                                  else
                                  { %>
                              <%: Html.ActionLink(item.Name, "Details", "Announcement", new { id = item.ID }, new { @style = "font-weight:bolder;" })%> 
                              <%} %>
                         </p>

                           <% string des = SMCHSGManager.Models.GeneralFunction.RemoveHtmlFormat(item.Description); %>
                           <p>  
								<%: Html.Truncate(des, 50)%>
								<%: Html.ActionLink("read more", "Details", new { id = item.ID })%> &raquo
                            </p>
                            <div class="clearlist"> </div>
                        </div>
                <%} %>

                <div class="listitem">                
                    <div class="dashedlineIndex"></div>
    
                           <div class="newscrumbs">
                           <% if (ViewData["AnnounceGroupID"] != null)
                          {%>
                               Page: <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("Index", new { page = p, announceGroupID = (int)ViewData["AnnounceGroupID"], searchContent = ViewData["searchContent"] }))%>
                           <%}
                          else
                          { %>
                              Page: <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("Index", new { page = p, searchContent = ViewData["searchContent"] }))%>
                            <%} %>
                           </div>

					</div>
   
        <%} %>

	 <%} %>

	 </div>
 </div>
 
</asp:Content>


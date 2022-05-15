<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.EventListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Event List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
  
    <div class="fullwidth">    
          
             <div class="listitem">
                <div class = "nextlink">
                   <% if (User.IsInRole("Administrator"))
                  {%> 
                        <%: Html.ActionLink("Add Local Retreat", "Create", new { eventTypeID = 1 }, new { @style = "color:white;", @class = "buttonsmall" })%>
                        <%: Html.ActionLink("Add New Event", "Create", null, new { @style = "color:white;", @class = "buttonsmall" })%>
                 <%} %>
                  </div>
            </div>

       <h2> Upcoming events</h2>       
                 
                <div class="listitem">

         <% if (Model.UpcomingEvents.Count() == 0)
           { %>
                 There is no Upcoming Event currently .
        <%}
           else
           {
                    int[] eventsImageIDs = (int[])ViewData["UpcomingEventsImageIDs"];
                     int i = 0;  
                     foreach (var item in Model.UpcomingEvents)
                    { 
						string eventTitle = item.Title;
						if (item.EventTypeID == 1)
						{
							eventTitle += ' ' + item.EventType.Name; 
						} 
						 %>
                              <div class="dashedlineIndex"></div>
                             
                             <div class="thumbnail">
                              <% if(User.IsInRole("Administrator")) {%> 
                                    <%: Html.ActionLink("Edit", "Edit", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>  
                                    <%: Html.ActionLink("Remove", "Delete", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>
                              <%}%> 
                             </div>
                             
                             <div class="thumbnail">
                              <%  if (!string.IsNullOrEmpty(Model.UpcomingEventsImages[i] ))
                                  { %>
                                        <img src="<%: Model.UpcomingEventsImages[i]  %>" height="60" alt="" />
                                  <%} i++; %>
                            </div>
                         
                            <p>
                                 <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", item.StartDateTime)%>
                                 <%: Html.ActionLink(eventTitle, "Details", "Event", new { id = item.ID }, new { @style = "font-weight:bolder;" })%>    
                             </p>    
                               
                           
                             <%if (item.RegistrationOpenDate != null)
                               { %>
                                        <% Html.RenderPartial("RSVPStatus", item); %>
                              <%}%>

							  <% string des = SMCHSGManager.Models.GeneralFunction.RemoveHtmlFormat(item.Description); %>
                              <p>
                                  <%: Html.Truncate(des, 50) %>
                                  <%: Html.ActionLink("read more", "Details", new { id = item.ID })%> &raquo
                              </p>
                
                <%} %>
                        <div class="clearlist"> </div>
                        <div class="dashedline"></div>
  
                        <div class="newscrumbs">
                            Page: <%=Html.PageLink((int)ViewData["CurrentPageUpcoming"], (int)ViewData["TotalPagesUpcoming"], p => Url.Action("Index", new { pageUpcoming = p, pageRecent = (int)ViewData["CurrentPageRecent"] }))%>
                        </div>
<%} %>
                     </div>
 
              <div class="clear2column">  </div>

 <div class="solidline"></div>
       </br>
       <h2>Recent events</h2>       
               
                 <div class="listitem">
            <% if (Model. RecentEvents.Count() == 0)
           { %>
                There is no Recent Event currently .
        <%}
           else
           {
               foreach (var item in Model.RecentEvents)
                        { %>
                          <div class="dashedlineIndex"></div>

                             <div class="thumbnail">
                              <% if(User.IsInRole("Administrator")) {%> 
                                    <%: Html.ActionLink("Edit", "Edit", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>  
                                    <%: Html.ActionLink("Remove", "Delete", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>
                              <%}%> 
                             </div>

                             <div class="thumbnail">
                             <%--  <%: Html.ActionLink(" ", "Details", new { id = item.ID })%> --%>
                              <%  int i = 0;  
                                  if (!string.IsNullOrEmpty(Model.RecentEventsImages[i]))
                                  {%>
                                <img src="<%: Model.RecentEventsImages[i] %>" height="60" alt="" />
                                  <%} i++; %>
                            </div>
                         
                             <p>
                                 <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", item.StartDateTime)%>
                                 <%: Html.ActionLink(item.Title + ' ' + item.EventType.Name, "Details", "Event", new { id = item.ID }, new { @style = "font-weight:bolder;" })%>    
                             </p>    
                               
							 <% string des = SMCHSGManager.Models.GeneralFunction.RemoveHtmlFormat(item.Description); %>   
                             <p>
                                   <%: Html.Truncate(des, 50) %>
                                   <%: Html.ActionLink("read more", "Details", new { id = item.ID })%> &raquo
                             </p>

                    <%} %>

                             <div class="clearlist"> </div>
                            <div class="dashedline"></div>

                            <div class="newscrumbs">
                                Page: <%=Html.PageLink((int)ViewData["CurrentPageRecent"], (int)ViewData["TotalPagesRecent"], p => Url.Action("Index", new { pageUpcoming = (int)ViewData["CurrentPageUpcoming"], pageRecent = p }))%>
                            </div>
<%} %>
                </div>
  
            <div class="clear2column">  </div>
</div>
</div>

</asp:Content>

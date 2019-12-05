<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.EventSchedule>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%-- <div id="body">
    <h5>Delete Confirmation</h5>
        <div class="centerblock">
        <h6>Are you sure you want to delete the Local Retreat Schedule From<strong> <%: String.Format("{0:g}", Model.DateTimeFrom) %></strong> to <strong> 
        <%: String.Format("{0:g}", Model.DateTimeFrom.AddHours(Model.ScheduleOffset.OffsetHours) )%></strong>?</h6>
             <% using (Html.BeginForm()) { %>
              <div class="editbuttons" align="center">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Index", new { localRetreatID = Model.EventID }, new { @style = "color:white;", @class = "buttonsmall" })%>
             </div>
                <% } %>

       </div>
</div>--%>

  <% Html.RenderPartial("DeleteTemplate", "Local Retreat Schedule (from  " + String.Format("{0:g}", Model.DateTimeFrom) + " to " + String.Format("{0:g}", Model.DateTimeFrom.AddHours(Model.ScheduleOffset.OffsetHours) ); %>

</asp:Content>


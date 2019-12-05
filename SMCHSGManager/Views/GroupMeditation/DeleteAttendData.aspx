<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.GroupMeditationAttendance>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DeleteAttendData
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
    <h5>Delete Attend Data Confirmation</h5>
      <div class="centerblock">
    <h6>Are you sure you want to cancel the GM  Attend Datad <%: Model.GroupMeditation.StartDateTime.ToString()  %> 

    <% using (Html.BeginForm()) { %>
              <div class="editbuttons"  align="center">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Back to List", "Details", new { eventID =Model.GroupMeditationID }, new { @style = "color:white; ", @class = "buttonsmall" })%>
             </div>
    <% } %>

    </div>
</div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.GMVolunteerJobName>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
    <h5>Delete GM Volunteer Job Name Confirmation</h5>
      <div class="centerblock">
    <h6>Are you sure you want to cancel the GM Volunteer Job Named <%: Model.MemberInfo.Name  %> 

    <% using (Html.BeginForm()) { %>
              <div class="editbuttons"  align="center">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Back to List", "Index", new { volunteerJobTypeID = Model.VolunteerJobTypeID }, new { @style = "color:white;", @class = "buttonsmall" })%>
             </div>
    <% } %>

    </div>
</div>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.VolunteerJobType>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%--<h2>Delete Confirmation</h2>
<div class="centerblock">
    <p>Are you sure you want to delete the Member Info Named
    <strong><%: Model.Name %></strong>?
    </p>

    <% using (Html.BeginForm()) { %>
               <div class="editbuttons">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
             </div>
    <% } %>
    </div>--%>

      <% Html.RenderPartial("DeleteTemplate", Model.Name + " VolunteerJobType"); %>

</asp:Content>


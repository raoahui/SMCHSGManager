<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.EventActivity>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <%--<div id="body">
   <h5>Delete Confirmation</h5>
    <div class="centerblock">
    <p>Are you sure you want to delete the Member Info Named
    <strong><%: Model.Name %></strong>?
    </p>

    <% using (Html.BeginForm()) { %>
              <div class="editbuttons" align="center">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
             </div>

    <% } %>

    </div>
</div>--%>
    <%: Html.EditorFor(model=>model.Name, "Delete")%>

</asp:Content>


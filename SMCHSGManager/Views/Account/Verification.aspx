<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Verification
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div id="body">
    <h5>Account Verification </h5>

    <% if ((bool)ViewData["Status"] )
       { %>
    <h6> Your account has been approved. Please <%: Html.ActionLink("Login", "LogOn", "Account")%>  to the site</h6>
    <%}
       else
       { %>
    <h6> User account could not be found... </h6>
    <%} %>
</div>

</asp:Content>

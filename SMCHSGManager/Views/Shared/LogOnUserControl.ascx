<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
       
        <h4>Welcome <b><%: Page.User.Identity.Name %> &nbsp;
        <%: Html.ActionLink("Logout", "LogOff", "Account", new { Area = "" }, new { @style = "color:white;", @class = "buttonsmall" })%> 
<%
    }
    else {
%> 
       <%: Html.ActionLink("Login", "LogOn", "Account", new { Area = "" }, new { @style = "color:white;", @class = "buttonsmall" })%> 
<%
    }
%>
</h4>
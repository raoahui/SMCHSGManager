<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
SMCH Association Singapore - 	 Change Password
</asp:Content>

<asp:Content ID="changePasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="body">
   <h5>Change Password</h5>
    <p align="center">
        Your password has been changed successfully.
    </p>
    
    <p align="center">
    </br>
        <%: Html.ActionLink("Click here", "Index", "Home", null, new { @style = "color:white;", @class = "buttonsmall" })%> to return to Home Page.
    </p>

</div>
</asp:Content>

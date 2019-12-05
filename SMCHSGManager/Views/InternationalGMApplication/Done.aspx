<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Done
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">      
    <h5>You have done your application.</h5>
    
    
    <h6>For more detail about your application, please click <%: Html.ActionLink("Here", "Details", new { id = (int)ViewData["InternationalGMApplicationInfoID"] })%></h6>
</div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.Announcement>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
    <h5>The Announcement Deleted</h5>
        <div class="centerblock">
    <h6>Announcement was successfully deleted.</h6>
 
       <h6>
		    <%: Html.ActionLink("Click here", "Index", new { announceGroupID = (int)ViewData["AnnounceGroupID"] })%>
            to return to the Announcement List.
        </h6>
        </div>
  </div>

 <%--<% Html.RenderPartial("DeletedTemplate", "Announcement"); %>--%>

</asp:Content>


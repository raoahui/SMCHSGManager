<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.AnnouncementViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
SMCH Association Singapore - 	Edit Announcement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="body">

	<%: Html.EditorFor(model => model.Announcement, new { Mode = "Edit" })%>

   <%--   <%: Html.EditorFor(model => model.Announcement, new { Mode = "Edit", UploadFiles = Model.UploadFiles })%>--%>
  
</div>
</asp:Content>


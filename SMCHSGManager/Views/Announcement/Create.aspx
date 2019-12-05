<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.AnnouncementViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
SMCH Association Singapore - 	Create Announcement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="body">

          <%-- <%: Html.EditorFor(model => model.Announcement, new { Mode = "Create", AnnounceGroupID = Model.AnnouncementIDs[0], UploadFiles = Model.UploadFiles })%>--%>

		   <%: Html.EditorFor(model => model.Announcement, new { Mode = "Create", AnnounceGroupID = Model.AnnouncementIDs[0] })%>
  
</div>
 
</asp:Content>


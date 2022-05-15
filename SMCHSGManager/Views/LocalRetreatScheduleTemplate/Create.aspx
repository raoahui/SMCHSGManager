<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.LocalRetreatScheduleTemplateViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div id="body">
             <%: Html.EditorFor(model => model.LocalRetreatScheduleTemplate, "LocalRetreatScheduleTemplate", new { EventActivities = Model.EventActivity, ScheduleOffsets = Model.ScheduleOffset, Mode = "Create" })%>
</div>

</asp:Content>


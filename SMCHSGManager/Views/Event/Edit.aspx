<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.EventViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="body">

       <%: Html.EditorFor(model => model.Event, new { Locations = Model.Locations, EventTypes = Model.EventTypes, Mode = "Edit" })%>
        
 </div>

</asp:Content>


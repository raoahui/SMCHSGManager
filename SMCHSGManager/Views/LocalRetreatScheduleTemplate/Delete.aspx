<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.LocalRetreatScheduleTemplate>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<% Html.RenderPartial("DeleteTemplate", Model.ID + " LocalRetreatScheduleTemplate"); %>

</asp:Content>


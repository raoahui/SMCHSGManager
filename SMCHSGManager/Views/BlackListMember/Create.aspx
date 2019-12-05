<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.BlackListMemberViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">

 	<%: Html.EditorFor(model => model.BlackListMember, new { MemberInfos = Model.MemberInfos, IDCardTypes = Model.IDCardTypes, Mode = "Create" })%>
     
</div>

</asp:Content>

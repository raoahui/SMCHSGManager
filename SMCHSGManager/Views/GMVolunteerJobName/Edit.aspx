<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.GMVolunteerJobNameViewdModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
	<%--	 <%: Html.EditorFor(model => model, "GMVolunteerJobName", new { Mode = "Edit" })%>--%>
       <%: Html.EditorFor(model => model.GMVolunteerJobName, new { MemberInfos = Model.MemberInfo, Mode = "Edit" })%>
        
    </div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.GMVolunteerJobNameViewdModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
   <%--    <%: Html.EditorFor(model => model, "GMVolunteerJobName", new { Mode = "Create" })%>--%>
       <%: Html.EditorFor(model => model.GMVolunteerJobName, new { MemberInfo = Model.MemberInfo, Mode = "Create" })%>
        
    </div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.InternationalGMApplicationInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	ApplicationInternationGMInfo Deleted</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
    <h5>International GM Application Deleted</h5>
        <div class="centerblock">
            <h6>International GM Application was successfully deleted.</h6>
            <h6>
			<% 
				if (ViewData["MemberID"] != null)
				{%>
  					<%: Html.ActionLink("Click here", "Index", "InternationalGMApplication", new { memberID = (Guid)ViewData["MemberID"] }, null)%>
				<%}else{ %>
					<%: Html.ActionLink("Click here", "Index", "InternationalGMApplication")%>
				<%} %>
                to return to the International GM Application List Page.
            </h6>
        </div>
</div>

</asp:Content>

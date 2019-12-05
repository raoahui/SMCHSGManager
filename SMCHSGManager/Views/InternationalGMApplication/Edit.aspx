<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.InternationalGMApplicationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">

<% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

    <%: Html.EditorFor(model => model.InternationalGMApplicationInfo, new { Mode = "Edit" })%>

	<%: Html.EditorFor(model => model.TransportInfos, "InternationalGMApplicationTransportInfo", new { InternationalTransports = ViewData["InternationalTransports"] })%>

  <%--  <% if (Model.InternationalGMApplicationInfo.AshramAndCenterInfo.AccommodationPermit) { %>
			<%: Html.EditorFor(model => model.HsihuAshramEvent, new { Mode = "Edit" })%>
	<%} %>--%>

	<div class="actionbuttons">
                    <input type="submit"  value="Save"  name="Save" class ="buttonsmall" /> &nbsp;
    </div>
 <% } %>

</div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

    <h5>The Black List Member Deleted</h5>

    <div class="centerblock">
        <h6>the record was successfully deleted.</h6>
        <h6>
 	        <%: Html.ActionLink("Click here", "Index", "BlackListMember")%>
            to return to the  Black List Member List.
        </h6>
    </div>

</div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
     <h5>GM Volunteer Job Named <%: (string)ViewData["Name"] %> was successfully deleted.</h5>
        <div class="centerblock">
         <p style="text-align:center">
		    <%: Html.ActionLink("Back to List", "Index", new { volunteerJobTypeID = (int)ViewData["volunteerJobTypeID"]}, new { @style = "color:white;", @class = "buttonsmall" })%>
            to return to the index Page.
        </p>
         </div>
</div>

</asp:Content>

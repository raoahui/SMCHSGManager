<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.EventRegistration>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
		SMCH Association Singapore - 	Event Registration Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
     <h5>Event Registration Deleted</h5>
        <div class="centerblock">
         <p style="text-align:center">Your Event Registration was successfully deleted.</p>
 
        <p style="text-align:center">
		    <%: Html.ActionLink("Click here", "Details", "Event", new { id = (int)ViewData["EventID"] }, null)%>

            to return to the Event Page.
        </p>
        
        </div>
</div>

</asp:Content>

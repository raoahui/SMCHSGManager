<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.EventRegistration>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
		SMCH Association Singapore - 	Event Registered already
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
     <h5>You already registered <%: Model.Event.Title %>  <%: Model.Event.EventType.Name %></h5>
        <div class="centerblock">
 
        <p style="text-align:center">
		    please <%: Html.ActionLink("Click here", "Details", "EventRegistration", new { id = (int)Model.ID }, null)%>

            to see the details.
        </p>
        
        </div>
</div>

</asp:Content>

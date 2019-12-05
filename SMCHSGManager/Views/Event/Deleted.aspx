<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.Event>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%--<div id="body">
    <h5>The Event Deleted</h5>
        <div class="centerblock">
    <h6>Event was successfully deleted.</h6>
 
       <h6>
		    <%: Html.ActionLink("Click here", "Index")%>
            to return to the Event List.
        </h6>
        </div>
  </div>--%>

 <% Html.RenderPartial("DeletedTemplate", "Event"); %>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.EventActivity>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <%--<div id="body">
   <h5>Event Activity Deleted</h5>
     <div class="centerblock">
    <p>Event Activity was successfully deleted.</p>

     <p>
        <%: Html.ActionLink("Click here", "Index") %>
        to return to the Event Activity list.
     </p>
     </div>
 </div>--%>

  <% Html.RenderPartial("DeletedTemplate", "Event Activity"); %>

</asp:Content>


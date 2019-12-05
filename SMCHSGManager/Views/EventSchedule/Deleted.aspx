<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.EventSchedule>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="body">
    <h5>The Local Retreat Schedule item Deleted</h5>
        <div class="centerblock">
    <h6>Schedule was successfully deleted.</h6>
 
       <h6>
		    <%: Html.ActionLink("Click here", "Index", new { localRetreatID = ViewData["LocalRetreatID"] })%>
            to return to the Local Retreat Schedule List.
        </h6>
        </div>
  </div>
</asp:Content>


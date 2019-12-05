<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.Location>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%--<div id="body">
    <h5>The Location Deleted</h5>
        <div class="centerblock">
    <h6>Location was successfully deleted.</h6>
 
       <h6>
		    <%: Html.ActionLink("Click here", "Index")%>
            to return to the Location List.
        </h6>
        </div>
  </div>--%>

   <% Html.RenderPartial("DeletedTemplate", "Location"); %>

</asp:Content>

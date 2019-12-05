<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.VolunteerJobType>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--<h5>VolunteerJobType Deleted</h5>
     <div class="centerblock">
    <p>VolunteerJobType was successfully deleted.</p>

     <p>
        <%: Html.ActionLink("Click here", "Index") %>
        to return to the VolunteerJobType list.
     </p>
 </div>--%>

  <% Html.RenderPartial("DeletedTemplate", "Volunteer Job Type"); %>

</asp:Content>


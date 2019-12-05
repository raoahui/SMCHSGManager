<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.MemberInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AddNewAttend</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

	<div id="body">
    <br />
    <h2>Add New Attendence</h2>
    <br />
    <br />
    Please Select Name  &nbsp;&nbsp; 
    <%: Html.DropDownList("MemberID", new SelectList(ViewData["MemberInfo"] as IEnumerable, "MemberID", "Name", Model.Name))%> 

    <div class="actionbuttons">
        <input type="submit" value="Save" class ="buttonsmall"/>&nbsp; &nbsp;&nbsp; &nbsp      
       <%: Html.ActionLink("Back to List", "Details", new { eventID = ViewData["aEvent"], descending = true }, new { @style = "color:white; ", @class = "buttonsmall" })%>
    </div>

</div>

 <% } %>

</asp:Content>

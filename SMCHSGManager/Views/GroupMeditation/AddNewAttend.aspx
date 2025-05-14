<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AddNewAttend</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

    <div class="fullwidth">         
        <table style="border:0" >
            <tr><td colspan="4" class="formlabel"><h1>Add New Attendence</h1></td></tr>
            <tr><td colspan=2 class="formlabel"><div class="dashedline"></div></td></tr>
            <tr>
                <td class="formlabel"><font color="red" size="2">*</font>Please Select Name </td>
                <td class="formvalue"><%: Html.DropDownList("MemberID", (List<SelectListItem>)ViewData["MemberInfo"])%></td>
            </tr>
            <tr><td colspan=2 class="formlabel"><div class="dashedline"></div></td></tr>
            <tr>
                <td align="Center" colspan=2 class="formlabel">
                    <input type="submit"  value="Save"  class ="buttonsmall" /> &nbsp;
                    <% if (ViewData["checkTime"] != null) { %>
                        <%: Html.ActionLink("Click here", "Index", "Home")%> to return to Home Page.
                    <%} else { %>
                        <%: Html.ActionLink("Back to List", "Details", new { eventID = ViewData["aEvent"], descending = true }, new { @style = "color:white; ", @class = "buttonsmall" })%>
                    <%} %>
                </td>
             </tr>
             <tr><td colspan=2 class="formlabel"><div class="dashedline"></div></td></tr>
        </table>
    </div> 

 <% } %>

</asp:Content>

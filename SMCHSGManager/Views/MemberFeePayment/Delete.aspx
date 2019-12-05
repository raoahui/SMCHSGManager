<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.MemberFeePayment>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="Div1">

    <h5>Delete Confirmation</h5>

        <div align="center" style=" margin-bottom:10px; margin-top:15px">
            <h6>Are you sure you want to delete the Member Fee Payment with Named
            <strong><%: (string)ViewData["Name"] %> from <%: string.Format("{0:d}", Model.FromDate)%> to <%: string.Format("{0:d}", Model.ToDate) %></strong>? </h6>
            </br>

             <% using (Html.BeginForm()) { %>
                <div class="editbuttons">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
                </div>
             <% } %>
        </div>

  </div>
</asp:Content>


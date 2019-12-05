<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.MemberOrder>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Order Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">

   <h5>Delete Confirmation</h5>

        <div align="center">
            <h4>Are you sure you want to delete your product order (time :   
             <strong><%: String.Format("{0:g}", Model.LatestOrderDateTime) %> , price :   <%: String.Format(Model.CurrencyCode + "{0:n}", Model.Price)%></strong> ?
            </h4>
            </br>

             <% using (Html.BeginForm()) { %>
                <div class="editbuttons">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Details", new { memberOrderID = Model.ID, orderStatusID = 1 }, new { @style = "color:white;", @class = "buttonsmall" })%>
                </div>
             <% } %>
        </div>
        
</div>

</asp:Content>

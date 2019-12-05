<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.MemberOrder>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Order Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
    <h5>The MemberOrder Deleted</h5>
        <div class="centerblock">
            <h6>Member Order was successfully deleted.</h6>
            <h6>
  		        <%: Html.ActionLink("Click here", "Index", "ProductOrder", new { recentOrder = false }, null)%>
                to return to the Order Page.
            </h6>
        </div>
</div>

</asp:Content>

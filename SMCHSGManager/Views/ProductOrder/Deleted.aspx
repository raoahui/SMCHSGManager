<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.ProductOrder>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
    <h5>The Order Info Deleted</h5>
        <div class="centerblock">
            <h6>Order Info was successfully deleted.</h6>
 
            <h6>
		        <%: Html.ActionLink("Click here", "Index", "ProductOrder", new { recentOrder = false }, null)%>
                        to return to the Order Info List.
            </h6>
        </div>
</div>

 <%--<% Html.RenderPartial("DeletedTemplate", "Product Order"); %>--%>

</asp:Content>

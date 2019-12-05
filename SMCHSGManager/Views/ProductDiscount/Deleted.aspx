<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

    <h5>The Product Discount Deleted</h5>

    <div class="centerblock">
        <h6>Product Discount was successfully deleted.</h6>
        <h6>
 	        <%: Html.ActionLink("Click here", "Index", "ProductDiscount")%>
            to return to the Product Detail List.
        </h6>
    </div>

</div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.ProductDiscount>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">     

<h5>Product Discount List</h5>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

    <table>
        <tr>
            <th></th>
            <th>
                S/N
            </th>
            <th>
                Product Name
            </th>
            <th>
                Discount
            </th>
            <th>
                DateFrom
            </th>
            <th>
                DateTo
            </th>
        </tr>

    <% int i = 0;
    	 foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.ID }) %> |
                <%: Html.ActionLink("Delete", "Delete", new { id=item.ID })%>
            </td>
            <td>
                <%: (++i).ToString() %>
            </td>
            <td>
                <%: item.Product.Name + ' ' + item.Product.NameChi %>
            </td>
            <td>
                <%: (item.Discount * 100).ToString() + '%' %>
            </td>
            <td>
                <%: String.Format("{0:d}", item.DateFrom) %>
            </td>
            <td>
                <%: String.Format("{0:d}", item.DateTo) %>
            </td>
        </tr>
    
    <% } %>

    </table>


</div>

</asp:Content>


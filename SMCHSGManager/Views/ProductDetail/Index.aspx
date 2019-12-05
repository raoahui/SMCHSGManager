<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.ProductDetail>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

   <h5>Product Detail List of</h5>
   <h6> <%: Model.FirstOrDefault().Product.Name %></h6>
	<h6> <%: Model.FirstOrDefault().Product.NameChi %></h6>

    <table>
        <tr>
            <th></th>
      <%--      <th>
                ID
            </th>
            <th>
                ProductID
            </th>--%>
            <th>
                SizeDescription
            </th>
            <th>
                UnitsInStock
            </th>
            <th>
                UnitsOnOrder
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.ID }) %> 
				<% if (Model.Count() > 1)
				{ %>
					| <%: Html.ActionLink("Delete", "Delete", new { id = item.ID })%>
				<%} %>
            </td>
          <%--  <td>
                <%: item.ID %>
            </td>
            <td>
                <%: item.ProductID %>
            </td>--%>
            <td>
                <%: item.SizeDescription %>
            </td>
            <td>
                <%: item.UnitsInStock %>
            </td>
            <td>
                <%: item.UnitsOnOrder %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <h6>
           <%: Html.ActionLink("Create New", "Create", "ProductDetail", new { productID = Model.FirstOrDefault().ProductID }, new { @style = "color:white;", @class = "buttonsmall" })%> &nbsp; &nbsp;
           <%: Html.ActionLink("Done", "Index", "Product", null, new { @style = "color:white;", @class = "buttonsmall" })%>
    </h6>
  

	</div>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.NamesByProductDetailViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	NamesByProductDetail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

<div style=" padding: 1.6em 1.6em; text-align: center; ">

    <h2><%: 	ViewData["ProductName"] %></h2>
    <h4><%: 	ViewData["ProductItemCode"] %></h4>
    <h4><%: 	ViewData["SizeDes"] %></h4>
</div>

    <table>
        <tr>
            <th></th>
            <th>
                Order Name
            </th>
            <th>
                Quantity
            </th>
        </tr>

    <% int i = 1;
    	foreach (var item in Model) { %>
    
        <tr>
            <td><%: (i++).ToString() %>
              </td>
            <td>
                <%: item.name %>
            </td>
            <td>
                <%: item.qty %>
            </td>
        </tr>
    
    <% } %>

    </table>

</div>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.ProductDetail>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ProductSelect
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ProductSelect</h2>

    <table>
        <tr>
            <th></th>
            <th>
                Select?
            </th>
            <th>
                Name
            </th>
 			<% if (Model.FirstOrDefault().Product.CategoryID == 10){ %>
            <th>
                QuantityPerUnit
            </th>
			<%}else{ %>
			<th>
                Item Code
            </th>
			<%} %>
            <th>
                UnitPrice
            </th>
             <th>
                Discount
            </th>
            <th>
                New Product
            </th>
            <th>
                Description
            </th>
            <th>
                UploadFileID
			</th>
            <th>
                SizeDescription
            </th>
        </tr>

  <% Html.EnableClientValidation(); %>
     <% using (Html.BeginForm())
        {%>
        <%: Html.ValidationSummary(true)%>

    <%   int i = 0;
         List<bool> productSelect = (List<bool>)ViewData["ProductSelect"];
         foreach (var item in Model)
         { %>
    
        <tr>
             <td><%: (i + 1).ToString()%></td>
             <td>
                <%: Html.CheckBox(item.ID.ToString(), productSelect[i])%> 
               <%-- <%: item.ID %>--%>
            </td>
            <td>
                <%: item.Product.Name%>
            </td>
           	<% if (Model.FirstOrDefault().Product.CategoryID == 10)
			   { %>
			<td>
                <%: item.Product.QuantityPerUnit%>
            </td>
 			<%}else{ %>
			<td>
                 <%: item.Product.ItemCode%>
            </td>
			<%} %>
            <td>
                <%: String.Format(item.Product.CurrencyCode + "{0:n}", item.Product.UnitPrice)%>
            </td>
 			<td>
				<%--<% if (item.Product.Discount.HasValue)
	   { %>
					 <%: (item.Product.Discount * 100).ToString() + '%'%>
				<%} %>--%>
            </td>
			<td>
				<% if (item.Product.NewProduct)
	   { %>
					Yes
				<%}else{ %>
					No
				<%} %>
			</td>
            <td>
                <%: item.Product.Description%>
            </td>
            <td>
                <%: item.Product.ProductUploadFiles.First().UploadFileID%>
            </td>
        </tr>
    
    <% i++;
         } %>

                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
 
                       <tr>
                            <td  colspan=7 class="formlabel">
                             <div class="actionbuttons">
                                  <input type="submit"  value="ProductSelect" class ="buttonsmall" /> &nbsp;
                                  <%: Html.ActionLink("Back to List", "Index", null, new { @style = "color:white; ", @class = "buttonsmall" })%>
                               </div>
                       </td></tr>
<%} %>
    </table>

 <%--   <p>
        <%: Html.ActionLink("Confirm", "ProductSelectConfirm") %>
    </p>--%>

</asp:Content>


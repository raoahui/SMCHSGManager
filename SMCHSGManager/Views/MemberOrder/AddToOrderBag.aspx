<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.ProductDetailViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AddToOrderBag
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
  
  <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm())
	   {%>

<div style=" padding: 1.6em 1.6em; text-align: center; font-size: 1.9em; ">Product Detail</div>

<table style="border:0">

	<tr>
	<td style="border:0">
		<table style="border:0">
			<tr>
				<td colspan = 2 style="border:0">
					<h2><%: Model.Product.Name %></h2>
				</td>
			</tr>
			<tr>
				<td colspan = 2 style="border:0">
					<h2><%: Model.Product.NameChi %></h2>
				</td>
			</tr>
			<tr>
				<td colspan = 2 style="border:0">
					<% if (!string.IsNullOrEmpty(Model.Product.ItemCode))
			{%>
				<h4>ItemCode: [<%: Model.Product.ItemCode%>]</h4>
			<%} %>

			<%  int temp;
                if (!string.IsNullOrEmpty(Model.Product.QuantityPerUnit) && !int.TryParse(Model.Product.QuantityPerUnit, out temp))
	  {%>
				<h4>QuantityPerUnit: <%: Model.Product.QuantityPerUnit%></h4>
			<%} %>					
				</td>
			</tr>

			<tr><td colspan = 2 style="border:0">
					<h2>Usual Price: <%: String.Format(Model.Product.CurrencyCode + "{0:n}", Model.Product.UnitPrice)%></h2>
			</td></tr>
			
	
			<% 	SMCHSGManager.Models.ProductDiscount productDiscount = new SMCHSGManager.Models.ProductDiscount();
				if (Model.Product.ProductDiscounts.Any(a => a.DateTo >= DateTime.Today))
			   {
				   productDiscount = Model.Product.ProductDiscounts.Where(a => a.DateTo >= DateTime.Today).FirstOrDefault();
			   }
			   //decimal realUnitPrice = Model.Product.UnitPrice * (decimal)(1 - productDiscount.Discount);

			 if (productDiscount.Discount > 0) { %>
			<tr><td colspan = 2 style=" color:Red; border:0">			
				<% if (productDiscount.Discount == 0.3333f){ %>
					<h2>Buy 2 get 1 free Until <%: string.Format("{0:d}", productDiscount.DateTo)%></h2>
				<%}else{ %>
	   				<h2>Now Price: <%: String.Format(Model.Product.CurrencyCode + "{0:n}", Model.ReadPrice) + " (Until " + string.Format("{0:d}", productDiscount.DateTo) + ")"%> </h2>
				<%} %>
			</td></tr>
			<%} %>
	
			<tr>
				<td colspan = 2 style="border:0">
					<h4><%: Model.Product.Description%></h4>
				</td>
			</tr>
		
		<%  string[] quantities = (string[])ViewData["productQuantities"]; 
		   if(Model.Sizes.Count == 1 && string.IsNullOrEmpty(Model.Sizes[0])){ %>
				<tr>	
					<h4><td colspan = 2 style="border:0">Quantity</td></h4>
				</tr>
				<tr>
					<h4><td  colspan = 2 style="border:0">
							<%= Html.TextBox("Quantity", quantities[0], new { cols = 5, style = "width:50px;" })%>
					</td></h4>
				</tr>
	<%}else{ 
			    int qtyIndex = 0; %>
				<tr>	
					<h4><td style="border:0">Size</td>
					<td style="border:0">Quantity</td></h4>
				</tr>
				<%  foreach (var s in Model.Sizes){ %>
					<tr>
						<h4><td style=" border:0; text-align:left">
							<%: s%>
						</td>
						<td  style="border:0">
							<%= Html.TextBox("Quantity", quantities[qtyIndex++], new { cols = 5, style = "width:50px;" })%>
						</td></h4>
					</tr>
				<%} %>
		<%} %>

		</table>
	</td>
	<td  style="border:0"></td>
	<td style="border:0">	 
			<% 
				string srcImage = Model.Product.ProductUploadFiles.FirstOrDefault().UploadFile.FilePath +
											Model.Product.ProductUploadFiles.FirstOrDefault().UploadFile.Name; %>
			<img src="<%: srcImage %>"  width = "280" alt="" />	
	</td>
	</tr>
	<tr><td style="border:0; text-align:center">
	  </td>
	  <td  style="border:0"></td>
	  <td style="border:0; text-align:center">
	  <input type="submit"  value="Order" name="AddToOrderBag" class ="buttonsmall" /> 
	 </td></tr>
		<tr>
		<td colspan=3 style="border:0; text-align:right"  >By clicking the Order button, you will make a order for this product. </td></tr>
</table>


<%} %>
</div>

</asp:Content>

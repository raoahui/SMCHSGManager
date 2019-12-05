<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.MemberOrderDetail>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
SMCH Association Singapore - 	Order Detail List</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">

<div style=" padding: 1.3em 1.3em; text-align: center; font-size: 1.9em; ">

<%  Guid curGuid = (Guid)Membership.GetUser().ProviderUserKey;
	 if (Model.Count() > 0)
   { %>
    <p><%: Model.FirstOrDefault().MemberOrder.ProductOrder.Title%> Order List for</p>
    <%: Model.FirstOrDefault().MemberOrder.MemberInfo.Name%>
</div>
 <h6>Your order has been confirmed.</h6>
<div style=" padding: 0.5em 0.5em; text-align: center; font-size: 1.3em; color:Green; ">
	You have until <%: string.Format("{0:ddd, MMM d yyyy HH:mm}", Model.FirstOrDefault().MemberOrder.ProductOrder.OrderCloseDate.AddMinutes(-1)) %> to change your order, after which it will be consolidated and submitted
 </div>
 <br />

     <table>
        <tr>
            <th></th>
            <th></th>
           <th>
                Product Name
            </th>
			<% if (Model.FirstOrDefault().ProductDetail.Product.CategoryID == 10)
	  { %>
            <th>
                QuantityPerUnit
            </th>
			<%}
	  else
	  { %>
			<th>
                Item Code
            </th>
			<%} %>
             <th>
                UnitPrice
            </th>
			<th>Discount</th>
             <th>
                Size
            </th>
			<th>
                Qty
            </th>
            <th>
                Sub Price
            </th>
			<th>OrderStatus
			</th>

            <th>
                
            </th>

        </tr>

    <%
	   int i = 0;
	   List<decimal> realPrices = (List<decimal>)ViewData["RealPrices"];
	   foreach (var item in Model)
	   {
		   SMCHSGManager.Models.ProductDiscount productDiscount = new SMCHSGManager.Models.ProductDiscount();
		   if (item.ProductDetail.Product.ProductDiscounts.Any(a => a.DateTo >= DateTime.Today))
		   {
			   productDiscount = item.ProductDetail.Product.ProductDiscounts.Where(a => a.DateTo >= DateTime.Today).FirstOrDefault();
		   }
		   %>
    
        <tr>
              <td><%: (++i).ToString()%></td>	
			  <td>   <%   if (User.IsInRole("Administrator") || !User.IsInRole("Administrator") && item.MemberOrder.OrderStatusID==1 )
				 { %>
					<%: Html.ActionLink("Edit", "AddToOrderBag", new { productID = item.ProductDetail.ProductID, productOrderID = item.MemberOrder.ProductOrderID, memberID = item.MemberOrder.MemberID })%>  
                    <%: Html.ActionLink("Remove", "DeleteMemberOrderDetail", new { productDetailID = item.ProductDetailID, memberOrderID = item.MemberOrderID })%>			
					<%} %>
			  </td>
            <td>
                <%: item.ProductDetail.Product.Name%>
            </td>
           	<% if (item.ProductDetail.Product.CategoryID == 10){ %>
			<td><%: item.ProductDetail.Product.QuantityPerUnit%> </td>
 			<%}else{ %>
			<td><%: item.ProductDetail.Product.ItemCode%> </td>
			<%} %>
		
            <td>
                <%: String.Format(item.ProductDetail.Product.CurrencyCode + "{0:n}", item.ProductDetail.Product.UnitPrice)%>
            </td>
			
			<td nowrap = "nowrap">
				<% if (productDiscount.Discount == 0.3333f){ %>
				    Buy 2 get 1 free, Until <%: string.Format("{0:d}", productDiscount.DateTo)%>
				<% }else if (productDiscount.Discount > 0){ %>
					 <%: (productDiscount.Discount * 100).ToString() + "% (Until " + string.Format("{0:d}", productDiscount.DateTo) + ")"%>  
				<%} %>
			</td>

           <td>
                <%: item.ProductDetail.SizeDescription%>
            </td>
			<td>
                <%: item.Quantity%>
            </td>
            <td>
 			  <%:  String.Format(item.ProductDetail.Product.CurrencyCode + "{0:n}", realPrices[i-1])%>
            </td>
			<td><%: item.MemberOrder.OrderStatus.Status %></td>

            <td>
                 <% if (item.ProductDetail.Product.ProductUploadFiles != null)
					{
						List<SMCHSGManager.Models.UploadFile> uploadFiles = item.ProductDetail.Product.ProductUploadFiles.Select(a => a.UploadFile).ToList();
						foreach (SMCHSGManager.Models.UploadFile uploadFile in uploadFiles)
						{
							string srcImage = uploadFile.FilePath + uploadFile.Name;
							
							if (uploadFile.ContentType.StartsWith("image"))
							{%>
                                     <img src="<%: srcImage %>" height="40" alt="" />
                                    <%}
							else
							{ %>
									<p>
										<%: Html.ActionLink(uploadFile.Name, "FileDownload", "Image", new { imageID = uploadFile.ID }, new { style = "color:Olive " })%>
									</p>
                                        <%--  <% Html.RenderPartial("FileDownload"); %>--%>
                                <%} %>
                            <%} %>
   
            <%} %>
            </td>
        </tr>
     <% } %>
	 </table>

			<h6>Total Price :  <%: Model.FirstOrDefault().ProductDetail.Product.CurrencyCode + String.Format("${0:n}", Model.FirstOrDefault().MemberOrder.Price)%> </h6>
			<div style=" padding: 0.5em 0.5em; text-align: center; font-size: 1.2em; color:Green; font-weight:lighter">
				(Update Time: <%: string.Format("{0:ddd, MMM d yyyy HH:mm}", Model.OrderByDescending(a=>a.MemberOrder.LatestOrderDateTime).FirstOrDefault().MemberOrder.LatestOrderDateTime) %>)
			</div>
			<div style=" padding: 0.5em 0.5em; text-align: center; font-size: 1.2em; color:Green;">
				The total price does not including shipping charges, GST, etc
			</div>
		
<%} %>

     </br>
               <div class="editbuttons" align="center">
 	                 <%   
 	                 	if (User.IsInRole("Administrator") || !User.IsInRole("Administrator") && Model.FirstOrDefault().MemberOrder.OrderStatusID == 1)
                      { %>
			   				<%: Html.ActionLink("More Order", "ProductList", new { productOrderID = Model.FirstOrDefault().MemberOrder.ProductOrderID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
							<%: Html.ActionLink("Cancel Order", "Delete", new { id = Model.FirstOrDefault().MemberOrder.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                   <%} %>

              </div>
    
</div>

</asp:Content>


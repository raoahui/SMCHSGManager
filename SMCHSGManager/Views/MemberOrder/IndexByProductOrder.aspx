<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.ProductDetailListViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	IndexByProductOrder
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">

  <h5><%: ViewData["OrderTitle"]%> </br>  By Product Order</h5>

	<% 	List<SelectListItem> assi = (List<SelectListItem>)ViewData["OrderStatuses"];
		List<DateTime> submitDateTimes = new List<DateTime>();
		if (ViewData["SubmitDateTimes"] != null)
		{
			submitDateTimes = (List<DateTime>)ViewData["SubmitDateTimes"];
		} %>

    <% using (Html.BeginForm())
	   {%>
        <%: Html.ValidationSummary(true)%>

			<%if (Roles.IsUserInRole("SuperAdmin"))
	 {%>
					 <div align="center" style="margin-bottom:20px; color:#2A4013; ">
						Order Status:  &nbsp;  <%: Html.DropDownList("OrderStatusID", assi, new { onChange = "this.form.submit()" })%> 		&nbsp;&nbsp;&nbsp;	
						<% if(submitDateTimes.Count > 0){ %>
							Submit DateTimes:  &nbsp;  <%: Html.DropDownList("submitDateTime", new SelectList(ViewData["SubmitDateTimes"] as IEnumerable), new { onChange = "this.form.submit()" })%> 		
						<%} %>
					</div>	
			<%}%>

<% if (Model.Count() == 0)
   {%> 
            <h5> There is not any product order currently .</h5> 
    <%}
   else
   { %>    

 	<p align="center"> 
		<% if ((DateTime)ViewData["OrderCloseDate"] > DateTime.Now.ToUniversalTime().AddHours(8))
	 { %>
				<%: string.Format("{0:f}", DateTime.Now.ToUniversalTime().AddHours(8))%>
			<%}
	 else
	 { %>
				<%: string.Format("{0:f}", (DateTime)ViewData["OrderCloseDate"])%>
				<%} %>
	</p>

     <table>
        <tr>
            <th></th>
            <th>
                Product Name
            </th>
			<th>
                Item Code
            </th>
              <th>
                Size
            </th>
            <th>
                UnitPrice
            </th>
			<th>
                Qty
            </th>
            <th>
                Sub Price
            </th>
   			<th>Order Status
			</th>
   			<th>Submit DateTime
			</th>
			<th>            
            </th>

        </tr>

    <% 
		int i = 0;
		float allPrice = 0;
		List<decimal> realPrices = (List<decimal>)ViewData["RealPrices"];
		foreach (var item in Model)
		{ 
			//float discount = 0;
			//if(item.ProductDetail.Product.ProductDiscounts.Count> 0)
			//{
			//    discount = item.ProductDetail.Product.ProductDiscounts.FirstOrDefault().Discount;
			//}
			SMCHSGManager.Models.UploadFile uploadFile = new SMCHSGManager.Models.UploadFile ();
			if(item.ProductDetail.Product.ProductUploadFiles.Count >0)
			{
				uploadFile = item.ProductDetail.Product.ProductUploadFiles.FirstOrDefault().UploadFile;
			}
			%>
    
        <tr>
              <td><%: (++i).ToString()%></td>
            <td>
                <%: item.ProductDetail.Product.Name%>
            </td>
  			<td>
                 <%: item.ProductDetail.Product.ItemCode%>
            </td>
		
           <td>
                <%: item.ProductDetail.SizeDescription%>
            </td>
           <td>
				<%--<% float realPrice = (float)item.ProductDetail.Product.UnitPrice * (1 - discount); %>--%>
                <%:  String.Format(item.ProductDetail.Product.CurrencyCode + "{0:n}", realPrices[i-1])%>
            </td>
 			<td>
				<%: Html.ActionLink(item.OrderTotalNumber.ToString(), "NamesByProductDetail", 
					new { productID = item.ProductDetail.Product.ID, 
							sizeDes = item.ProductDetail.SizeDescription,
						  productOrderID = (int)ViewData["productOrderID"],
						  orderStatusID = item.OrderStatus.ID,
						  submitDateTime = item.SubmitDateTime,
					})%>
            </td>
            <td>
			<%  allPrice += (float)realPrices[i - 1]; %>
 			  <%:  String.Format(item.ProductDetail.Product.CurrencyCode + "{0:n}", realPrices[i-1])%>
            </td>
			<td><%: item.OrderStatus.Status %></td>
  			<td><%: String.Format("{0:ddd, MMM d yyyy HH:mm}", item.SubmitDateTime) %></td>
        <td>
                 <%
		if (uploadFile.ID != 0)
		{
			//string srcImage = "/Image/ShowPhoto/" + item.uploadFileID.ToString();
			string srcImage = uploadFile.FilePath + uploadFile.Name; %>
                          <img src="<%: srcImage %>"height="40" alt="" />
					<%} %>
            </td>
        </tr>
    
    <% } %>
          <tr><td colspan = 6 style="text-align:right; font-weight:bold">Total Price :  </td>
		  <td colspan = 4 style="font-weight:bold"> <%:  String.Format(Model.FirstOrDefault().ProductDetail.Product.CurrencyCode + "{0:n}", allPrice)%></td>
		  </tr>
    </table>

  <%}
	   }%>

</div>

</asp:Content>


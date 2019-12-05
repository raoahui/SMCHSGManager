<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.ProductOrderViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Order Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div id="body">
   <div class="fullwidth">        

    
   <% Html.EnableClientValidation(); %>
     <% using (Html.BeginForm()) {%>
   
        <%: Html.ValidationSummary(true) %>

                    <table style="border:0" >
                         <tr><td  style="border:0" align="center">  <h1><%: Model.ProductOrder.Title  %></h1>
                       </td></tr>

					   <tr><td   style="border:0"" >
							Name/itemCode : <%: Html.TextBox("searchContent")%>  
							<input type="submit" value="Search" class ="buttonsearch" name="Search" />
					   </td></tr>
                     
                     <%-- <tr>
                              <%if (Model.MemberInfo != null)
                                 {%>
                                     <td class="formvalue" align="left">
                                        Please Select Order Name  &nbsp; 
                                            <%: Html.DropDownList("MemberID", new SelectList(ViewData["MemberInfo"] as IEnumerable, "MemberID", "Name", Model.MemberInfo.FirstOrDefault().Name))%> 
                                     </td>
  	                        <%} %>
                             </tr>--%>
                    
                       <tr><td style="border:0">
                       </td></tr>
 
            </table>

      <table>
        <tr>
            <th></th>
            <th>
                Name
            </th>
			<th>
                Item Code
            </th>
			<th>
                UnitPrice
            </th>
			<th> 
			    Discount
			</th>
            <th> 
                Picture               
            </th>
	
        </tr>

    <%  int productIndex = 0;
 		  List<SMCHSGManager.Models.ProductDiscount> discounts = (List<SMCHSGManager.Models.ProductDiscount>)ViewData["Discounts"];

    		int qtyIndex = 0;
         foreach (var item in Model.Products) 
		 {
		    SMCHSGManager.Models.ProductDiscount productDiscount = new SMCHSGManager.Models.ProductDiscount();
			if (item.ProductDiscounts.Any(a => a.DateTo >= DateTime.Today))
			{
				productDiscount = item.ProductDiscounts.Where(a => a.DateTo >= DateTime.Today).FirstOrDefault();
			}
		 %>
    
        <tr>
            <td><%: (++productIndex).ToString()%></td>
             <td>
			 <%: Html.ActionLink(item.Name + ' ' + item.NameChi, "AddToOrderBag", "MemberOrder", new { productID = item.ID, productOrderID = Model.ProductOrder.ID, page = ViewData["CurrentPage"], memberID = ViewData["MemberID"] }, null)%> 
				<% if (item.NewProduct) { %>
					<img src="../../images/new1.jpg" />
				<%} %>
            </td>

 			<td>
				<%: item.ItemCode%>
			</td>

            <td>
				<% decimal unitPrice = item.UnitPrice; %>
                <%: String.Format(item.CurrencyCode + "{0:n}" ,unitPrice)%>
            </td>
 		
			<td>
				<% if(productDiscount.Discount == 0.3333f){ %>
				    Buy 2 get 1 free, Until <%: string.Format("{0:d}", productDiscount.DateTo) %>
				<%}else if (productDiscount.Discount > 0){ %>
					 <%: (productDiscount.Discount * 100).ToString() + "% Until " + string.Format("{0:d}", productDiscount.DateTo)%>
				<%} %> 		
			</td>

            <td>
			     <% if (item.ProductUploadFiles != null){
					List<SMCHSGManager.Models.UploadFile> uploadFiles = item.ProductUploadFiles.Select(a => a.UploadFile).ToList();
					foreach (SMCHSGManager.Models.UploadFile uploadFile in uploadFiles)
				    {
						string srcImage = uploadFile.FilePath + uploadFile.Name;
						
						if (uploadFile.ContentType.StartsWith("image")){%>
									<a href="<%: srcImage %>" class="preview"><img src="<%: srcImage %>" height="40" alt="gallery thumbnail" /></a>
						<%}else {%>
									<p>
										<%: Html.ActionLink(uploadFile.Name, "FileDownload", "Image", new { imageID = uploadFile.ID }, new { style = "color:Olive " })%>
									</p>
						<%} %>

	               <%} %>
              <%} %>           
            </td>
			
        </tr>
  	<%} %>  
  
    </table>     

<% } %>

     
	  <div align="center"  style="margin-top:10px" > 
           <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("ProductList", new {productOrderID = Model.ProductOrder.ID , page = p, searchContent = ViewData["searchContent"] }))%>
     </div>

    </div>
</div>

</asp:Content>

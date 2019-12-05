<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.Product>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">


	<% using (Html.BeginForm()) {%>
   <h5> <%: Html.DropDownList("CategoryID", new SelectList(ViewData["Categories"] as IEnumerable, "ID", "Name", (int)ViewData["currentcategoryID"]), new { onChange = "this.form.submit()" })%>   Product List</h5>

		<% if (ViewData["productOrderID"] != null && (int)ViewData["productOrderID"] > 0){%>
			<h6>Please select products for order</h6>
		<%}else{ %>
			<h6> <%: Html.ActionLink("Create New", "Create", null, new { @style = "color:white;", @class = "buttonsmall" })%>  &nbsp; &nbsp;&nbsp; &nbsp;	Name/itemCode : <%: Html.TextBox("searchContent")%>  
							<input type="submit" value="Search" class ="buttonsearch" name="Search" /></h6>
		<%}%>

   <% if (Model.Count() == 0)
      {%>
          <div align="center" style=" margin-bottom:20px; margin-top:15px; color:Green; font-size:14px">
			There is no Product record in database, please use "Create New" button to create new one.
        </div>      
     <% }else {%> 

	<%-- <table>
		 <tr><td   style="border:0"" >
							Name/itemCode : <%: Html.TextBox("searchContent")%>  
							<input type="submit" value="Search" class ="buttonsearch" name="Search" />
		 </td></tr>
	</table>--%>

     <table>
        <tr>

			<th>
			<% if (ViewData["productOrderID"] != null && (int)ViewData["productOrderID"] > 0) {%>
				Select
			<%}%>
			</th>

            <th>S/N</th>
            <th>ID</th>
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

    <%  int i = 0;  
		   List<bool> productUseds = (List<bool>)ViewData["productUseds"];
		   List<SMCHSGManager.Models.ProductDiscount> discounts = (List<SMCHSGManager.Models.ProductDiscount>)ViewData["Discounts"];
        foreach (var item in Model) { %>
    
        <tr>
			
            <td>
			<% 
			if (ViewData["productOrderID"] != null && (int)ViewData["productOrderID"] > 0) {%>
				<%: Html.CheckBox(item.ID.ToString(), productUseds[i])%> 
				<%--<%: Html.CheckBox("selectedProductID",  productUseds[i])%> --%>
			<%}else{ %>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.ID }) %>
				<% if (!productUseds[i]) {%>
                 | <%: Html.ActionLink("Delete", "Delete", new { id = item.ID })%>
				 <%} %>
			<%} %>
            </td>
		
             <td><%: (++i).ToString()%></td>
		     <td><%: item.ID.ToString() %></td>
            <td>
                <%: item.Name + ' ' + item.NameChi %>
            </td>
           	<% if (Model.FirstOrDefault().CategoryID == 10){ %>
			<td>
                <%: item.QuantityPerUnit %>
            </td>
 			<%}else{ %>
			<td>
                 <%: item.ItemCode %>
            </td>
			<%} %>
			<td>
                <%: String.Format(item.CurrencyCode + "${0:n}", item.UnitPrice) %>
            </td>
 			<td>
				<% if (discounts[i-1].Discount > 0) { %>
					 <%: (discounts[i - 1].Discount * 100).ToString() + "% (Until " + string.Format("{0:d}", discounts[i - 1].DateTo) + ")"%>
				<%} %>
            </td>
            <td>
			 
                 <% if (item.ProductUploadFiles != null)
                    {
						List<SMCHSGManager.Models.UploadFile> uploadFiles = item.ProductUploadFiles.Select(a => a.UploadFile).ToList();
						foreach (SMCHSGManager.Models.UploadFile uploadFile in uploadFiles)
						{
									//string srcImage = "/Image/ShowPhoto/" + uploadFile.ID.ToString();
									string srcImage = uploadFile.FilePath + uploadFile.Name; 
                                    if (uploadFile.ContentType.StartsWith("image"))
										{%>
                                     <img src="<%: srcImage %>" height="40" alt="" />
                                    <%}
                                    else
                                    {%>
									<p>
										<%: Html.ActionLink(uploadFile.Name, "FileDownload", "Image", new { imageID =uploadFile.ID }, new { style = "color:Olive " })%>
									</p>
                                       <%--   <% Html.RenderPartial("FileDownload"); %>--%>
                                <%} %>
                            <%} %>
               <%} %>
            </td>

	

        </tr>
    
    <% } %>

    </table>

		<div align="center"  style="margin-top:15px;" > 
			<% if (ViewData["productOrderID"] != null && (int)ViewData["productOrderID"] > 0) {%>
	  	        <input type="submit" value="AddToProductOrder"  name="AddToProductOrder" class ="buttonsmall" />   &nbsp; &nbsp;
				<%: Html.ActionLink("Done", "Index", "ProductOrder", new { recentOrder = false }, new { @style = "color:white; ", @class = "buttonsmall" })%>
			<%} %>
		 </div>

    <% } %>

    <div align="center"  style="margin-top:10px" > 
         <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("Index", new { page = p, categoryID = Model.FirstOrDefault().CategoryID, productOrderID = ViewData["productOrderID"], sort = (string)ViewData["SortItem"], searchContent = ViewData["searchContent"] }))%>
   </div>

<%} %> 

</div>

</asp:Content>


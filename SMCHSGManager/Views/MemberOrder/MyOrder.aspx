<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.MemberOrder>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Order List By Name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

<% if ((int)ViewData["MemberOrderCount"] == 0) {%> 
            <h5> You don't have any Order .</h5> 
    <%}else{%>
            <h5>Your Order List</h5>

     <h6>Name 姓名: <%: Model.FirstOrDefault().MemberInfo.Name%></h6>
     <h6>ID Card No 识别证编号 : <%: Model.FirstOrDefault().MemberInfo.IDCardNo%></h6>
    <table>
        <tr>
           <th>No</th>
           <th>Title</th>
           <th>Order Date</th>
           <th>Total Price</th>
        <%--   <th>Product Details</th>--%>
 		   <th>OrderStatus</th>
   		   <th>Submit DateTime</th>
        </tr>
        
    <% int i = 0;
       foreach (var item in Model)
       { %>
    
        <tr>
             <%--<td><%: (++i).ToString()%></td>--%>
            <td>  
                   <%: Html.ActionLink((++i).ToString(), "Details", new
{
	memberOrderID = item.ID,
	orderStatusID = item.OrderStatusID,
	submitDateTime = item.SubmitDateTime,
}, null)%>
            </td>

            <td nowrap="nowrap">
                <%: item.ProductOrder.Title%>
            </td>
            
             <td >
                 <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", item.LatestOrderDateTime)%>
            </td>

            <td>
                <%: String.Format(item.CurrencyCode + "{0:n}", item.Price) %>
            </td>

           <%-- <td nowrap="nowrap" >
               <%   int k = 0;
                    foreach (SMCHSGManager.Models.MemberOrderDetail memberOrderDetail in item.MemberOrderDetails)
                    {
                        string temp = memberOrderDetail.Product.Name + " " + memberOrderDetail.Quantity.ToString() + " " + memberOrderDetail.UnitPrice.ToString();%>
                         <%: temp%>
                  <%  }%>
          </td>--%>

 		  <td>
			<%: item.OrderStatus.Status %>
		</td>
   			<td><%: String.Format("{0:ddd, MMM d yyyy HH:mm}", item.SubmitDateTime) %></td>
      </tr>
    
    <% } %>

    </table>

<%} %>

</div>

    <div align="center" style="margin-bottom:20px; margin-top:20px">
          <a href="javascript:history.go(-1)" style = "color:white;" class="buttonsmall" >Back</a>
    </div>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.MemberOrder>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	IndexByMemberOrder
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
<div id="body">

 
	<% 	List<SelectListItem> assi = (List<SelectListItem>)ViewData["OrderStatuses"];
		List<DateTime> submitDateTimes = new List<DateTime>();
		if (ViewData["SubmitDateTimes"] != null)
		{
			submitDateTimes = (List<DateTime>)ViewData["SubmitDateTimes"];
		} %>

     <% using (Html.BeginForm())
		{%>
        <%: Html.ValidationSummary(true)%>

			<%if (Roles.IsUserInRole("SuperAdmin")) {%>
					 <div align="center" style="margin-bottom:20px; color:#2A4013; ">
						Order Status:  &nbsp;  <%: Html.DropDownList("OrderStatusID", assi, new { onChange = "this.form.submit()" })%> 		&nbsp;&nbsp;&nbsp;	
						<% if(submitDateTimes.Count > 0){ %>
							Submit DateTimes:  &nbsp;  <%: Html.DropDownList("submitDateTime", new SelectList(ViewData["SubmitDateTimes"] as IEnumerable), new { onChange = "this.form.submit()" })%> 		
						<%} %>
					</div>	
			<%}%>


<% if (Model.Count() == 0)
   {%> 
             <h5> There is not any member order currently .</h5> 
    <%}
   else
   { %>    
     <h5> <%: Model.First().ProductOrder.Title%></br>By Member Order</h5>
  
	<p align="center"> 
		<% if (Model.First().ProductOrder.OrderCloseDate > DateTime.Now.ToUniversalTime().AddHours(8))
	 { %>
				<%: string.Format("{0:f}", DateTime.Now.ToUniversalTime().AddHours(8))%>
			<%}
	 else
	 { %>
				<%: string.Format("{0:f}", Model.First().ProductOrder.OrderCloseDate)%>
				<%} %>
	</p>

    <table>
        <tr>
            <th ></th>
           
            <th >
                Name
            </th>
            <th>
                Contact No.
            </th>
             
           <th >
                Price
            </th>
            <th>
                LatestOrderDateTime
            </th>
			<th>OrderStatus
			</th>
  			<th>Submit DateTime
			</th>
          </tr>


    <% int i = 0;
	   foreach (var item in Model)
	   { %>
    
        <tr>
              <td>
                  <%: Html.ActionLink((++i).ToString(), "Details", new {
							memberOrderID = item.ID,  
							orderStatusID = item.OrderStatusID, 
							submitDateTime = item.SubmitDateTime}, null)%>
            </td>
           <td>
                <%: item.MemberInfo.Name%>
            </td>
            <td>
                <%: item.MemberInfo.ContactNo%>
            </td>

             <td>
                <%: item.CurrencyCode + String.Format("{0:n}", item.Price)%>
            </td>
            <td>
                <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", item.LatestOrderDateTime)%>
            </td>
 
			<td><%: item.OrderStatus.Status %></td>
 
 			<td><% if (item.SubmitDateTime.HasValue)
		   { %>
			<%: String.Format("{0:ddd, MMM d yyyy HH:mm:ss}", item.SubmitDateTime)%>
			<%} %>
			</td>
       </tr>
    
    <% } %>

         <tr >
          <td></td>
        <td></td>                
        <td></td>      
        
          <td colspan = 5 align="left" style="font-weight:700; ">   <%: Model.FirstOrDefault().CurrencyCode + String.Format("{0:n}", ViewData["TotalPrice"])%></td>
        </tr>

    </table>

<p align="Center">
	   <% if( assi.Single(a => a.Selected).Value == "1" ){ %>
			<%: Html.ActionLink("Submit Order", "OrderSubmit", new { productOrderID = Model.FirstOrDefault().ProductOrderID, orderStatusID = int.Parse(assi.Single(a => a.Selected).Value) }, new { @style = "color:white;", @class = "buttonsmall" })%>
	 <%} %>
</p>

<%}%>

<%}%>


</div>

 
</asp:Content>


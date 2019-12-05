<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.ProductOrderViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Product Order Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

      <div class="fullwidth">        
            <div class="listitem">
                <div class = "titlenextlink">
              <% if (User.IsInRole("Administrator"))
                 {%> 
                    <%: Html.ActionLink("Order List", "Index", new {recentOrder = ViewData["RecentOrder"]}, new { @style = "color:white;", @class = "buttonsmall" })%>
             <%} %>
                 </div>
            </div>

               <div class="listitem">
               
					   <div class="title">
                         <%: Model.ProductOrder.Title %>
						</div>
                       <div class="dashedline">    </div>

                    
                           <p>
                               Order Open Date <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", Model.ProductOrder.OrderOpenDate)%>  --  Close Date  <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", Model.ProductOrder.OrderCloseDate.AddMinutes(-1))%>
                          </p>

						  <div class="itemdetails">
                           <%  if (Model.ProductOrder.Description != null)
                                {%>
									<%: MvcHtmlString.Create(Model.ProductOrder.Description)%>
                           <%} %> 
						  </div>

                    </br>
       
	
                           <% DateTime now = DateTime.Now.ToUniversalTime().AddHours(8);
							  if (Model.MemberOrderID != 0 && Model.ProductOrder.MemberOrders.Any(a=>a.OrderStatusID == 1))
							  {%>
                                <p>
                                        You have items in your order bag.
                                     <%: Html.ActionLink("Details", "Details", "MemberOrder", new { memberOrderID = Model.MemberOrderID, orderStatusID = 1 }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                 </p>
                            <%}
							  else if (now >= Model.ProductOrder.OrderOpenDate && now < Model.ProductOrder.OrderCloseDate) 
                              { %>
                                 <p>
                                     <%: Html.ActionLink("Order", "ProductList", "MemberOrder", new { productOrderID = Model.ProductOrder.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                     Please Order Before <%: string.Format("{0: ddd, MMM d yyyy HH:mm}", Model.ProductOrder.OrderCloseDate.AddMinutes(-1))%>
                                 </p>
                              <%}%>            
							           
                     </br>
                
                       <div class="actionbuttons" >
                       <% if (User.IsInRole("Administrator"))
                          {%> 
                                 <%-- <%: Html.ActionLink("OrderFromAdmin", "ProductList", "MemberOrder", new { productOrderID = Model.ProductOrder.ID, forOthers = true }, new { @style = "color:white;", @class = "buttonsmall" })%> --%>
 
                                  <%   if(Model.ProductOrder.MemberOrders.Count() > 0){ %>
                                           <%: Html.ActionLink("ProductOrderList", "IndexByProductOrder", "MemberOrder", new { productOrderID = Model.ProductOrder.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                           <%: Html.ActionLink("MemberOrderList", "IndexByMemberOrder", "MemberOrder", new { productOrderID = Model.ProductOrder.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                  <%} %>

                                 <%if (Model.ProductOrder.OrderOpenDate <= DateTime.Today.ToUniversalTime().AddHours(8) && Model.ProductOrder.OrderCloseDate >= DateTime.Today.ToUniversalTime().AddHours(8))
                                  { %>
                                          <%: Html.ActionLink("Edit", "Edit", new { id = Model.ProductOrder.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                 <%} %>

                        <%} %>
                        </div>

                        <div class="dashedline"></div>

                        <div class="prenextlink">
                         <%  	int current = Model.ProductOrderIDs.IndexOf(Model.ProductOrder.ID); %>
 					  
                           <%if (current > 0)
                             { %>
									<%: Html.ActionLink("Previous Order", "Details", new { id = Model.ProductOrderIDs[current - 1],  recentOrder = ViewData["RecentOrder"] }, new { @style = "color:white;", @class = "buttonsmall" })%>
 							 <%} %>
                       
                        <%if (current < Model.ProductOrderIDs.Count - 1) 
                          { %>
						    <div class="nextlink">
									<%: Html.ActionLink("Next Order", "Details", new { id = Model.ProductOrderIDs[current + 1], recentOrder = ViewData["RecentOrder"] }, new { @style = "color:white;", @class = "buttonsmall" })%>
						  </div>
                        <%}%>
                        </div>

            <div class="clear2column"></div>

        </div>
    </div>
</div>

</asp:Content>

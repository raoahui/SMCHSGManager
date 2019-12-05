<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.ProductOrder>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Product Order List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

    <div class="fullwidth">    
          
            <div class="listitem">
                <div class = "nextlink">
                   <% if (User.IsInRole("Administrator") && !(bool)ViewData["RecentOrder"])
                  {%> 
                     <%: Html.ActionLink("Add New ProductOrder", "Create", null, new { @style = "color:white;", @class = "buttonsmall" })%>
                 <%} %>
                  </div>
            </div>

			<% if((bool)ViewData["RecentOrder"]){ %>
				<h2> Recent Order List</h2>
			<%}else{ %>
				<h2> Order List</h2>       
            <%} %>
                 <% int i=0; 
                    List<int> OrderStatus = (List<int>)(ViewData["OrderStatus"]);
                 	List<SMCHSGManager.Models.UploadFile> uploadFiles = (List<SMCHSGManager.Models.UploadFile>)ViewData["ImageFiles"] ;
                    foreach (var item in Model)
                    { %>
                        <div class="listitem">
                            <div class="dashedlineIndex"></div>

							<% if(!(bool)ViewData["RecentOrder"]){ %>
                             <div class="thumbnail">
                                   <% if(User.IsInRole("Administrator")) {%> 
                                          <%: Html.ActionLink("Edit", "Edit", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>  
                                          <%: Html.ActionLink("Remove", "Delete", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>
                                   <%} %>
                              </div>
							<%} %>

                            <div class="thumbnail">
								<%  
						if(uploadFiles[i].ID != 0){  
										string srcImage = uploadFiles[i].FilePath + uploadFiles[i].Name; 
										if (uploadFiles[i].ContentType.StartsWith("image")) {%>
											<img src="<%: srcImage %>" height="40" alt="" />
										<%} %>
								<%} %>
 							</div>
					                                              
                           <p>
                                  <%: Html.ActionLink(item.Title + " Order", "Details", "ProductOrder", new { id = item.ID, recentOrder = ViewData["RecentOrder"] }, new { @style = "font-weight:bolder;" })%>    
                             </p>    
                           <p>
                                Order Open : <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", item.OrderOpenDate)%>  &nbsp; &nbsp; 
                                    To  : <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", item.OrderCloseDate.AddMinutes(-1))%>   
                             </p>    
 

                            <% DateTime now = DateTime.Now.ToUniversalTime().AddHours(8); 
								if(OrderStatus[i] > 0) {%>
                                <p>
                                        You have already ordered.
                                     <%: Html.ActionLink("Details", "Details", "MemberOrder", new{memberOrderID = OrderStatus[i], orderStatusID = 1}, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                 </p>
                            <%}
                               else if (now >= item.OrderOpenDate && now < item.OrderCloseDate) 
                              { %>
                                 <p>
                                     <%: Html.ActionLink("Order", "ProductList", "MemberOrder", new { productOrderID = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                     Please Order Before <%: string.Format("{0: ddd, MMM d yyyy HH:mm}", item.OrderCloseDate.AddMinutes(-1))%>
                                 </p>
                              <%}%>

                              <p>
								<% string des = SMCHSGManager.Models.GeneralFunction.RemoveHtmlFormat(item.Description); %>
							<%--	<%: Html.Truncate(des, 50)%> <%: Html.ActionLink("read more", "Details", new { id = item.ID })%> &raquo--%>
                            </p>
                            <div class="clearlist"> </div>

     
                <% i++;
                    } %>
 
               <%-- <div class="newscrumbs">
                    Page: <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("Index", new { page = p }))%>
                 </div>--%>
  
            </div>
 
       
</div>

     <div class="listitem">                
                    <div class="dashedlineIndex"></div>
    
                           <div class="newscrumbs">
                  
                              Page: <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("Index", new { page = p, recentOrder = (bool)ViewData["RecentOrder"]}))%>
                          
                           </div>
          
                 </div>
 </div>



</div>
</asp:Content>


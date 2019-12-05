<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.MemberFeePaymentListViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
   <div align="center" style=" margin-top:20px; color:#2A4013; font-size:1.6em; font-family:Verdana; font-weight:bold">
        List of Member Fee Payment
   </div>
   
         <% using (Html.BeginForm()) {%>

   <% 
				if (Model.Count() == 0)
      {%>
          <div align="center" style=" margin-bottom:20px; margin-top:15px; color:Green; font-size:14px">
			<%: ViewData["Message"] %>
        </div>   
			<p align="center">
				<%: Html.ActionLink("Create New", "Create", "MemberFeePayment", null, new { @style = "color:white;", @class = "buttonsearch" })%> 
			</p>
   
     <% }
      else
      {%>  
	    
	<table style="border:0">
		<tr>
			<td style="border:0">
              Name/MemberNo:  <%: Html.TextBox("searchContent")%>  
               <input type="submit" value="Search" class ="buttonsearch" name="Search" />
			</td>
			<td style="width:20%; border:0"></td>
			<td style="border:0;">
				<%: Html.ActionLink("Create New", "Create", "MemberFeePayment", null, new { @style = "color:white;", @class = "buttonsearch" })%> 
			</td>
		</tr>
	</table>
      
             <%=Html.DataGrid<SMCHSGManager.ViewModel.MemberFeePaymentListViewModel>()%>
   
    <% } %>

    <div align="center"  style="margin-top:10px" > 
           <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("Index", new { page = p, sort = (string)ViewData["SortItem"], searchContent = ViewData["searchContent"], memberID = ViewData["MemberID"] }))%>
     </div>
 
     <% } %>

</div>


</asp:Content>


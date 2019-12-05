<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.PublicMemberInfo>>" %>
<%@ Import Namespace = "SMCHSGManager.ViewModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div id="body">
   <div align="center" style=" margin-top:20px; color:#2A4013; font-size:1.6em; font-family:Verdana; font-weight:bold">
        List of Ordinary Member
   </div>
   
         <% using (Html.BeginForm()) {%>
  
	<table style="border:0">
		<tr>
			<td style="width:20%; border:0">
				<%= Html.ActionLink("Get Excel", "GenerateExcel2", "OrdinaryMember")%>  
			</td>
			<td style="border:0">
              Name/MemberNo:  <%: Html.TextBox("searchContent")%>  
               <input type="submit" value="Search" class ="buttonsearch" name="Search" />
				<%:  ViewData["message"] %>
			</td>
			<td style="border:0;">
				<% if ((bool)ViewData["IsFromLocalHost"])
				{ %>
				<%: Html.ActionLink("Create New", "Create", "OrdinaryMember", null, new { @style = "color:white;", @class = "buttonsearch" })%> 
				<%} %>
			</td>
		</tr>
	</table>
      
             <%=Html.DataGrid<PublicMemberInfo>()%>
             <%-- <%=Html.DataGrid<OrdinaryMemberListViewModel>()%>--%>
   
    <% } %>

    <div align="center"  style="margin-top:10px" > 
           <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("Index", new { page = p, sort = (string)ViewData["SortItem"], searchContent = ViewData["searchContent"],  IsActive = ViewData["IsActive"]}))%>
     </div>

 
</div>

</asp:Content>


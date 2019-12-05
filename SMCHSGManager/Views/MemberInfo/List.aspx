<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.MemberOnlineInfo>>" %>
<%@ Import Namespace = "SMCHSGManager.ViewModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	 Member Info List (Paged)</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
   <div align="center" style=" margin-top:20px; color:#2A4013; font-size:1.6em; font-family:Verdana; font-weight:bold">
        List of User
   </div>
   
         <% using (Html.BeginForm()) {%>
            <div class="actionbuttons">
 
               Name :  <%: Html.TextBox("searchContent")%>  
               <input type="submit" value="Search" class ="buttonsearch" name="Search" />
				<br />
				<%:  ViewData["message"] %>

           </div>
            
              <%=Html.DataGrid<MemberOnlineInfo>()%>
   
    <% } %>

    <div align="center"  style="margin-top:10px" > 
              <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("List", new { page = p, sort = (string)ViewData["SortItem"], searchContent = ViewData["searchContent"], initiateTypeID = ViewData["initiateTypeID"], IsActive = ViewData["IsActive"]}))%>
     </div>

   <%-- <div align="center" style="margin-bottom:20px; margin-top:20px">
           <%: Html.ActionLink("Create New", "Register", "Account", new {initiateOnly = true }, new { @style = "color:white;", @class = "buttonsmall" })%> 
    </div>--%>
 
</div>

</asp:Content>




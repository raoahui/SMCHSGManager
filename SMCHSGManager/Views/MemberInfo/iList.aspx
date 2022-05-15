<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.PublicMemberShortInfo>>" %>
<%@ Import Namespace = "SMCHSGManager.ViewModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	 Member Info List (Paged)</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
   <div align="center" style=" margin-top:20px; color:#2A4013; font-size:1.6em; font-family:Verdana; font-weight:bold">
        List of User of Membership with short format
   </div>
   
         <% using (Html.BeginForm()) {%>
            <div class="actionbuttons">
 
               Name :  <%: Html.TextBox("searchContent")%>  
               <input type="submit" value="Search" class ="buttonsearch" name="Search" />
				<br />
				<%:  ViewData["message"] %>

           </div>
            
              <%=Html.DataGrid<PublicMemberShortInfo>()%>
   
    <% } %>

    <div align="center"  style="margin-top:10px" > 
              <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("List", new { page = p, sort = (string)ViewData["SortItem"], searchContent = ViewData["searchContent"], initiateTypeID = ViewData["initiateTypeID"], IsActive = ViewData["IsActive"]}))%>
     </div>
 
</div>

</asp:Content>




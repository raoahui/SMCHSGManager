<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.Product>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
    <h5>Delete Confirmation</h5>
  <div class="centerblock">

   <%if(TempData["ProductStatus"] != null){  %>

   <br /><br />
    <h6><%: (string)TempData["ProductStatus"] %></h6>

    <%}else{ %>
          <h6>Are you sure you want to delete this Product? </h6>

             <% using (Html.BeginForm()) { %>
                <div align="center" style="margin-bottom:20px; margin-top:20px">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
             </div>

               <% } %>
     <%} %>

    </div>
</div>

</asp:Content>

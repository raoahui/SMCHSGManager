<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.Announcement>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div id="body">

   <h5>Delete Confirmation</h5>

        <div align="center">
            <h4>Are you sure you want to delete titled 
             <strong><%: Model.Name %></strong>?
            </h4>
            </br>
 </div>
             <% using (Html.BeginForm()) { %>
              <div class="actionbuttons">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>
                    <%: Html.ActionLink("Cancel", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
            </div>
 
             <% } %>
       
        
     </div>--%>

     <% Html.RenderPartial("DeleteTemplate", Model.Name + " Announcement"); %>
   
</asp:Content>


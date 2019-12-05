<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.ProductOrder>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%--<div id="body">

   <h5>Delete Confirmation</h5>

        <div align="center">
            <h4>Are you sure you want to delete titled 
             <strong><%: Model.Title %></strong>?
            </h4>
            </br>

             <% using (Html.BeginForm()) { %>
                <div class="editbuttons">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
                </div>
             <% } %>
        </div>
        
     </div>--%>
     
     <% Html.RenderPartial("DeleteTemplate", Model.Title + " Product Order"); %>

</asp:Content>

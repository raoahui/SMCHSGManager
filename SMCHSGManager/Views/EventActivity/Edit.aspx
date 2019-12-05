<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.EventActivity>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="body">
    <h5>Edit</h5>

     <div class="centerblock">

     <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

             <%: Html.EditorFor(model=>model)%>

               <div class="editbuttons" align="center">
		            <input type="submit" value="Save"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Index", null, new{ @style = "color:white;", @class = "buttonsmall" })%>
             </div>
         

    <% } %>
   
     </div>
</div>
</asp:Content>


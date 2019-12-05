<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<string>" %>

 <div id="body">

   <h5>Delete Confirmation</h5>
         <div align="center">
            <h4>Are you sure you want to delete
                    <strong style="color:Green"><%: Model %></strong>?
            </h4>
            </br>

             <% using (Html.BeginForm()) { %>

             <div class="editbuttons" align="center">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
             </div>
             <% } %>
        </div>
        
</div>

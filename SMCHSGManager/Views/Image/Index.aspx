<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.UploadFile>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="body">
   <h5>Photo List</h5>

    <% if ((int)ViewData["imageCount"] == 0)
      {%>
          <div align="center" style=" margin-bottom:20px; margin-top:15px; color:Green; font-size:14px">
         There is no Image record in database, please use "Create New" button to create new one.
        </div>      
     <% }
      else
      {%> 
        <table align="center">
        <tr>
            <th>S/N</th>
            <th></th>
            <th>
                ID
            </th>
             <th>
                Name
            </th>
             <th>
                Content Type
            </th>
            <th>
                Upload Time
            </th>

            <th>Preview</th>
        </tr>

    <% int i = 0;
          List<int> imageUsed = (List<int>)ViewData["imageUsed"];
          foreach (var item in Model) { %>
    
        <tr>
            <td><%: (++i).ToString()%></td>
            <td nowrap="nowrap">
                 <%if(!imageUsed.Contains(item.ID)) {%>
                    <%: Html.ActionLink("Delete", "Delete", new { id=item.ID })%>
                 <%} %>
            </td>
			<td><%: item.ID.ToString() %></td>
             <td>
                <%: item.Name %>
            </td>
            <td>
                <%: item.ContentType %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.UploadTime) %>
            </td>


            <td>
            <%if (item.ContentType.StartsWith("image"))
              { %>
                    <% 
						//string srcImage = "/Image/ShowPhoto/" + item.ID.ToString();
						string srcImage = item.FilePath + item.Name;
						%>
                    <img src="<%: srcImage %>"  height="70" alt="" />
            <%} %>
            </td>
          
    </tr> 
    
    <% } %>

    </table>
   <% } %>

    <div align="center"  style="margin-top:10px" > 
         <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("Index", new { page = p,  sort = (string)ViewData["SortItem"] }))%>
   </div>

 
              <div class="editbuttons" align="center">
                    <%: Html.ActionLink("Create New", "Create", null, new { @style = "color:white;", @class = "buttonsmall" })%>
             </div>
    
</div>
</asp:Content>


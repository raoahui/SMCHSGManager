<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.VolunteerJobType>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="body">
   <h5>Volunteer Job Type List</h5>
     
    <table>
        <tr>
            <th></th>
            <th>
                ID
            </th>
            <th>
                Name
            </th>
            <th>
                Remark
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.ID }) %> |
                <%: Html.ActionLink("Details", "Details", new { id=item.ID })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id=item.ID })%>
            </td>
            <td>
                <%: item.ID %>
            </td>
            <td>
                <%: item.Name %>
            </td>
            <td>
                <%: item.Remark %>
            </td>
        </tr>
    
    <% } %>

    </table>

   <div align="center" style="margin-bottom:20px; margin-top:20px">
        <%: Html.ActionLink("Create New", "Create") %>
        &nbsp;&nbsp;&nbsp;&nbsp; <%: Html.ActionLink("Back", "Details", "Event", new { id = (int)ViewData["LocalRetreatID"] }, new { @style = "color:white;", @class = "buttonsmall" })%> 

    </div>
</div>
</asp:Content>


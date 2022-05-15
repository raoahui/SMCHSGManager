<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.LocalRetreatScheduleTemplate>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <h5>Local Retreat Schedule List Model <%: ViewData["modelID"] %></h5>
 
    <table>
        <tr>
            <th></th>
            <th>
                Local Retreat Activity
            </th>
            <th>
                Schedule Offset (Hours)
            </th>
            <th>
                DP_PersonNeeded
            </th>
            <th>
                Video_PersonNeeded
            </th>
            <th>
                Clean_PersonNeeded
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.ID }) %> |
                <%: Html.ActionLink("Delete", "Delete", new { id=item.ID })%>
            </td>
       
            <td>
                <%: item.EventActivity.Name %>
            </td>
            <td>
                <%: item.ScheduleOffset.OffsetHours %>
            </td>
            <td>
                <%: item.DP_PersonNeeded %>
            </td>
            <td>
                <%: item.Video_PersonNeeded %>
            </td>
            <td>
                <%: item.Clean_PersonNeeded %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p align=center>
        <%: Html.ActionLink("Create New", "Create", new { modelID = ViewData["modelID"] })%>
    </p>

</asp:Content>


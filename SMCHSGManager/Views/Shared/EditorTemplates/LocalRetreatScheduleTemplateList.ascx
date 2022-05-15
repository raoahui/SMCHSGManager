﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SMCHSGManager.Models.LocalRetreatScheduleTemplate>>" %>
  
  <h5>Local Retreat Schedule List Model <%: Model.FirstOrDefault().Model %></h5>
 
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
                <%: Html.ActionLink("Details", "Details", new { id=item.ID })%> |
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

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>



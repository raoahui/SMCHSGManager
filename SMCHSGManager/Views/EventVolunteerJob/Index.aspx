<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.EventVolunteerJob>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="body">
   <h5>Local Retreat Volunteer Job List</h5>

    <table>
        <tr>
             <th></th>
            <th>
                From
            </th>
            <th>
                To
            </th>
            <th>
                Name
            </th>
            <th>
                Persons Needed
            </th>
            <th>
                Persons Taked
            </th>
        </tr>

    <% int i = 0; 
        foreach (var item in Model) { %>
    
        <tr>
              <td><%: (++i).ToString()%></td>
            <td nowrap=nowrap>
                <%:  Html.DisplayFor(EventVolunteerJob => item.EventSchedule.DateTimeFrom)%>
             </td>
            <td nowrap=nowrap>
                <% DateTime DateTimeTo = item.EventSchedule.DateTimeFrom.AddHours(item.EventSchedule.ScheduleOffset.OffsetHours); %>
                <%= Html.Encode(String.Format("{0:g}", DateTimeTo))%> 
             </td>
            <td nowrap=nowrap>
                <%: item.VolunteerJobType.Remark %>
            </td>
            <td>
                <%: item.PersonsNeeded %>
            </td>
            <td>
                <%: item.PersonsTaked%>
            </td>

         </tr>
    
    <% } %>

    </table>


    <div align="center" style="margin-bottom:20px; margin-top:20px">
       <%: Html.ActionLink("Back", "Details", "Event", new { id = (int)ViewData["LocalRetreatID"] }, new { @style = "color:white;", @class = "buttonsmall" })%> 
   </div>

</div>
</asp:Content>


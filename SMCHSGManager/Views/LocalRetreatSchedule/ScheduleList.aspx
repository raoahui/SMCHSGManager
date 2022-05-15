<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.LocalRetreatScheduleTemplate>>" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.EventSchedule>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ScheduleList
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ScheduleList</h2>

<table style="font-size: 14px;">
        <tr>
            <th></th>
            <th>
                Local Retreat Activity
            </th>
            <th>
                From
            </th>
            <th>
                To
            </th>
        </tr>

    <% int i=0;
       foreach (var item in Model)
       {
           if (!item.EventActivity.Name.StartsWith("Bless"))
           {%>
    
        <tr>
             <td><%: (++i).ToString()%></td>
            <td>
                <%: item.EventActivity.Name%>
            </td>
			<% if (i == 1){ %>
           <td colspan =2 align=center>
                <%= Html.DisplayFor(EventScheduleMetaDate => item.DateTimeFrom)%>
           </td>
		   <%}else{ %>
            <td>
                <%= Html.DisplayFor(EventScheduleMetaDate => item.DateTimeFrom)%>
           </td>
            <td>
            <% DateTime DateTimeTo = item.DateTimeFrom.AddHours(item.ScheduleOffset.OffsetHours); %>
                <%= Html.Encode(String.Format("{0:d MMM yyyy HH:mm }", DateTimeTo))%> 
            </td>
			<%} %>
        </tr>

		<% if (i == 2)
		 { %> 
		 <tr>
             <td><%: (++i).ToString()%></td>
            <td style="color:Navy"><strong> Door Closes</strong>
            </td>
            <td colspan =2 align=center style="color:Navy">
			  <% DateTime DateTimeTo = item.DateTimeFrom.AddHours(item.ScheduleOffset.OffsetHours); %>
                <strong> <%= Html.Encode(String.Format("{0:d MMM yyyy HH:mm }", DateTimeTo))%> </strong>
           </td>
        </tr>

		<%} %>

    <% }
       }%>

    </table>


    <table>
        <tr>
            <th></th>
            <th>
                ID
            </th>
            <th>
                Model
            </th>
            <th>
                EventActivityID
            </th>
            <th>
                ScheduleOffsetID
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
                <%: item.ID %>
            </td>
            <td>
                <%: item.Model %>
            </td>
            <td>
                <%: item.EventActivityID %>
            </td>
            <td>
                <%: item.ScheduleOffsetID %>
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

</asp:Content>


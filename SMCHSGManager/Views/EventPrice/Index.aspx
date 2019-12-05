<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.EventPrice>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div id="body">
   <h5><%: Model.FirstOrDefault().Event.Title + ' ' + Model.FirstOrDefault().Event.EventType.Name %> Activities</h5>

    <table>
        <tr>
            <th></th>
            <th>
                Name
            </th>
            <th>
                UnitPrice
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { eventID = item.EventID, eventActivityID = item.EventActivityID })%> |
            </td>
            <td>
                <%: item.EventActivity.Name %>
            </td>
            <td>
                <%: String.Format("{0:F}", item.UnitPrice) %>
            </td>
        </tr>
    
    <% } %>

    </table>

   <div align="center" style="margin-bottom:20px; margin-top:20px">
        <%: Html.ActionLink("Back", "Details", "Event", new { id = Model.FirstOrDefault().EventID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
   </div>

</div>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.GroupMeditation>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="body" align="center">
    <h2>Group Meditation List</h2>
    </br >
    <table>
        <tr>
           <th>
             </th>
      <%--      <th>
                Title
            </th>--%>
           <th>
                Date
            </th>
           <th>
              Time
            </th>
        </tr>

    <% int i = 1; 
        foreach (var item in Model) { %>
    
        <tr>
            <td> 
               <%: (i++).ToString() %>
            </td>
         <%--   <td>
                <%: item.Title %>
            </td>--%>
             <td> <%: Html.ActionLink(String.Format("{0:ddd, d MMM yyyy}", item.StartDateTime), "Details", new { eventID = item.ID, descending = true })%>
               <%-- <%: String.Format("{0:ddd, d MMM yyyy}", item.StartDateTime)%>--%>
            </td>
             <td>
                <%: String.Format("{0:HH:mm}", item.StartDateTime)%> ~  <%: String.Format("{0:HH:mm}", item.EndDateTime)%>
            </td>
 
        </tr>
    
    <% } %>

    </table>

  <%--  <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>--%>

</div>

</asp:Content>


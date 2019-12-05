<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.initiateEmail>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UpdateEmailAndGendar
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="body">
    <h2>UpdateEmailAndGendar</h2>

    <table>
        <tr>
            <th>
                No
            </th>
            <th>
                Name
            </th>
            <th>
                Email
            </th>
            <th>
                GendarID
            </th>
            <th>
                IsValid
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
              <td>
                <%: item.No %>
            </td>
            <td>
                <%: item.Name %>
            </td>
            <td>
                <%: item.Email %>
            </td>
            <td>
                <%: item.GendarID %>
            </td>
            <td>
                <%: item.IsValid %>
            </td>
        </tr>
    
    <% } %>

    </table>

 </div>

</asp:Content>


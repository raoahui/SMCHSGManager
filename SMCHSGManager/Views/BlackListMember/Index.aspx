<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.BlackListMember>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">     

	<h5>Black List Member List</h5>

    <table>
        <tr>
            <th></th>
            <th>
                No
            </th>
            <th>
                Name
            </th>
            <th>
                ID Card Type
            </th>
            <th>
                DateFrom
            </th>
            <th>
                DateTo
            </th>
            <th>
                Remark
            </th>
        </tr>

    <% int i = 0;
    	foreach (var item in Model)
	   { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id = item.ID })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id = item.ID })%>
            </td>
            <td>
                <%: (++i).ToString() %>
            </td>
            <td>
                <%: item.MemberInfo.Name%>
            </td>
             <td>
                <%: item.IDCardType.Name%>
            </td>
           <td>
                <%: String.Format("{0:d MMM yyyy}", item.DateFrom)%>
            </td>
            <td>
                <%: String.Format("{0:d MMM yyyy}", item.DateTo)%>
            </td>
            <td>
                <%: item.Remark%>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p align="center">
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</div>

</asp:Content>


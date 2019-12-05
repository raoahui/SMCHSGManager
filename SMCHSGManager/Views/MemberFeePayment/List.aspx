<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.MemberFeePayment>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List</h2>

    <table>
        <tr>
            <th></th>
            <th>
                IMemberID
            </th>
            <th>
                FromDate
            </th>
            <th>
                ToDate
            </th>
            <th>
                PayAmount
            </th>
            <th>
                PayMethodID
            </th>
            <th>
                ReceivedDate
            </th>
            <th>
                AttachedFileID
            </th>
            <th>
                Remark
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.IMemberID }) %> |
                <%: Html.ActionLink("Details", "Details", new { id=item.IMemberID })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id=item.IMemberID })%>
            </td>
            <td>
                <%: item.IMemberID %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.FromDate) %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.ToDate) %>
            </td>
            <td>
                <%: String.Format("{0:c}", item.PayAmount) %>
            </td>
            <td>
                <%: item.PayMethodID %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.ReceivedDate) %>
            </td>
            <td>
                <%: item.AttachedFileID %>
            </td>
            <td>
                <%: item.Remark %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>


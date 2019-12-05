<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.MemberFeePayment>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	MemberFeeExpiredDateList
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>MemberFeeExpiredDateList</h2>

    <table>
        <tr>
            <th>
                MemberNo
            </th>
            <th>
                Name
            </th>
            <th>
                ToDate
            </th>
         </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
             <td>
                <%: item.MemberInfo.MemberNo %>
            </td>
            <td>
                 <%: item.MemberInfo.Name %>
            </td>
            <td>
                <%: String.Format("{0:d MMM yyyy}", item.ToDate) %>
            </td>

        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.GroupMeditation>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	CreateGMEventYearly
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <script src="/Scripts/MicrosoftAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>    

<script type="text/javascript">

    function AnimateRSVPMessage() {
        $("#rsvpmsg").animate({ fontSize: "1.5em" }, 400);
    }

</script>

<div id="body">
    <h2>CreateGMEventYearly</h2>

    <table>
        <tr>
           <th>
             </th>
          <%--  <th>
                Title
            </th>--%>
            <th>
                Date
            </th>
           <th>
                From
            </th>
            <th>
                To
            </th>
            <th></th>
        </tr>

    <% int i = 1; 
        foreach (var item in Model) { %>
    
        <tr>
            <td>
               <%: (i++).ToString() %>
            </td>
          <%--  <td>
                <%: item.Title %>
            </td>--%>
             <td>
                <%: String.Format("{0:ddd, d MMM yyyy}", item.StartDateTime)%>
            </td>
             <td>
                <%: String.Format("{0:HH:mm}", item.StartDateTime)%>
            </td>
            <td>
                <%: String.Format("{0:HH:mm}", item.EndDateTime)%>
            </td>
            <td>
                 <%: Html.ActionLink("Remove", "Delete", "Event", new { id = item.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>
                <%-- <%: Ajax.ActionLink("Delete", "Deleted", "GroupMeditation", new {eventID = item.ID }, new AjaxOptions { UpdateTargetId = "rsvpmsg", OnSuccess = "AnimateRSVPMessage" })%>         --%>
             </td>

        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</div>
</asp:Content>



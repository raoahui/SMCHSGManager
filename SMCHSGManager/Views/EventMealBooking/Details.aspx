<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<string>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="body">

    
   <h5><%: ViewData["LocalRetreatTitle"]%>  Booking Person Table</h5>
    <h6><%: ViewData["MealName"] %> </h6>
    <table>
        <tr>
            <th></th>
            <th>
               Name
             </th>
        </tr>

    <%
        int i = 0;
       foreach (var item in Model) { %>
    
        <tr>
            <td><%: (++i).ToString()%></td>
            
             <td>
                <%: item %>
            </td>
          </tr>
    
    <% } %>

 
    </table>

   <div align="left" style="margin-left:30px; margin-bottom:10px; margin-top:10px">
         <%: Html.ActionLink("Back", "Table", new { localRetreatID = ViewData["LocalRetreatID"]})%>
   </div>


</div>

</asp:Content>

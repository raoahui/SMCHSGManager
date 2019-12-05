<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.EventMealBookingViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Table
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="body">

   
   <h5><%: ViewData["localRetreatName"] %> Local Rertreat Meal Booking Table</h5>

    <table>
        <tr>
            <th></th>
            <th>
                Meal Time
            </th>
            <th>
                Unit Prce
            </th>
            <th nowrap=nowrap>
                Number of meals (packets)
            </th>
            <th>
                Price
            </th>
        </tr>

    <% int i = 1;
      decimal totalPrice = 0;  
        foreach (var item in Model) { %>
    
        <tr>
            <td>
            <% if (i < Model.Count())
               { %>
                <%: Html.ActionLink((i++).ToString(), "Details", new { localRetreatScheduleID = item.EventScheduleID })%> 
                <%} %>
            </td>
           <td>
                <%: item.MealNameDate %>
            </td>
             <td>
                <%: String.Format("SGD{0:C}", item.UnitPrce) %>
            </td>
            <td>
                <%: item.Count %>
            </td>
            <td>
                    <%: String.Format("SGD{0:C}", item.UnitPrce * item.Count)%>
            </td>
        </tr>
    
    <%
        totalPrice += (decimal)item.UnitPrce * item.Count;
        } %>
           
        <tr ><td  colspan = 6 align="right" bgcolor="#2A4013" ><font color=white size=3>Total Collection : 
                  <%: String.Format("SGD{0:C}", totalPrice)%></font>
                </td>
        </tr>
    </table>

   <div align="center" style="margin-bottom:20px; margin-top:20px">
             <%: Html.ActionLink("Back", "Details", "Event", new { id = (int)ViewData["LocalRetreatID"] }, new { @style = "color:white;", @class = "buttonsmall" })%> 
   </div>

</div>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.EventSchedule>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="body">
 <h5>Local Retreat Schedule List</h5>
 
     <% 
         if (Model != null)
         { %>
    <table>
        <tr>
            <th></th>
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
<%--            <th>
                Remark
            </th>--%>
        </tr>

    <% int i=0;
       foreach (var item in Model)
       {
           if (!item.EventActivity.Name.StartsWith("Bless"))
           {%>   
        <tr>
            <td>
               <%-- <%: Html.ActionLink("Edit", "Edit", new { id = item.ID })%>--%>
 <%--               <%if (++i == Model.Count())
                  {%>
              |   <%: Html.ActionLink("Delete", "Delete", new { id = item.ID })%>
                <%} %>--%>
            </td>
             <td><%: (++i).ToString()%></td>
            <td>
			
                <%: item.EventActivity.Name%>
				<% if (i == 2)
	   { %> (door open)
				<%} %>
            </td>
            <td>
                <%= Html.DisplayFor(EventScheduleMetaDate => item.DateTimeFrom)%>
           </td>
            <td>
            <% DateTime DateTimeTo = item.DateTimeFrom.AddHours(item.ScheduleOffset.OffsetHours); %>
                <%= Html.Encode(String.Format("{0:d MMM yyyy HH:mm}", DateTimeTo))%> 
            </td>
<%--            <td>
                <%: item.Remark %>
            </td>--%>
        </tr>
    <% }
       }%>

    </table>
    <%
        }
         else
         {%>
             <div align="center" style=" margin-bottom:20px; margin-top:15px; color:Green; font-size:14px">
             <%:  (string)ViewData["Message"]%>
             </div>
         <%} %>
      
    <div align="center" style="margin-bottom:20px; margin-top:20px">
          <a href="javascript:history.go(-1)" style = "color:white;" class="buttonsmall" >Back</a>
    </div>

   
</div>
</asp:Content>


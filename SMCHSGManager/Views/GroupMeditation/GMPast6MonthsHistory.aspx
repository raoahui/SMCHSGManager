<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.GMAttendanceCountViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	GroupMeditationPast6MonthsHistory
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
  <div align="center" style=" margin-top:20px; color:#2A4013; font-size:1.6em; font-family:Verdana; font-weight:bold">
        Past 6 Months Group Meditation and Local Retreat History
   </div>
   <br />

  <% using (Html.BeginForm()) {%>

  		 <div align="center" >
              Name/MemberNo:  <%: Html.TextBox("searchContent")%>  
               <input type="submit" value="Search" class ="buttonsearch" name="Search" />
		  </div>

     <table>
        <tr>
			<th>No</th>
            <th>
                Member No
            </th>
            <th>
                Name
            </th>

 			<% foreach(var gmCount in Model.FirstOrDefault().GMAttendanceMonthCounts){%>
		    <th><%: String.Format("{0:d MMM yyyy}", gmCount.ToDate) %> ~ </br><%: String.Format("{0:d MMM yyyy}", gmCount.FromDate) %></th>
			<%} %>

           <th>
                Meet Require
            </th>
            <th>
                Total Count
            </th>
        </tr>

    <% int i = 0;
				foreach (var item in Model) { %>
    
        <tr>
			<td><%: (++i).ToString() %></td>
           <td>
                <%: item.MemberInfo.MemberNo %>
            </td>
            <td nowrap = "nowrap">
                <%: item.MemberInfo.Name %>
            </td>
 			<% foreach(var gmCount in item.GMAttendanceMonthCounts){%>
				<td><%: gmCount.Count.ToString() %></td>
			<%} %>
            <td><% if (item.MeetRequire)
				   { %>
				Yes
				<%}
				   else
				   { %>
				   No
				   <%} %>
   
            </td>
            <td>
                <%: item.TotalCount %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <% } %>

   <%-- <div align="center"  style="margin-top:10px" > 
           <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("Index", new { page = p, searchContent = ViewData["searchContent"]}))%>
     </div>--%>

 
</div>

</asp:Content>


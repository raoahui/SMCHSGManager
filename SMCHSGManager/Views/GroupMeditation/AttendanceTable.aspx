<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.GMAttendanceViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AttendanceTable
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
  <%-- <h5>Attendance Table</h5>--%>
  	
				<%= Html.ActionLink("Get Excel", "GenerateExcel2", "GroupMeditation")%>  

<%--  <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>--%>
		
    <table>
        <tr>
          <%--  <th>No</th>--%>
            <th rowspan = 3>
                No.
            </th>
            <th rowspan = 3>
                Member Name
            </th>
			<th rowspan = 3>
				Expiry Month Year
			</th>

			  <% int count = Model.nextMonthGMs.Count;
                   if(count > 0)
				   {%>
						<th colspan=<%: count.ToString()%> >
							<% if (Model.HavePreviousMonth){ %>
								<a href='<%: Url.Action("AttendanceTable", "GroupMeditation",  new { nextMonth = Model.NextMonth - 1}) %>' > 
								<img src="../images/previous.gif" alt=""  width="16" height="18" title="previous month" /></a> 
							<%} %>

							 <%: Model.NextMonthStr.Replace(", ", " ") %>
				
							<% if(Model.HaveNextMonth){ %>
							<a href='<%: Url.Action("AttendanceTable", "GroupMeditation",  new { nextMonth = Model.NextMonth + 1}) %>' > 
							<img src="../images/next.gif" alt=""  width="16" height="18" title="next month" /></a> 
							<%} %>
						</th>
				<%} %>

			     <th rowspan = 3>S/N</th>
        </tr>

		<tr>
			<% foreach (var gm in Model.nextMonthGMs){%>
			<th nowrap = "nowrap">
	
				<%: gm.StartDateTime.DayOfWeek.ToString().Substring(0,3) %> 
				<% if(gm.StartDateTime.Hour == 0){ %>
					</br>(0-<%: gm.EndDateTime.Hour.ToString() %>)
				<%}%>
			</th>
			<%} %>
		</tr>

		<tr>
			<% foreach (var gm in Model.nextMonthGMs){%>
			<th><%: gm.StartDateTime.Day.ToString() %></th>
			<%} %>
		</tr>

    <% int i = 0; 
        foreach (var item in Model.MemberFeeExpiredDates) { %>
    
        <tr>
          <%--  <td><%: (++i).ToString()%></td>--%>
           <td>
                <% if (item.MemberNo.HasValue)
                   { %>
                    <%: item.MemberNo.Value%>
                 <% }%>
            </td>
             <td>
               <b>  <%: item.Name %>		</b>
            </td>
		
            <td>
			<% if (item.MemberFeeExpiredDate.HasValue){ %>
			    <% if (item.MemberFeeExpiredDate.Value == SMCHSGManager.Models.MemberFeePayment.ToDateGiro){ %>
			         Giro 
			    <%}else{ %>
				    <%: string.Format("{0: MMM-yy}", item.MemberFeeExpiredDate)%>
			    <%}
            } %>
            </td>
		
			<% int j=0;
				foreach (var gm in Model.nextMonthGMs){%>
				<td style="font-weight:bold; color:Green">
					<% if (Model.AttendenceChecks[i, j++]){ %>
						Y
					<%} %>
				</td>
			<%} %>

			<td><%: (++i).ToString()%></td>
        </tr>
    
    <% } %>

    </table>
        
	<%--	<input type="submit"  value="Save"  name="Save" class ="buttonsmall" /> &nbsp;--%>
  <%--  <%} %>--%>
</div>

</asp:Content>

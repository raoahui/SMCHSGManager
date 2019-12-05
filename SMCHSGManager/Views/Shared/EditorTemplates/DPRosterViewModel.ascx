<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.ViewModel.DPRosterViewModel>" %>

    <% using (Html.BeginForm())
	   {%>

	  <h5> DP Roster<% if (Model.HavePreviousMonth)
		  { %>
              <a href='<%: Url.Action("DPRoster", "GMVolunteerJobName",  new { nextMonth = Model.NextMonth - 1, edit = Model.Edit }) %>' > 
              <img src="../images/previous.gif" alt=""  width="16" height="18" title="previous month" /></a> 
		<%} %>
	 <%: (Model.NextMonthStr).Replace('^', ' ') %>
			<% if(Model.HaveNextMonth){ %>
              <a href='<%: Url.Action("DPRoster", "GMVolunteerJobName",  new { nextMonth = Model.NextMonth + 1, edit = Model.Edit }) %>' > 
              <img src="../images/next.gif" alt=""  width="16" height="18" title="next month" /></a> 
			<%} %>
	  </h5>
	
	<table style="padding:0">
        <tr>
            <th>
                Sunday
            </th>
            <th>
                Monday
            </th>
            <th>
                Tuesday
            </th>
 			<% if ((DateTime)ViewData["FirstDayOfMonth"] >= new DateTime(2012, 5, 1) ){ %>
             <th>
                Wednesday Overnight
            </th>
			<%} %>
             <th>
                Thursday
            </th>
          <th>
                Friday (conv.)
            </th>
 			<% if ((DateTime)ViewData["FirstDayOfMonth"] >= new DateTime(2015, 3, 1) ){ %>
            <th>
                Saturday Morning
            </th>
			<%} %>
            <th>
                Saturday Evening
            </th>
            <th>
                Saturday Overnight
            </th>
          </tr>

    <% string[,] monthDpList = Model.MonthDpList; // (string[,])ViewData["monthDpList"];
	   List<List<SMCHSGManager.Models.MemberInfo>> WeekNoDPLists = Model.WeekNoDPLists;   //(List<List<SMCHSGManager.Models.MemberInfo>>)ViewData["WeekNoDPLists"];
	   for (int i = 0; i < monthDpList.GetLength(0); i++)
	   { 
		   //if(i>0 && string.IsNullOrEmpty(monthDpList[i, 0]))
		   //{
		   //    continue; 
		   //}else
		   {%>
		    <tr>
					<% for (int j = 0; j < monthDpList.GetLength(1); j++)
					{
							string[] days = new string[3];
							if (!string.IsNullOrEmpty(monthDpList[i, j]))
							{
								days = monthDpList[i, j].Split('^');
							} %>
					
					<td>
						 <table style=" border:0; padding:0 ">
							<tr>
								<% if (!string.IsNullOrEmpty(days[0]))
								 { %>
										<td align="center" style="background-color: #CFCFCF;  border:0; padding:3px,2px; ">
											<%: days[0]%>
										<%} else { %>
										<td align="center" style="border:0; padding:3px,2px; "> 
										<%} %>
								</td>
							</tr>
							<tr>
								<td align="center" style=" border:0;  padding: 3px,2px;"> 
								<% if (!string.IsNullOrEmpty(days[1]))
										{ %>
										<%: days[1]%>
										<%} %>
								</td>
							</tr>
							<tr>
									<% if (!string.IsNullOrEmpty(days[2]))
										 {
											 if (Model.Edit)
											 { %>
												<td align="center" style=" border:0; padding:3px,2px;">     
												<%: Html.DropDownList("IMemberID", new SelectList(WeekNoDPLists[j] as IEnumerable, "MemberID", "Name", Guid.Parse(days[2])))%> 
											<%}else{%>
												<td align="center" style=" border:0; padding:3px,2px; font-weight:bold; font-family:Tahoma">   
                                                  
                                                <%: days[2] %>											<%}
										} %>
								</td>
							</tr>
						</table>
					</td>
				<%}%>
		    </tr>
		<%} %>
	<%} %>


 </table>

	  <% if (Roles.IsUserInRole("Administrator"))
          { %>
            <div align="center" style="margin-bottom:20px; margin-top:20px">
               <%: Html.ActionLink("Back", "DPRoster", new { nextMonth = Model.NextMonth, edit = false }, new { @style = "color:white;", @class = "buttonsmall" })%> 
 				<% if (Model.Edit)
				{ %>
					&nbsp;&nbsp;&nbsp;&nbsp; <input type="submit" value="Save" class ="buttonsmall"/>
			   <%}
				else
				{ %>
                   <%: Html.ActionLink("Edit", "DPRoster", new { nextMonth = Model.NextMonth, edit = true }, new { @style = "color:white;", @class = "buttonsmall" })%> 
				<%} %>
            </div>
		<%} %>

 <%} %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.InternationalGMApplicationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div class="fullwidth">         

	<h2>Application International GM Details</h2>
   
   <% List<SMCHSGManager.Models.InternationalGMApplicationStatu> applicationStatus = 
										(List<SMCHSGManager.Models.InternationalGMApplicationStatu>)ViewData["ApplicationStatus"];
	%>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

            <table style="border:0" >
  
                       <tr><td colspan=4 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

                        <tr>
                            <td class="formlabel">Member Name: </td>
                            <td align="left" class="formvalue3" >
								<%: Model.InternationalGMApplicationInfo.MemberInfo.Name %>
							</td>
                            <td class="formlabel">Ashram Name: </td>
                            <td align="left" class="formvalue3" >
								<%: Model.InternationalGMApplicationInfo.AshramAndCenterInfo.Name %>
							</td>
                        </tr>

                        <tr>
                            <td class="formlabel"> Arrival Date</td>
                            <td align="left" class="formvalue3">
									<%: String.Format("{0:ddd, d MMM yyyy}", Model.InternationalGMApplicationInfo.ArrivalDate)%>  &nbsp;
	                        </td>
                             <td class="formlabel"> Departure Date</td>
                            <td align="left" class="formvalue3">
									<%: String.Format("{0:ddd, d MMM yyyy}", Model.InternationalGMApplicationInfo.DepartureDate)%>  &nbsp;
                            </td>
                        </tr>

						<tr>
                            <td class="formlabel"> ApplyDate</td>
                            <td align="left"class="formvalue3">
							        <%: String.Format("{0:ddd, d MMM yyyy HH:mm}", Model.InternationalGMApplicationInfo.ApplyDate)%>  &nbsp;
                           </td>
    					<% if (Model.InternationalGMApplicationInfo.AccommodationNeeded.HasValue){ %>
                            <td class="formlabel"> AccomodationNeeded </td>
                            <td align="left" class="formvalue3">
									 <% if (Model.InternationalGMApplicationInfo.AccommodationNeeded.Value) { %> 
										Yes
									<%}else{ %>
										No
									<%} %>
                               </td>
 						<%} %>
                       </tr>

					   <tr>
                            <td class="formlabel">Remark: </td>
                            <td align="left" colspan="3" class="formvalue3" >
								<%: Model.InternationalGMApplicationInfo.Remark%>
							</td>
					   </tr>

						<% if (Model.HsihuAshramEvent.TwoDaysRetreats != null && Model.HsihuAshramEvent.TwoDaysRetreats.Count > 0){%>                       
 						<tr>
                            <td class="formlabel" nowrap="nowrap" style="width: 279px" >Apply 2 Days Retreat </td>
                            <td align="left" colspan=3 class="formvalue3" >
								<% for (int i = 0; i < Model.HsihuAshramEvent.TwoDaysRetreats.Count; i++ ){ %>
 									 <%: string.Format("{0: d MMM yyyy}", Model.HsihuAshramEvent.TwoDaysRetreats[i])%> to <%: string.Format("{0: d MMM yyyy}", Model.HsihuAshramEvent.TwoDaysRetreats[i].AddDays(1))%> &nbsp;&nbsp;&nbsp;
								<%} %>							
							</td>
                        </tr>
						<%} %>

             			<% if (Model.HsihuAshramEvent.SundayGMs != null && Model.HsihuAshramEvent.SundayGMs.Count > 0){%>         
  						<tr>
                            <td class="formlabel" nowrap="nowrap" style="width: 279px" >Apply Sunday Group Meditation </td>
                            <td align="left" colspan=3 class="formvalue3" >
								<%  for (int i = 0; i < Model.HsihuAshramEvent.SundayGMs.Count; i++ ){ %>
 									<%: string.Format("{0: d MMM yyyy}", Model.HsihuAshramEvent.SundayGMs[i])%> &nbsp;&nbsp;&nbsp;
								<%} %>
							</td>
                        </tr>
						<%} %>

	                   <tr><td colspan=4 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

						<tr>
							<td class="formlabel" nowrap=nowrap> Application Status</td>
							<td align="left" colspan=3 class="formvalue3" >
							<% foreach (SMCHSGManager.Models.InternationalGMApplicationStatu ias in applicationStatus)
								{%>
										<%: ias.ApplicationStatus.Name %> &nbsp; <%:  String.Format("{0:ddd, d MMM yyyy HH:mm}", ias.ConfirmDate)+", "  %>  
								<%}%>
							</td>
						</tr>

	                   <tr><td colspan=4 class="formlabel" style="height: 85px">
                            <div class="dashedline"></div>
                       </td></tr>

				   <% if(Model.TransportInfos.Count > 0){ 
							foreach(var transportInfo in Model.TransportInfos){%>
 							<tr>
								<td class="formlabel" nowrap=nowrap> 
									<% if (transportInfo.InBound){ %>
										InBound
									<%}else { %>
										OutBound 
									<%} %>		
										Station 
								</td>
								<td align="left" class="formvalue3"> <%: transportInfo.InternationalTransport.StationName %> 
									<%if(!string.IsNullOrEmpty(transportInfo.FlightNo)){ %>
										(FlightNo: <%: transportInfo.FlightNo %>) 
									<%}%>
								</td>
								<td class="formlabel" nowrap=nowrap> DateTime</td>
								<td align="left" class="formvalue3">  <%: String.Format("{0:ddd, d MMM yyyy HH:mm}", transportInfo.DateTime) %> </td>
							</tr>
					 <%}%>
 
					   <tr><td colspan=4 class="formlabel">
                          <div class="dashedline"></div>
                       </td></tr>

				   <%} %>
	 				   
				   <% if (Model.InternationalGMApplicationInfo.AshramID == 2){ %>
                      <tr>
                            <td class="formlabel"> Note</td>
                            <td align="left" class="formvalue3">
								<%: MvcHtmlString.Create(Model.HsihuAshramEvent.Remark)%>
                            </td>
                      </tr>
				   
				      <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
					  </td></tr>
				   <%}%>

				   <% if(!string.IsNullOrEmpty(Model.InternationalGMApplicationInfo.CPComments) || Roles.IsUserInRole("SuperAdmin") &&  (applicationStatus.LastOrDefault().ApplicationStatusID == 1 || 
									applicationStatus.LastOrDefault().ApplicationStatusID ==4)){ %>
                        <tr>
                            <td class="formlabel"> CP Comments</td>
                            <td align="left" colspan=3 class="formvalue3">
								<% if (Roles.IsUserInRole("SuperAdmin") &&  (applicationStatus.LastOrDefault().ApplicationStatusID == 1 || 
									applicationStatus.LastOrDefault().ApplicationStatusID ==4)){ %>
									<%: Html.TextBoxFor(model=>model.InternationalGMApplicationInfo.CPComments, new { style = "width:100%;" })%>
								<%}else {%>
									<%: Model.InternationalGMApplicationInfo.CPComments %>
								<%}%>
                            </td>
                        </tr>
	                   <tr><td colspan=4 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
 					<%}%>

 	</table>

	<% SMCHSGManager.ViewModel.GMAttendanceCountViewModel gmModel =
		(SMCHSGManager.ViewModel.GMAttendanceCountViewModel)ViewData["GMAttendanceViewModel"]; %>
		<p align="center">
			Total Count: <%: gmModel.TotalCount.ToString() %>
		</p>
		<table> 
			<tr>
				<% foreach(var gmCount in gmModel.GMAttendanceMonthCounts){%>
					<td><%: String.Format("{0:d MMM yyyy}", gmCount.ToDate) %> ~ </br><%: String.Format("{0:d MMM yyyy}", gmCount.FromDate) %></td>
				<%}%>
			</tr>
			<tr>
				<% foreach(var gmCount in gmModel.GMAttendanceMonthCounts){%>
					<td><%: gmCount.Count.ToString() %></td>
				<%}%>
			</tr>
		</table>

  	<%  if (Roles.IsUserInRole("SuperAdmin") && (applicationStatus.LastOrDefault().ApplicationStatusID == 1 ||
									applicationStatus.LastOrDefault().ApplicationStatusID == 4))
	   { %>
		<div align="center">
		   <input type="submit"  value="Reject"  name="reject" class ="buttonsmall" /> &nbsp;
		   <%: Html.ActionLink("Cancel", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
		</div>
	<%}%>
							 <%--DateTime.UtcNow.AddHours(8) < (DateTime)ViewData["SubmitDateTime"])--%>
	<% if (applicationStatus.LastOrDefault().ApplicationStatusID < 4 ) 	{%>
		<div align="center">
			<font color="red">Please visit <%: Model.InternationalGMApplicationInfo.MemberInfo.AshramAndCenterInfo.Name %> after group meditation for a physical screening by CP.</font>
		</div>
		<div class="dashedline"></div>
		<div align="center">
			You can modify or remove your application before submit to <%: Model.InternationalGMApplicationInfo.AshramAndCenterInfo.Name %>.
			<%--You can modify or remove your application by <%: string.Format("{0:ddd, MMM d yyyy HH:mm}", (DateTime)ViewData["SubmitDateTime"])%></br></br>--%>
			<%: Html.ActionLink("Edit", "Edit", new { id = Model.InternationalGMApplicationInfo.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>  &nbsp;
			<% if (applicationStatus.LastOrDefault().ApplicationStatusID == 1 ) 	{%>
                <%: Html.ActionLink("Delete", "Delete", new { id = Model.InternationalGMApplicationInfo.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> </br>
            <%}%>
		</div>
	<%}%>

 <% } %>  

</div>

</asp:Content>


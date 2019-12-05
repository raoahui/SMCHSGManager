<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.ViewModel.HsihuAshramEventModel>" %>

	           <table style="border:0" >
  
                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

					    <% if (ViewData["Mode"] == "Create") { %>
                        <tr>
                            <td class="formlabel" nowrap="nowrap" style="width: 279px" >
								<% if (Model.AshramID == 2){ %>
									Apply Stay & Meditate
								<%}else{ %>
									Accommodation Needed
								<%} %>
							</td>
                            <td align="left" class="formvalue3" >
								<%: Html.CheckBox("AccommodationNeededCheckBox", true)%>
							</td>
                        </tr>
						<%} %>

 <%--<% List<DateTime> retreats = (List<DateTime>)ViewData["retreats"];
   List<DateTime> sundayGMs = (List<DateTime>)ViewData["sundayGMs"]; %>--%>
			 
					<% if (Model.TwoDaysRetreats != null && Model.TwoDaysRetreats.Count > 0){%>                       
                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
						<tr>
                            <td class="formlabel" nowrap="nowrap" style="width: 279px" >Apply 2 Days Retreat </td>
                            <td align="left" class="formvalue3" >
								<% for (int i = 0; i < Model.TwoDaysRetreats.Count; i++ ){
									 if (ViewData["Mode"] == "Create") {%>
 										<%: string.Format("{0: d MMM yyyy}", Model.TwoDaysRetreats[i])%> to <%: string.Format("{0: d MMM yyyy}", Model.TwoDaysRetreats[i].AddDays(1))%> 	<%: Html.CheckBox("RetreatCheckBox" + string.Format("{0:d-MMM-yyyy}", Model.TwoDaysRetreats[i]))%> &nbsp;&nbsp;&nbsp;
									<%}else{ %>
 										<%: string.Format("{0: d MMM yyyy}", Model.TwoDaysRetreats[i])%> to <%: string.Format("{0: d MMM yyyy}", Model.TwoDaysRetreats[i].AddDays(1))%> 	<%: Html.CheckBox("RetreatCheckBox" + string.Format("{0:d-MMM-yyyy}", Model.TwoDaysRetreats[i]), Model.TwoDaysRetreatCheckeds[i])%> &nbsp;&nbsp;&nbsp;
									 <%} %>	
								<%} %>							
							</td>
                        </tr>
					<%} %>

             	 <% if (Model.SundayGMs != null && Model.SundayGMs.Count > 0){%>         
                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
						<tr>
                            <td class="formlabel" nowrap="nowrap" style="width: 279px" >Apply Sunday Group Meditation </td>
                            <td align="left" class="formvalue3" >
								<%  for (int i = 0; i < Model.SundayGMs.Count; i++ ){ 
									 if (ViewData["Mode"] == "Create") {%>
 										<%: string.Format("{0: d MMM yyyy}", Model.SundayGMs[i])%> 	<%: Html.CheckBox("SundayGMCheckBox" + string.Format("{0:d MMM yyyy}", Model.SundayGMs[i]))%>  &nbsp;&nbsp;&nbsp;
									<%}else{ %>
 										<%: string.Format("{0: d MMM yyyy}", Model.SundayGMs[i])%> 	<%: Html.CheckBox("SundayGMCheckBox" + string.Format("{0:d MMM yyyy}", Model.SundayGMs[i]), Model.SundayGMCheckeds[i])%>  &nbsp;&nbsp;&nbsp;
								    <%} %>	
								<%} %>
							</td>
                        </tr>
				<%} %>

					<tr><td colspan=2 class="formlabel">
                           <div class="dashedline"></div>
                   </td></tr>
	
				   <% if (Model.Remark != null){ %>
                   <tr>
                       <td class="formlabel"> Note</td>
                       <td align="left" class="formvalue3">
                                	<%: MvcHtmlString.Create(Model.Remark)%> 
                       </td>
                   </tr>
				   <%} %>

					<tr><td colspan=2 class="formlabel">
                           <div class="dashedline"></div>
                   </td></tr>

	</table>
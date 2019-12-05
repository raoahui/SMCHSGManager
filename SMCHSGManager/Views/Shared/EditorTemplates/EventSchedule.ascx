<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.EventSchedule>" %>

<%--<script src="/Scripts/MicrosoftAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcValidation.js" type="text/javascript"></script>
--%>            
  

			<table  align="left"   border="1" width = 100%  style ="border-color:Lime; background-color: #FED36B; border-bottom-style:groove">
					<tr style="height: 28px" >
						<td style="width: 200px" ><font color="red" size="2">*</font>Local Retreat Activity</td>
						<td >
							<%: Html.DropDownList("EventActivityID", new SelectList(ViewData["EventActivity"] as IEnumerable, "ID", "Name", Model.EventActivityID))%>     
                        </td>
					</tr>

					<tr>
						<td style="width: 200px" >From</td>
						<td > <%: Html.DisplayFor(model => model.DateTimeFrom)%> </td>
					</tr>

                    <%if (ViewData["Mode"] == "create")
                      { %>
                    <tr>
						<td style="width: 200px" ><font color="red" size="2">*</font>Duration (hours)</td>
						<td ><%: Html.DropDownList("ScheduleOffsetID", new SelectList(ViewData["ScheduleOffsets"] as IEnumerable, "ID", "OffsetHours", Model.ScheduleOffsetID))%>  </td>
					</tr>
                    <%}
                      else
                      {%>

                    <tr>
						<td style="width: 200px" >To</td>
                                    <% DateTime DateTimeTo = Model.DateTimeFrom.AddHours(Model.ScheduleOffset.OffsetHours); %>
                        <td ><%= Html.Encode(String.Format("{0:g}", DateTimeTo))%> </td>
					</tr>
                    <%} %>

			</table>


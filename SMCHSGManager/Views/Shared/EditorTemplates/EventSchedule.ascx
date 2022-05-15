<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.EventSchedule>" %>

 <head id="Head1" runat="server">
  <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.debug.js") %>" type="text/javascript"></script>
  <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.debug.js") %>" type="text/javascript"></script>
  <script src="<%= Url.Content("~/Scripts/MicrosoftMvcValidation.debug.js") %>" type="text/javascript"></script>
  <% Html.EnableClientValidation();%>
</head> 

<div class="fullwidth">         
 
 
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

			<table  align="left"   border="1" width = 100%  style ="border-color:Lime; background-color: #FED36B; border-bottom-style:groove">
					<tr style="height: 28px" >
						<td style="width: 200px" ><font color="red" size="2">*</font>Local Retreat Activity</td>
						<td >
                         <%if (ViewData["EventActivity"] != null) {%>
							<%: Html.DropDownList("EventActivityID", new SelectList(ViewData["EventActivity"] as IEnumerable, "ID", "Name", Model.EventActivityID))%>     
                            <%: Html.ValidationMessageFor(model => model.EventActivityID)%>
                         <%}
                      else
                      {%>
                         <%: Html.DisplayFor(model => model.EventActivity.Name)%>
                         <%} %>
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
                        <%if (ViewData["ScheduleOffsets"] != null) {%>
						<td ><%: Html.DropDownList("ScheduleOffsetID", new SelectList(ViewData["ScheduleOffsets"] as IEnumerable, "ID", "OffsetHours", Model.ScheduleOffsetID))%>  </td>
                         <%}
                      else
                      {%>
                        <% DateTime DateTimeTo = Model.DateTimeFrom.AddHours(Model.ScheduleOffset.OffsetHours); %>
                       <td ><%= Html.Encode(String.Format("{0:d MMM yyyy HH:mm}", DateTimeTo))%> </td>
                         <%} %>
					</tr>
                    <%} %>

                     <tr>
                            <td  colspan=2 class="formlabel">
                             <div class="actionbuttons">
                                <% if (ViewData["Mode"] == "Create")
                                { %>
                                 <input type="submit"  value="Create & next" name="Create" class ="buttonsmall" /> &nbsp;
                                 <%}else{ %>
                                  <input type="submit"  value="Save & next"  name="Save" class ="buttonsmall" /> &nbsp;
                                  <%} %>
                                 <%: Html.ActionLink("Back to List", "Index", new { localRetreatID = (int)ViewData["localRetreatID"] }, new { @style = "color:white; ", @class = "buttonsmall" })%>
                               </div>
                       </td></tr>

			</table>

    <% } %>
</div>

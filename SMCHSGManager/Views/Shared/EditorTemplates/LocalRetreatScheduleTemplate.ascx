<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.LocalRetreatScheduleTemplate>" %>

  <h5>Local Retreat Schedule </h5>

 <div class="fullwidth">         
     <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm((string)ViewData["Mode"], "LocalRetreatScheduleTemplate", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
         <%: Html.ValidationSummary(true) %>
  			<table>

                     <% if (ViewData["Mode"] == "Create" )
                        {%>
 					<tr  style="visibility:hidden">
						<td ><font color="red" size="2">*</font>Local Retreat Model</td>
                        <td ><%: Html.TextBoxFor(model => model.Model, "ModelID")%></td>
					</tr>
                    <%} %>

					<tr  >
						<td ><font color="red" size="2">*</font>Local Retreat Activity</td>
						<td ><%: Html.DropDownList("EventActivityID", new SelectList(ViewData["EventActivities"] as IEnumerable, "ID", "Name", Model.EventActivityID))%> </td>
					</tr>

                    <tr>
						<td><font color="red" size="2">*</font>Duration (hours)</td>
						<td ><%: Html.DropDownList("ScheduleOffsetID", new SelectList(ViewData["ScheduleOffsets"] as IEnumerable, "ID", "OffsetHours", Model.ScheduleOffsetID))%>  </td>
					</tr>
 
 					<tr>
						<td >DP PersonNeeded</td>
						<td ><%: Html.TextBoxFor(model => model.DP_PersonNeeded)%></td>
 					</tr>

 					<tr>
						<td >Video PersonNeeded</td>
                        <td ><%: Html.TextBoxFor(model => model.Video_PersonNeeded)%> </td>
					</tr>

					<tr>
						<td >Clean PersonNeeded</td>
                        <td ><%: Html.TextBoxFor(model => model.Clean_PersonNeeded)%> </td>
 					</tr>

			</table>
             
                              <div align="center" class="actionbuttons" >
                                <% if (ViewData["Mode"] == "Create")
                                {%>
                                 <input type="submit"  value="Create" name="Create" class ="buttonsmall"  /> &nbsp;
                                 <%}else{ %>
                                  <input type="submit"  value="Save"  name="Save" class ="buttonsmall" /> &nbsp;
                                  <%} %>
                                 <%: Html.ActionLink("Back to index", "Index", new { modelID = Model.Model }, new { @style = "color:white; ", @class = "buttonsmall" })%>
                               </div>
       <% } %>
</div>           
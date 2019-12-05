<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.LocalRetreatScheduleTemplate>" %>

  			<table  align="left"   border="1" width = 100%  style ="border-color:Lime; background-color: #FED36B; border-bottom-style:groove">
					<tr style="height: 28px" >
						<td style="width: 200px" ><font color="red" size="2">*</font>Local Retreat Activity</td>
						<td ><%: 
						         Html.DropDownList("EventActivityID", new SelectList(ViewData["EventActivities"] as IEnumerable, "ID", "Name", Model.EventActivityID))%>     
                        </td>
					</tr>

                    <tr>
						<td style="width: 200px" ><font color="red" size="2">*</font>Duration (hours)</td>
						<td ><%: Html.DropDownList("ScheduleOffsetID", new SelectList(ViewData["ScheduleOffsets"] as IEnumerable, "ID", "OffsetHours", Model.ScheduleOffsetID))%>  </td>
					</tr>
 
                     <%
                               List<SelectListItem> si = new List<SelectListItem>();
                              for (int i = 0; i <= 6; i++)
                              {
                                  SelectListItem item = new SelectListItem { Text = (i).ToString(), Value = i.ToString() };
                                  if (Model.DP_PersonNeeded == i)
                                  {
                                      item.Selected = true;
                                  }
                                  si.Add(item);
                              }
                    %>
					<tr>
						<td style="width: 200px" >DP PersonNeeded</td>
                        <td><%: Html.DropDownList("VolunteerJobChecks", si)%></td>
					</tr>

                    <%
                              si = new List<SelectListItem>();
                              for (int i = 0; i <= 6; i++)
                              {
                                  SelectListItem item = new SelectListItem { Text = (i).ToString(), Value = i.ToString() };
                                  if (Model.DP_PersonNeeded == i)
                                  {
                                      item.Selected = true;
                                  }
                                  si.Add(item);
                              }
                    %>
					<tr>
						<td style="width: 200px" >Video PersonNeeded</td>
                        <td><%: Html.DropDownList("VolunteerJobChecks", si)%></td>
					</tr>

                   <%
                              si = new List<SelectListItem>();
                              for (int i = 0; i <= 6; i++)
                              {
                                  SelectListItem item = new SelectListItem { Text = (i).ToString(), Value = i.ToString() };
                                  if (Model.DP_PersonNeeded == i)
                                  {
                                      item.Selected = true;
                                  }
                                  si.Add(item);
                              }
                    %>
					<tr>
						<td style="width: 200px" >Clean PersonNeeded</td>
                        <td><%: Html.DropDownList("VolunteerJobChecks", si)%></td>
					</tr>

                  <%
                              si = new List<SelectListItem>();
                              for (int i = 0; i <= 6; i++)
                              {
                                  SelectListItem item = new SelectListItem { Text = (i).ToString(), Value = i.ToString() };
                                  if (Model.DP_PersonNeeded == i)
                                  {
                                      item.Selected = true;
                                  }
                                  si.Add(item);
                              }
                    %>
					<tr>
						<td style="width: 200px" >FoodPrepared PersonNeeded</td>
                        <td><%: Html.DropDownList("VolunteerJobChecks", si)%></td>
					</tr>

			</table>
             
              
        
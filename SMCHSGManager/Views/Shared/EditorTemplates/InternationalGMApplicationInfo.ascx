<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.InternationalGMApplicationInfo>" %>

<div class="fullwidth">         
   
                      <table style="border:0" >
                         <tr><td colspan=4 class="formlabel">
                             <%if (ViewData["Mode"] == "Create") { %>
									<h1>New
                                 <%} else{%>
									<h1>Edit
                                <%} %>
								 International GM Application </h1>
                       </td></tr>

                       <tr><td colspan=4 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

                        <tr>
                            <td class="formlabel" nowrap = "nowrap">
							     <%if (ViewData["Mode"] == "Create") {%>
									 <font color="red" size="2">*</font> Select Ashram:
								 <%}else{ %>
									Ashram
								 <%} %>
                            </td>
                            <td align="left" class="formvalue3" >
							    <%if (ViewData["Mode"] == "Create") {%>
									 <%: Html.DropDownList("AshramID", new SelectList(ViewData["AshramAndCenterInfos"] as IEnumerable, "ID", "Name"))%>  
								 <%}else{ %>
									<%: Model.AshramAndCenterInfo.Name%>
								 <%} %>
							</td>
   					<% if (ViewData["Mode"] == "Edit" && Model.AshramAndCenterInfo.AccommodationPermit && !Model.AshramAndCenterInfo.Name.EndsWith("Ashram"))
		   { %>
                            <td class="formlabel" nowrap = "nowrap"> Accommodation Needed </td>
                            <td align="left" class="formvalue3">
		                              <%: Html.CheckBoxFor(model => model.AccommodationNeeded.Value)%>    
	                          </td>
                        </tr>
						<%} %>

						<tr>
                            <td class="formlabel" nowrap = "nowrap"> <font color="red" size="2">*</font> Arrival Date</td>
                            <td align="left" class="formvalue3">
                                    <%: Html.EditorFor(model => model.ArrivalDate, "Date")%>    
                                    <%= Html.ValidationMessageFor(model => model.ArrivalDate)%>
                            </td>
                            <td class="formlabel" nowrap = "nowrap"> <font color="red" size="2">*</font> Departure Date</td>
                            <td align="left" class="formvalue3">
                                    <%: Html.EditorFor(model => model.DepartureDate, "Date")%>    
                                    <%= Html.ValidationMessageFor(model => model.DepartureDate)%>
									
                            </td>
                        </tr>

	
                     <tr><td colspan=4 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

						<% if (ViewData["Mode"] == "Create" && (!(bool)ViewData["MeetRequire"] || (bool)ViewData["65AgeAbove"])) {%>
						<tr>
							<td class="formlabel" colspan=4> <font color="red" size="2">

							<% if (!(bool)ViewData["MeetRequire"]) { %>
								Our database shows that you do not fulfill requirement #5. Please provide explanations in here.
							<%}%>

							<% if ((bool)ViewData["65AgeAbove"]){ %>
								You are over 65 years old. Please provide reason(s) for appeal in here.
							 <%} %>

							  </font>
							</td>
						</tr>
                        <%} %>

                         <tr>
                            <td class="formlabel"> Remark</td>
                            <td align="left" colspan=4 class="formvalue3">
									<%: Html.TextBoxFor(model => model.Remark, new { style = "width:100%;" })%>   
                                    <%= Html.ValidationMessageFor(model => model.Remark)%>
                           </td>
                        </tr>
						
    		   
                    </table>


</div>



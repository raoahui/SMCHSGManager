<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.ViewModel.EventRegistrationViewModel>" %>

  
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
         
            <div class="dashedline"></div>
            
            <% if(!string.IsNullOrEmpty((string)ViewData["errorMsg"])) { %>
            <p style="color:Red; text-align:center"><%: ViewData["errorMsg"]  %></p>
            <%} %>

            <table style="border:0; width:85%"  >
     
                               <%if (Model.MemberInfo != null)
                                 {%>
                             <tr>
                                    <td class="formvalue" colspan="3" align="center">
                                        Please Select Register Name  &nbsp;&nbsp; 
                                            <%: Html.DropDownList("MemberID", new SelectList(ViewData["MemberInfo"] as IEnumerable, "MemberID", "Name", Model.MemberInfo.FirstOrDefault().Name))%> 
                                     </td>
                            </tr>
                           <tr>
                                 <td colspan=3 class="formlabel">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                            </tr>

                            <%}
                                 else
                                 { %>
                                  <div  align="center">
                                  <% if (ViewData["Mode"] == "Create")
                                     { %>
                                  You are about to register for
                                  <%}
                                     else
                                     { %>
                                  You have already registered for
                                  <%} %>
                                  <%: Model.EventRegistration.Event.Title %> 
                                  <% if (Model.EventRegistration.Event.EventTypeID != 5)
                                     { %>
                                    <%: Model.EventRegistration.Event.EventType.Name%>. 
                                    <%} %>
                                   </div>
                                  </br>
                            <%} %>

                            <% if (Model.EventBreakDateTimeValues != null && Model.EventBreakDateTimeValues.Count() > 0)
                              { %>
                             <tr>
                                   <td class="formvalue" colspan="3" align="center">
                                        <% if (Model.EventRegistration.Event.EventTypeID != 5) { %>
                                             <%-- <%= Html.CheckBoxList4("earlyBackDateTimeCheck", Model.EventBreakDateTimeValues, Model.EventBreakDateTimeLabels, null)%>
                                        <%}else{ %>--%>
                                            Select if you need to leave earlier &nbsp;
                                            <%= Html.CheckBoxList4("earlyBackDateTimeCheck", Model.EventBreakDateTimeValues, Model.EventBreakDateTimeLabels, null)%>
                                            <div class="notemsg"><font color="red" size="2">*</font>Note: Participants are allowed to leave only during the breaks and are NOT allowed to come back once left</div>
                                        <%} %>
                                   </td>
                            </tr>
 
                            <tr>
                                <td colspan=3 class="formlabel">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                            </tr>
                            <%} %>

                            <% if (Model.LocalRetreatMealBookingValues != null && Model.LocalRetreatMealBookingValues.Count > 0)
                               { %>
                             <tr>
                                <td colspan=3 class="formlabel" style="text-decoration:underline"  align="center">
                                     <% if (Model.EventRegistration.Event.EventTypeID == 5){ %>
                                        Buffet will be provided
                                    <%}else{ %>
                                        Meal Booking
                                    <%} %>
                                </td>
                            </tr>
                            
                            <% if (Model.EventRegistration.Event.EventTypeID == 5 && Model.EventRegistration.Event.IsPublic)
                               { %>
                            <tr>
                                <td colspan=4 class="formlabel" align="center">
                                     <div class="dashedlineIndex"> </div>
                                     Please fill the numbers of guests number you will bring:
                                         <br /><br /><%= Html.TextBox("GuestNumbersTex", null, new { style="width:50px" }) %>
                                </td>
                            </tr>
                                <%}else{ %>
                            <tr  style=" border:0" align="center">
                                  <%= Html.CheckBoxList1("MealBookingChecks", Model.LocalRetreatMealBookingValues, Model.LocalRetreatMealBookingLabels, null)%>
                            </tr>
                                <%} %>

                            <tr>
                                 <td colspan=3 class="formlabel">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                            </tr>

                            <%} %>

                            <% if (Model.EventVolunteerJobBookingValues != null && Model.EventVolunteerJobBookingValues.Count > 0)
                               { %>
                           <tr>
                                <td colspan=3 class="formlabel" style="text-decoration:underline"  align="center">
                                    Please Select Volunteer Jobs
                                </td>
                            </tr>

                               <tr style=" border:0">
                                <%= Html.CheckBoxList2("VolunteerJobBookingChecks", Model.EventVolunteerJobBookingValues, Model.EventVolunteerJobBookingLabels, null)%>       
                                </tr>

                           <tr>
                                 <td colspan=3 class="formlabel">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                           </tr>
                          <%} %>

                           <tr>
                                <td align="center" style=" border:0" colspan=3 >
                                    <input type="submit" value="Save"  class ="buttonsmall"/>&nbsp; &nbsp;&nbsp; &nbsp
                                    <%-- <a href="javascript:history.go(-1)" style = "color:white;" class="buttonsmall" >Back</a>--%>
                                   <%: Html.ActionLink("Cancel", "Details", "Event", new { id = Model.EventRegistration.EventID }, new { @style = "color:white;", @class = "buttonsmall" })%>

                                <%--     <%: Html.ActionLink("Cancel", "Index", "Home", null, new { @style = "color:white;", @class = "buttonsmall" })%>--%>
                                </td>
                          </tr>
  

             </table>

            <% } %>
         
 



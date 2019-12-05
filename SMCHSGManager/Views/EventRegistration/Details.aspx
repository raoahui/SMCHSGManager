<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.EventRegistrationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
		SMCH Association Singapore - 	Event Registration Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
   <div class="fullwidth">     
   <br />   
   <h2>Your Registration Has Been Successful!</h2>
   <br />   
  <div class="prenextlink">You have registered for this event on <%: Html.DisplayFor(model => model.EventRegistration.RegisterTime)%> </div>

             <div class="dashedlineIndex"></div>

            <table style="border:0; width:70%"  >
                     
                             <tr>
                                    <td class="formlabel">Title:</td>
                                    <td class="formvalue">
                                         <%: Model.EventRegistration.Event.Title %> 
                                    </td>
                            </tr>
                            <tr>
                                    <td class="formlabel">Duration:</td>
                                    <td class="formvalue">
                                        From   <%: Html.DisplayFor(model => model.EventRegistration.Event.StartDateTime)%> To <%: Html.DisplayFor(model => model.EventRegistration.Event.EndDateTime)%>
                                    </td>
                            </tr>

                            <tr>
                                <td colspan=2 class="eventview">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                            </tr>
  
                            <tr>
                                    <td class="formlabel">Name:</td>
                                    <td class="formvalue">
                                         <%: Model.EventRegistration.MemberInfo.Name%>
                                    </td>
                            </tr>

                            <% if (Model.EventRegistration.Event.EventTypeID == 1)
                               { %>
                            <tr>
                                    <td class="formlabel">Date of Initiation:</td>
                                    <td class="formvalue">
                                        <%: Html.DisplayFor(model => model.EventRegistration.MemberInfo.DateOfInitiation, "date")%>
                                    </td>
                            </tr>
                            <tr>
                                    <td class="formlabel">ID Card No:</td>
                                    <td class="formvalue">
                                       <%: Model.EventRegistration.MemberInfo.IDCardNo%>
                                    </td>
                            </tr>
                          
                            <tr>
                                    <td class="formlabel">Leaving Time</td>
                                    <td class="formvalue">
                                        <%: Html.DisplayFor(model => model.EventRegistration.BackDateTime)%>
                                    </td>
                           </tr>
                           <%--<%}else if (Model.EventRegistration.Event.EventTypeID == 5){ %>
                            <tr>
                                    <td class="formlabel">Join AGM</td>
                                    <td class="formvalue">
                                       <% if (Model.EventRegistration.BackDateTime == new DateTime(1900, 1, 1))
                                          { %>
                                        Yes
                                        <%}else{ %>
                                        No
                                        <%} %>
                                    </td>
                           </tr>--%>
                           <%} %>




                           <tr>
                                <td colspan=2 class="eventview">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                           </tr>
                          
                           <% if (Model.EventVolunteerJobBookingLabels.Count() > 0)
                              {%>
                            <tr>
                                <td colspan=2 class="eventview" style="text-decoration:underline">
                                    Volunteer Job Booked 
                                </td>
                            </tr>
                             <%   int i = 0;
                                  foreach (string s in Model.EventVolunteerJobBookingLabels)
                                  {
                                      string s1 = Model.EventVolunteerJobBookingValues.ElementAt(i++); %>
                            <tr>
                                    <td class="formlabel" nowrap="nowrap"><%: s%></td>
                                    <td class="formvalue"  nowrap="nowrap"><%: s1%></td>
                           </tr>
                               <%} %>
  
                          <tr>
                                <td colspan=2 class="eventview">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                           </tr>
                         <%}%>

                    
                         <% if(Model.LocalRetreatMealBookingLabels.Count >= 1)
                            {
                                 if (Model.LocalRetreatMealBookingLabels.Count() > 1) 
                                {%>
                            <tr>
                                <td colspan=2 class="eventview" style="text-decoration:underline">
                                    Meal Booked
                                </td>
                            </tr>
                               <%}%>
  
                            <%  int count = 0;
                                  float tPrice = 0;
                                  int guests = 0;
                                  foreach (string s in Model.LocalRetreatMealBookingLabels)
                                  {
                                      string SGD = "SGD";
                                      int j = s.IndexOf("(") + SGD.Length + 2;
                                      int k = s.IndexOf(")");
                                      float price = float.Parse(s.Substring(j, k - j));
                                      tPrice += price;
                                      string s1 = Model.LocalRetreatMealBookingValues.ElementAt(count++);
                                      if (s == Model.LocalRetreatMealBookingLabels[0].ToString() && count > 1)
                                      {
                                          guests++;
                                      }
                                      if (count == 1 || count > 1 && s != Model.LocalRetreatMealBookingLabels[0].ToString())
                                      {  %>
                            <tr>
                                    <td class="formlabel"><%: s1 + ' ' + s.Substring(0, j - (SGD.Length + 2))%></td>
                                    <td class="formvalue"><%: "SGD$"+ price.ToString()%></td>
                           </tr>
                                  <%}else if (count == Model.LocalRetreatMealBookingLabels.Count && guests > 0)
                                   {%>
                            <tr>
                                    <td class="formlabel" colspan="2"><%: "You will bring " + guests.ToString() + " Guests."%></td>
                           </tr>
                                  <%}
                                }%>
                          
                          <tr>
                                <td colspan=2 class="eventview">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                          </tr>
                         
                          <tr>
                                    <td class="formlabel" style=" font-size:1.2em; color:Black;  text-align:right;  font-weight:bolder">Total Charge is</td>
                                    <td class="formvalue" style="font-size:1.2em; color:Black; font-weight:bolder">SGD$<%: tPrice.ToString() %></td>
                           </tr>

						 <% if (Model.EventRegistration.Event.EventTypeID == 1) { %>
                          <tr>
                                <td colspan=2 class="eventview">
                                    <div class="notemsg"><font color="red">*</font>Note:  </br>1. Please pay your fee on arrival at the retreat. This fee are non-refundable.
                                                                                                    </br>2. Modification or cancellation of your registration is only allowed before <%: Html.DisplayFor(model => model.EventRegistration.Event.RegistrationCloseDate)%>. 
                                                                                                    </br>3.	Participants are to bring their own cups, eating utensils and hygienic wipes
																									</br>4. Participants are required to give valid reasons for being absent. 
																									</br>5. Participants are expected to perform duty during the Retreat when call upon to do so even they did not volunteer.
                                    </div>
                                </td>
                          </tr>
  
                         <tr>
                                <td colspan=2 class="eventview">
                                    <div class="notemsg"><font color="red">*</font>Rules & Regulations  
                                                                                                                </br>1. Always keep your attention on wisdom eye and recite the Five Holy Names
                                                                                                                </br>2. Follow the retreat schedule
                                                                                                                </br>3. Keep in mind that this is a spiritual retreat thus loose and long sleeves clothes are appropriate. Skirts, shorts, vests, 
                                                                                                               </br>  tight clothes, tight pants, low waist pants, bare back dresses, see-through clothes are all FORBIDDEN. Tops must have 
                                                                                                               </br>  sleeves and cover the waist.
                                                                                                                </br>4. Initiates who violate any of these regulations will be asked to leave the retreat
                                      </div>

                                </td>
                          </tr>
						  <%} %>



                           <tr>
                                <td colspan=2 class="eventview">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                          </tr>
                      <%} %>

           </table>
    
                 <div class="editbuttons" align="center">
                                    <%: Html.ActionLink("Back", "Index", "Event", new { pageUpcoming = ViewData["CurrentPageUpcoming"], pageRecent = ViewData["CurrentPageRecent"] }, new { @style = "color:white;", @class = "buttonsmall" })%>
									<%: Html.ActionLink("Cancel Registration", "Delete", new { id = Model.EventRegistration.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
 									<% if (Model.EventRegistration.Event.EventTypeID != 5){ %>
                                        <%: Html.ActionLink("Modify Registration", "Edit", new { id = Model.EventRegistration.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                     <%} %>
                                     <%  if (User.IsInRole("Administrator"))
                                         {%>
                                     <%: Html.ActionLink("RegisterFromAdmin", "Create", "EventRegistration", new { eventID = Model.EventRegistration.EventID, forOthers = true }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                     <%} %>
             </div>

    </div>
</div>

</asp:Content>

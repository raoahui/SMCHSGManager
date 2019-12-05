<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.EventRegistrationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
		SMCH Association Singapore - 	Event Registration Confirmation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
    <div class="fullwidth">       
   <h2>Event Registration Confirmation</h2>

   <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
         
            <div class="dashedlineIndex"></div>
           
            <table style="border:0; width:60%"  >
                     
                            <%if (Model.EventRegistration.BackDateTime != null){ %>
                             <tr>
                                  <td class="formvalue" colspan="4" >
                                          Leaving Time  &nbsp;&nbsp;<%: Html.DisplayFor(model => model.EventRegistration.BackDateTime)%>
                                  </td>
                            </tr>

                            <tr>
                                <td colspan=4 class="formlabel">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                            </tr>
                            <%} %>

                            <% if (Model.EventVolunteerJobBookingLabels.Count() > 0)
                              {%>
                            <tr>
                                <td colspan=2 class="eventview" style="text-decoration:underline">
                                    You&#39;ll be volunteering for the following job(s)
                                </td>
                            </tr>
                             <%   int i = 0;
                                  foreach (string s in Model.EventVolunteerJobBookingLabels)
                                  {
                                      string s1 = Model.EventVolunteerJobBookingValues.ElementAt(i++); %>
                            <tr>
                                    <td class="formlabel" nowrap="nowrap"><%: s%></td>
                                    <td class="formvalue"><%: s1%></td>
                           </tr>
                               <%} %>
                             <tr>
                                <td colspan=2 class="eventview">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                            </tr>

                          <%}%>

                          

                         <%  if (Model.LocalRetreatMealBookingLabels != null)
                             {
                                 if (Model.LocalRetreatMealBookingLabels.Count() > 1)
                                 {%>
                            <tr>
                                <td colspan=2 class="eventview" style="text-decoration:underline">
                                    Meal Booked
                                </td>
                            </tr>
                          <%}%>
  
                          <%    int count = 0;
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
                                    <td class="formvalue"><%: "SGD$" + price.ToString()%></td>
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
                                    <td class="formvalue" style="font-size:1.2em; color:Black; font-weight:bolder";  >SGD$<%: tPrice.ToString() %></td>
                           </tr>

                            <tr>
                                <td colspan=2 class="eventview">
                                     <div class="dashedlineIndex"> </div>
                                </td>
                          </tr>
                        <%} %>

                      <tr>
                                <td align="center" style=" border:0"colspan=2 >
                                     <input type="submit" value="Confirm" class ="buttonsmall"/>&nbsp; &nbsp;
                                    <%-- <%: Html.ActionLink("Modify", "Modify", null, new { @style = "color:white;", @class = "buttonsmall" })%> &nbsp; &nbsp--%>
                                   <%--  <%: Html.ActionLink("Cancel", "Index", "Home", null, new { @style = "color:white;", @class = "buttonsmall" })%>--%>
                                   <%: Html.ActionLink("Cancel", "Details", "Event", new { id = Model.EventRegistration.EventID }, new { @style = "color:white;", @class = "buttonsmall" })%>
                                 </td>
                      </tr>

            </table>
  <%} %>
    </div>
</div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.EventViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Event Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
 
       <div class="fullwidth">        
            <div class="listitem">
                <div class = "titlenextlink">
                    <%: Html.ActionLink("Event List", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
                 </div>
            </div>

           <div class="listitem">
					<% string eventTitle = Model.Event.Title;
						 if (Model.Event.EventTypeID == 1)
						{
							eventTitle += ' ' + Model.Event.EventType.Name; 
						} %>
				     <div class="title">
							 <%: eventTitle %> 
					</div>

                       <p><% Html.RenderPartial("RSVPStatus", Model.Event); %></p>
  
                       <div class="dashedline"></div>

                       <p>
							<% if (User.IsInRole("Administrator")) {%> 
								Organiser: <%:Model.Event.MemberInfo.Name%>  
							<%} %>
                       
                            <% if (Model.Event.IsPublic)
                                 { %> (Public) <%}
                                 else
                                 { %><font color="red">(For Initiates Only) </font>
                                  <%} %>
    
                            <%  if (Model.Event.EventSchedules.Count() > 0 && Model.Event.EventTypeID == 1)
                                {%>
                                     &nbsp; &nbsp;   <%: Html.ActionLink("Schedule", "Table", "EventSchedule", new { localRetreatID = Model.Event.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                             <%} %>
                        </p>
                              
                          <div class="dashedline"></div>
                             
                           <p style="font-weight:bold"> Start from <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", Model.Event.StartDateTime)%>  &nbsp;  to  &nbsp;  <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", Model.Event.EndDateTime)%>
                             </p>

                          <p style="font-weight:bold"> Registration opens <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", Model.Event.RegistrationOpenDate)%> &nbsp; to  &nbsp;  <%: String.Format("{0:ddd, MMM d yyyy HH:mm}", Model.Event.RegistrationCloseDate)%>
                          </p>
    
                         <div class="itemdetails">
                           <%-- <p>EventType : <%: Model.Event.EventType.Name%></p>--%>
                             <% if (Model.Event.StaticURL != null)
                                {%>
                                     <p> <a  href="<%: Model.Event.StaticURL %>"> URL</a> </p>
                              <%} %>
                            <p>
                                Location :  <%: Html.ActionLink(Model.Event.Location.Name, "Details", "Location", new { id = Model.Event.LocationID }, null)%>
                            </p>
                            <%
                                if (Model.Event.Description != null)
                                {%>
                                    <%--<p> Description : <%: Model.Event.Description%> </p>--%>
										<%: MvcHtmlString.Create(Model.Event.Description)%>
                                <%} %>
                        </div>
   
                        <%if (Model.Event.EventUploadFiles != null)
                         {
							 List<SMCHSGManager.Models.UploadFile> uploadFiles = Model.Event.EventUploadFiles.Select(a => a.UploadFile).ToList();
							 foreach (SMCHSGManager.Models.UploadFile uploadFile in uploadFiles)
                                {
									//string srcImage = "/Image/ShowPhoto/" + uploadFile.ID.ToString();
									string srcImage = uploadFile.FilePath + uploadFile.Name;
									%>
									<p>
										<%: Html.ActionLink(uploadFile.Name, "FileDownload", "Image", new { imageID =uploadFile.ID }, new { style = "color:Olive " })%>
									</p>
                                   <%-- <% Html.RenderPartial("FileDownload"); %>--%>
									<% if (uploadFile.ContentType.StartsWith("image"))
										{%>
											 <img src="<%: srcImage %>"  width = "200" alt="" />
									<%} %>
                             <%}%>
                         <%}%>
 

                         <div class="actionbuttons" >
                          <%                     
                           if (User.IsInRole("Administrator"))
                          {%>
                                <%: Html.ActionLink("RegisterFromAdmin", "Create", "EventRegistration", new { eventID = Model.Event.ID, forOthers = true }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                <%  if(Model.Event.EventRegistrations.Count() > 0){ %>
 
                                  <%: Html.ActionLink("RegisterList", "Index", "EventRegistration", new { eventID = Model.Event.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                  <%: Html.ActionLink("CheckInList", "IndexByEvent", "EventRegistration", new { eventID = Model.Event.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
								 <%: Html.ActionLink("Sign From Admin", "SignatureList4GMAndLocalRetreat", "EventSignature", new { eventTypeID = Model.Event.EventTypeID, eventID = Model.Event.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>
                                 <% if(Model.Event.EventRegistrations.Any(a=>a.EventVolunteerJobBookings.Count() >0)){ %>
										<%: Html.ActionLink("VolunteerJobBooking", "Table", "EventVolunteerJobBooking", new { localRetreatID =  Model.Event.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>  &nbsp;
								  <%} %>
                                <% if (Model.Event.EventType.Name.Contains("Retreat") && Model.Event.EventRegistrations.Any(a => a.EventMealBookings.Count() > 0))
                                    {%> 
                                              <%: Html.ActionLink("MealBooking", "Table", "EventMealBooking", new { localRetreatID =  Model.Event.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                     <%} %>
                             <%}%>

       
                              <%
                              if(Model.Event.EventPrices.Count() > 0){%>
                                    <%: Html.ActionLink("EventPrices", "Index", "EventPrice", new { eventID = Model.Event.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>  &nbsp;
                               <%} %>
                              
                              <%  if (Model.Event.EventSchedules.Count() > 0 && Model.Event.EventTypeID == 1)
                                    {%>
                               <%: Html.ActionLink("VolunteerJob", "Index", "EventVolunteerJob", new { localRetreatID = Model.Event.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                                <%} %>
                              &nbsp;
                              <%if (Model.Event.StartDateTime <= DateTime.Today.ToUniversalTime().AddHours(8) && Model.Event.EndDateTime.ToUniversalTime().AddHours(8) >= DateTime.Today)
                                { %>
                                  <%: Html.ActionLink("Edit", "Edit", new { @style = "color:white;", id = Model.Event.ID }, new { @class = "buttonsmall" })%> 
                             <%}
                           } %>
               
                          </div>

                        <div class="dashedline"></div>

                        <div class="prenextlink">
                        <% 
                            int current = Model.EventIDs.IndexOf(Model.Event.ID); %>
                      
                           <%if (current < Model.EventIDs.Count - 1)
                          { %>
                            <%-- <%: Html.ActionLink("Next Event", "Details", new { id = Model.EventIDs[current + 1] }, new { @style = "color:white;", @class = "buttonsmall" })%>--%>
                            <%: Html.ActionLink("Previous Event", "Details", new { id = Model.EventIDs[current + 1] }, new { @style = "color:white;", @class = "buttonsmall" })%>
                             <%} %>
 
                        <%if (current > 0)
                          { %>
                            <div class="nextlink">
                           <%: Html.ActionLink("Next Event", "Details", new { id = Model.EventIDs[current - 1] }, new { @style = "color:white;", @class = "buttonsmall" })%>
                             </div>
                        <%--<%: Html.ActionLink("Previous Event", "Details", new { id = Model.EventIDs[current - 1] }, new { @style = "color:white;", @class = "buttonsmall" })%>--%>
                        <%} %>
                        </div>

        <div class="clear2column"></div>
            </div>

   </div>


</div>

</asp:Content>

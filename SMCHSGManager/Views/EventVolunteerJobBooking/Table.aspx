<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.EventVolunteerJobBookingViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Table
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="body">
	<% SMCHSGManager.Models.Event aEvent = (SMCHSGManager.Models.Event)(ViewData["LocalRetreat"]); %>
    <h5><%: aEvent.Title%> Local Retreat Volunteer Job Booking Table</h5>
   
    <h6>Start Time : <%: aEvent.StartDateTime %> </h6>
    <h6>End Time : <%: aEvent.EndDateTime %> </h6>

<table style="border:0">
    <tr>
       
         <%
			 List<SMCHSGManager.Models.VolunteerJobType> volunteerJobTypes = (List<SMCHSGManager.Models.VolunteerJobType>)(ViewData["VolunteerJobTypes"]);
            foreach(SMCHSGManager.Models.VolunteerJobType volunteerJobType in volunteerJobTypes) 
              {%>
            
        <td style="border:0">
            <table>
                <tr>
                    <th></th>
                    <th colspan=2><%: volunteerJobType.Remark %></th>
               </tr>

                    <% int i = 0;
                       foreach (var item in Model)
                       {%>
							<tr>
                               <% if (volunteerJobType.Remark == item.VolunteerJobName){ %>
                                     <td><%: (++i).ToString()%></td>
                                     <td nowrap = "nowrap"> <%: item.PersonInCharge%> </td>
                                     <td nowrap = "nowrap"> <%: item.VolunteerJobTime%> </td>
                              <%}%>
							</tr>
                       <%}%>
            </table>
        </td>
      <%} %>

    </tr>
</table>

   <div align="center" style="margin-bottom:20px; margin-top:20px">
             <%: Html.ActionLink("Back", "Details", "Event", new { id = (int)ViewData["LocalRetreatID"] }, new { @style = "color:white;", @class = "buttonsmall" })%> 
   </div>

</div>
</asp:Content>


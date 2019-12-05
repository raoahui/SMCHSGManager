<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.EventScheduleViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="body">
   <h5>Create Local Retreat Schedule item</h5>

    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
          
   
 <table align =center  style="width: 40%" bgcolor="#2A4013">
  <%--  <%: Html.Editor("&", "TableTitle") %>--%>
    <tr>
		<td nowrap="nowrap" >
             <%: Html.EditorFor(model => model.EventSchedule, new { EventActivities = Model.EventActivity, ScheduleOffsets = Model.ScheduleOffset, Mode = "create" })%>
		</td>
	</tr>
  
   <tr>
       <td bgcolor="#2A4013" style="height: 16px" ><font color="red" size="2">*</font><font face="Arial" color="white" size="2">Please Select Volunteer Jobs</font>
       </td>
   </tr>                
   <tr>
 	  	<td nowrap="nowrap" >
	          <table  align="left"   border="1" width = 100%  style ="border-color:Lime; background-color: #FED36B; border-bottom-style:groove">
				  <%
                          int iValue = 0;
                          foreach(var volumnJobName in Model.VolunteerJobNameLabels)
                          {
                               List<SelectListItem> si = new List<SelectListItem>();
                              for (int i = 0; i <= 6; i++)
                              {
                                  SelectListItem item = new SelectListItem { Text = (i).ToString(), Value = i.ToString() };
                                  if (Model.VolunteerJobNameValues[iValue] == i.ToString())
                                  {
                                      item.Selected = true;
                                  }
                                  si.Add(item);
                              }
                    %>
                      <tr style="height: 28px" >
                          <td style="width: 200px"><%: Html.Label(volumnJobName)%></td>    
                          <td><%: Html.DropDownList("VolunteerJobChecks", si)%></td>
                      </tr>
                    <%
                              iValue++;
                          } 
                    %>
              </table>
        </td>
   </tr>
   <%-- <%: Html.Editor("&", "TableFooter") %>--%>
</table>
 
               <div class="editbuttons" align="center">
		            <input type="submit" value="Create"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Index", new { localRetreatID = Model.EventSchedule.EventID }, new { @style = "color:white;", @class = "buttonsmall" })%>
             </div>

    <% } %>

  
</div>
</asp:Content>


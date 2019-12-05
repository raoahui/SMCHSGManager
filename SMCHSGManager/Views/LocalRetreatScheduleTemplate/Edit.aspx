<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.LocalRetreatScheduleTemplateViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <div id="body">
 <h5>Edit Local Retreat Schedule</h5>
 
   <% Html.EnableClientValidation(); %>
   <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
 <table align =center  style="width: 40%" bgcolor="#2A4013">
   <%-- <%: Html.Editor("&", "TableTitle") %>--%>
    <tr>
		<td nowrap="nowrap" >
             <%: Html.EditorFor(model => model.LocalRetreatScheduleTemplate, new { LocalRetreatActivities = Model.EventActivity, LocalRetreatScheduleOffsets = Model.ScheduleOffset, Mode = "edit"})%>
		</td>
	</tr>

</table>
 
       <div align="center" style="margin-bottom:10px; margin-top:20px">
                <input type="submit" value="Save" class ="buttonsmall"/>
       </div>
  
    <% } %>

   <div align="left" style="margin-left:30px; margin-bottom:10px; margin-top:10px">
         <%: Html.ActionLink("Back to Home", "index", "Home")%>
    </div>
</div>

</asp:Content>


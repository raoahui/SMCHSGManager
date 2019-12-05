<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.GroupMeditationAttendance>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body" align="center">
<br />
    <% SMCHSGManager.Models.GroupMeditation aEvent = (SMCHSGManager.Models.GroupMeditation)ViewData["aEvent"];
        string dString = String.Format("{0:ddd, d MMM yyyy  }", aEvent.StartDateTime) + String.Format("{0:HH:mm}", aEvent.StartDateTime) + " to " + String.Format("{0:HH:mm}", aEvent.EndDateTime) + ' ' +"Group Meditation"; %>
   
    <h2>Attendance Name List </h2> <h4><%: dString %> </h4>
    <br />

    <table>
        <tr>
            <th></th>
            <th></th>
            <th>
                Member No
            </th>
            <th>
                Name
            </th>
            <th>
                IDCard Number
            </th>
            <th>
            <% bool Descending = (bool)ViewData["Descending"];
			   string imageFile = "../../images/arrow_up.png";
			   string orderBy = "Ascending";
                
               if (!Descending)
               {
				   imageFile = "../../images/arrow_down.png";
				   orderBy = "Descending";
               }
            	 %>
               Checking Time 
               <a href="<%= Url.Action("Details", "GroupMeditation",  new  { eventID = aEvent.ID, descending = !Descending }) %>" ><img src=<%: imageFile %> title = "<%: orderBy%>" /></a>  &nbsp; 
				 <%--<%: Html.ActionLink(DescendingStr, "Details", new { eventID = aEvent.ID, descending = !Descending })%>--%>
            </th>
      
        </tr>

    <%  int i=1; 
        foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: (i++).ToString() %>
            </td>
            <td>
					<%: Html.ActionLink("Delete", "DeleteAttendData", new { GMAttendID = item.ID })%>
           </td>
           <td align="left">
                <%: item.MemberInfo.MemberNo.ToString() %>
            </td>
           <td align="left">
                <%: item.MemberInfo.Name %>
            </td>
            <td>
                <%: item.MemberInfo.IDCardNo %>
            </td>
            <td>
                <%: string.Format("{0:d MMM yyyy HH:mm}", item.CheckInTime) %>
            </td>
        
        </tr>
    
    <% } %>

    </table>
    <br />
 	
    <div class="actionbuttons" >
     <%                     
         if (User.IsInRole("Administrator"))
         {%>

           <%: Html.ActionLink("New Attendance", "AddNewAttend", "GroupMeditation", new { GMID = aEvent.ID }, new { @style = "color:white;", @class = "buttonsearch" })%> 
         <%} %>
    </div>

</div>

</asp:Content>


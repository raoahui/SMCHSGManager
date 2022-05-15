<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.GMVolunteerJobName>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="body">
	
   <% if (Model.Count() == 0)
	  {%>
	  </br>
          <div align="center" style=" margin-bottom:20px; margin-top:15px; color:Green; font-size:14px">
         There is no GM Volunteer Job Name record in database, please use "Create New" button to create new one.
        </div>      
     <% }
	  else
	  {%>  
		
    <h5><%: Model.FirstOrDefault().VolunteerJobType.Name%> Name List</h5>

    <table>
        <tr>
            <th>No</th>
			<th></th>
            <th>
                Name
            </th>
           <th>
                Contract No
            </th>
       <%--    <th>
                VolunteerJobTypeID
            </th>--%>
            <th>
                Mon
            </th>
            <th>
                Tue
            </th>
            <th>
                Wed
            </th>
            <th>
                Wed(O)
            </th>
            <th>
                Thu
            </th>
            <th>
                Fri(Conv)
            </th>
            <th>
                Sat Day
            </th>
           <th>
                Sat Eve
            </th>
            <th>
                Sat(O)
            </th>
            <th>
                Sun
            </th>
            <th>
                Sun Eve
            </th>
         <%--   <th>
                Availability
            </th>--%>
        </tr>

    <% int i = 0;
		  foreach (var item in Model)
	   { %>
    
        <tr>
			<td><%: (++i).ToString() %></td>
            <td>
					<%: Html.ActionLink("Edit", "Edit", new { memberID = item.MemberID, volunteerJobTypeID = item.VolunteerJobTypeID })%> |
					<%: Html.ActionLink("Delete", "Delete", new { memberID = item.MemberID, volunteerJobTypeID = item.VolunteerJobTypeID })%>
            </td>
            <td>
                <%: item.MemberInfo.Name%>
            </td>
            <td>
                <%: item.MemberInfo.ContactNo%>
            </td>
         <%--  <td>
                <%: item.VolunteerJobTypeID %>
            </td>--%>
            <td>
			    <% if (item.Monday)
				{ %> Y	<%} %> 
            </td>
            <td>
			    <% if (item.Tuesday)
				{ %> Y	<%} %> 
            </td>
            <td>
			    <% if (item.Wednesday)
				{ %> Y	<%} %> 
            </td>
 	        <td>
 			    <% if (item.WednesdayOvernight)
				{ %> Y	<%} %> 
            </td>
 	        <td>
 			    <% if (item.Thursday)
				{ %> Y	<%} %> 
            </td>
            <td>
			    <% if (item.Friday)
				{ %> Y	<%} %> 
            </td>
            <td>
			    <% if (item.SaturdayDay)
				{ %> Y	<%} %> 
            </td>
            <td>
			    <% if (item.SaturdayEvening)
				{ %> Y	<%} %> 
            </td>
            <td>
 			    <% if (item.SaturdayOvernight)
				{ %> Y	<%} %> 
            </td>
            <td>
  			    <% if (item.Sunday)
				{ %> Y	<%} %> 
            </td>
            <td>
  			    <% if (item.SundayEvening)
				{ %> Y	<%} %> 
            </td>
           <%-- <td>
                <%: item.Availability %>
            </td>--%>
        </tr>
    
    <% } %>

    </table>
	<%} %>

		</br>

	<div class="editbuttons" align="center">
			 <%: Html.ActionLink("Create New", "Create", new { volunteerJobTypeID = (int)ViewData["volunteerJobTypeID"] })%>
	</div>

</div>
</asp:Content>


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
         There is no DP name list record in database, please use "Create New" button to create new one.
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
                MemberNO
            </th>
           <th>
                Contract No
            </th>
</tr>

    <% int i = 0;
		  foreach (var item in Model)
	   { %>
    
        <tr>
			<td><%: (++i).ToString() %></td>
            <td>
					<%: Html.ActionLink("Delete", "Delete", new { memberID = item.MemberID, volunteerJobTypeID = item.VolunteerJobTypeID })%>
            </td>
            <td>
                <%: item.MemberInfo.Name%>
            </td>
            <td>
                <%: item.MemberInfo.MemberNo%>
            </td>
            <td>
                <%: item.MemberInfo.ContactNo%>
            </td>
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


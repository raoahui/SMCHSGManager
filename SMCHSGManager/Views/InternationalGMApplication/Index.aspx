<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.InternationalGMApplicationInfo1>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
   <div align="center" style=" margin-top:20px; margin-bottom:20px; color:#2A4013; font-size:1.6em; font-family:Verdana; font-weight:bold">
        List of Application for Internation GM
   </div>

		<% 	List<SelectListItem> assi = (List<SelectListItem>)ViewData["applicationStatuses"];
			List<SelectListItem> aisi = (List<SelectListItem>)ViewData["AshramAndCenterInfos"];%>

     <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

			<%if (Roles.IsUserInRole("SuperAdmin")) {%>
					 <div align="center" style="margin-bottom:20px; color:#2A4013; ">
						Application Status:  &nbsp;  <%: Html.DropDownList("applicationStatusID", assi, new { onChange = "this.form.submit()" })%> 		&nbsp;&nbsp;&nbsp;	
						Ashram:  &nbsp;  <%: Html.DropDownList("ashramID", aisi, new { onChange = "this.form.submit()" })%> 			
					</div>	
			<%}%>

  <% if (Model.Count() == 0) { %>
        <div align="center" style=" margin-bottom:20px; margin-top:15px; color:Green; font-size:14px">
            There is no International GM record in database.
        </div>      
  <%}else{ %>

    <table>
        <tr>
            <th></th>
            <th>
                Name
            </th>
            <th>
                Ashram
            </th>
            <th>
                ArrivalDate
            </th>
            <th>
                DepartureDate
            </th>
            <th>
                ApplyDate
            </th>
			<th>
				Application Status
			</th>
			<th>
				Remark
			</th>
			<% if (Roles.IsUserInRole("SuperAdmin") && (assi.Single(a => a.Selected).Value == "1" || assi.Single(a => a.Selected).Value == "4"))
      {%>
							<th> ApprovalResult </th>
			<%}%>

        </tr>

    <% int i = 0;
		 foreach (var item in Model) { %>
    
        <tr>
            <td>
				<%: (++i).ToString() %>
             </td>
             <td nowrap = "nowrop">
			    <%: Html.ActionLink(item.InternationalGMApplicationInfo.MemberInfo.Name, "Details", new { id = item.InternationalGMApplicationInfo.ID })%> 
              </td>
            <td>
                <%: item.InternationalGMApplicationInfo.AshramAndCenterInfo.Name%>
            </td>
            <td>
                 <%: String.Format("{0:d MMM yyyy}", item.InternationalGMApplicationInfo.ArrivalDate)%> 
            </td>
            <td>
                 <%: String.Format("{0:d MMM yyyy}", item.InternationalGMApplicationInfo.DepartureDate)%>
            </td>
             <td>
                <%: String.Format("{0:d MMM yyyy HH:mm}", item.InternationalGMApplicationInfo.ApplyDate)%>
            </td>
			<td>
			  <%: item.ApplicationStatus %>
			</td>
            <td>
				<%: item.InternationalGMApplicationInfo.Remark%>
            </td>

			<% if (Roles.IsUserInRole("SuperAdmin") && (assi.Single(a => a.Selected).Value == "1" && item.ApplicationStatus == "Screening" || assi.Single(a => a.Selected).Value == "4" && item.ApplicationStatus == "Pending Approval")) 
				{%>
			<td> 	
							<%: Html.ActionLink("Reject?", "Details", new { id = item.InternationalGMApplicationInfo.ID })%> |
						    <%: Html.ActionLink("Approve", "Approve", new { id = item.InternationalGMApplicationInfo.ID })%> 
			</td>
				<%}%>    
	    </tr>
		
    <% } %>

    </table>

   <%}%>
  <%}%>

    <div class="editbuttons" align="center">
		    <%--<%: Html.ActionLink("Create New", "Requirements", new {ashramID = }, new { @style = "color:white;", @class = "buttonsmall" })%>--%>

		<% if (Roles.IsUserInRole("SuperAdmin") && assi.Single(a => a.Selected).Value == "2" && aisi.Single(a => a.Selected).Text != "All")  {%>
			<%--&nbsp;&nbsp; <%: Html.ActionLink("Submit to " + Model.FirstOrDefault().InternationalGMApplicationInfo.AshramAndCenterInfo.Name, "SubmitToDestinationAshram", new { ashramID = int.Parse(aisi.Single(a => a.Selected).Value) }, new { @style = "color:white;", @class = "buttonsmall" })%>--%>
            &nbsp;&nbsp; <%: Html.ActionLink("Submit to Singapore Centre", "SubmitToDestinationAshram", new { ashramID = int.Parse(aisi.Single(a => a.Selected).Value) }, new { @style = "color:white;", @class = "buttonsmall" })%>
		 <%} %>

    </div>

</div>

</asp:Content>


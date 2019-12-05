<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.ViewModel.PublicMemberInfo>" %>
 
       <style type="text/css">
		   .style1
		   {
			   width: 117px;
		   }
		   .style2
		   {
			   width: 132px;
		   }
	   </style>
 
             <% if(!string.IsNullOrEmpty((string)ViewData["errorMsg"])) { %>
            <p style="color:Red; text-align:center"><%: ViewData["errorMsg"]  %></p>
            <%} %>

       <table style="border:0" >     
                       
					<tr>
						<%if (ViewData["Mode"] == "Edit" && !Roles.IsUserInRole("Administrator")){ %>
							<td class="formlabel5" nowrap="nowrap">Name </td>
							<td class="formlabel6" nowrap="nowrap"><%: Model.Name%>  </td>
						<%}else{ %>
							<td class="formlabel5" nowrap="nowrap">Name<font color="red" size="1">*</font></td>
							<td class="style2" nowrap="nowrap">	<%: Html.TextBoxFor(model => model.Name)%> </td>
							<%--<td class="formlabel5" nowrap="nowrap">Member Fee Expire Date</td>
							<td class="style2" nowrap="nowrap" ><%: Html.EditorFor(model => model.MemberFeeExpiredDate, "Date")%></td>--%>					
						<%} %>
						<td class="formlabel5" nowrap="nowrap">Member Fee Expire Date</td>
						<% if (Model.MemberFeeExpiredDate.HasValue && Model.MemberFeeExpiredDate.Value == new DateTime(2020, 12, 31)){ %>
							<td class="formlabel6" nowrap="nowrap"> Giro </td>
						<%}else { %>
							<td class="formlabel6" nowrap="nowrap"><%: Html.DisplayFor(model=>model.MemberFeeExpiredDate, "Date")%></td>
						<%} %>
					</tr>
 
                   <%-- <tr>
						<%if (ViewData["Mode"] == "Edit" && !Roles.IsUserInRole("Administrator")){ %>
							<td class="formlabel5" nowrap="nowrap">Name </td>
							<td class="formlabel6" nowrap="nowrap"><%: Model.Name%>  </td>
						<%}else{ %>
							<td class="formlabel5" nowrap="nowrap">Name<font color="red" size="1">*</font></td>
							<td class="style2" nowrap="nowrap">	<%: Html.TextBoxFor(model => model.Name)%> </td>
						<%} %>
							<td class="formlabel5" nowrap="nowrap">Member Fee Expire Date</td>
							<% if (Model.MemberFeeExpiredDate.HasValue && Model.MemberFeeExpiredDate.Value == new DateTime(2020, 12, 31)){ %>
								<td class="formlabel6" nowrap="nowrap"> Giro </td>
							<%}else { %>
								<td class="formlabel6" nowrap="nowrap"><%: Html.DisplayFor(model=>model.MemberFeeExpiredDate, "Date")%></td>
							<%} %>
					</tr>--%>

 					<tr>
						<td class="formlabel5" nowrap="nowrap">Member No</td>
						<%if (ViewData["Mode"] == "Edit" && !Roles.IsUserInRole("Administrator")){ %>
							<td class="formlabel6" nowrap="nowrap"><%: Model.MemberNo%> </td>
						<%}else{ %>
							<td class="style1" nowrap="nowrap" ><%: Html.TextBoxFor(model => model.MemberNo)%></td>
						<%} %>
						<td class="formlabel5" nowrap="nowrap">Gender</td>
						<td class="style2" nowrap="nowrap" > 
							<%: Html.DropDownList("GenderID", new SelectList(ViewData["Genders"] as IEnumerable, "ID", "Name", Model.GenderID))%>  
						</td>
					</tr>
 
 					<tr>
						<td class="formlabel5" nowrap="nowrap">ID Card No </br> <font color="green", size=1>Temporary ID Card holder fill "TempID".</font></td>
						<td class="style1" nowrap="nowrap"><%= Html.TextBoxFor(model => model.IDCardNo)%></td>
						<td class="formlabel5" nowrap="nowrap">Contact No</td>
						<td class="style2" nowrap="nowrap"><%= Html.TextBoxFor(model=>model.ContactNo)%></td>  
					</tr>

   					<tr>
						<td class="formlabel5" nowrap="nowrap">Date of Initiation</td>
						<td class="style1" nowrap="nowrap"><%: Html.EditorFor(model => model.DateOfInitiation, "Date")%></td>
						<td class="formlabel5" nowrap="nowrap">Date of Birth</td>
						<td class="style1" nowrap="nowrap"><%: Html.EditorFor(model => model.DateOfBirth, "Date")%></td>
  					</tr>

  					<tr>
						<%--<td class="formlabel5" nowrap="nowrap">Country of Initiation</td>
						<td class="style1" nowrap="nowrap"><%: Html.TextBoxFor(model => model.%></td>--%>
						<td class="formlabel5" nowrap="nowrap" >Email<font color="red" size="1">*</font></td>
						<td class="style1" nowrap="nowrap">	<%: Html.TextBoxFor(model => model.Email)%> </td>
						<td class="formlabel5" nowrap="nowrap">Place of Birth</br> <font color="green", size=1> City - Country</font></td>
						<td class="style1" nowrap="nowrap"><%: Html.TextBoxFor(model => model.CountryOfBirth)%></td>
  					</tr>

					<tr>
 						<td class="formlabel5" nowrap="nowrap" >Passport No</td>
						<td class="style1" nowrap="nowrap"><%: Html.TextBoxFor(model => model.PassportNo)%></td>
 						<td class="formlabel5" nowrap="nowrap">Initiate Type </td>
						<td class="style2" nowrap="nowrap">							
						    <%: Html.DropDownList("InitiateTypeID", new SelectList(ViewData["InitiateTypes"] as IEnumerable, "ID", "Name", Model.InitiateTypeID))%>  
                        </td>
                  </tr>

					    <%if (ViewData["Mode"] == "Edit" && Roles.IsUserInRole("Administrator"))
					    { %>
					<tr>
 							<td class="formlabel5" nowrap="nowrap">IsActive </td>
							<td class="style2" nowrap="nowrap"><%: Html.CheckBoxFor(model => model.IsActive)%></td>
                   </tr>
				   		   <%} %>
 
 					<tr>
						<td class="formlabel5" nowrap="nowrap">Remark</td>
						<td class="style1" nowrap="nowrap" colspan=3 ><%: Html.TextBoxFor(model => model.Remark, new { style = "width:100%;", })%> </td>
					</tr>

                </table>


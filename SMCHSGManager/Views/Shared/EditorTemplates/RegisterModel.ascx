<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.RegisterModel>" %>
<script src="/Scripts/MicrosoftAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcValidation.js" type="text/javascript"></script>

 
   			<table style="border:0; " >
       
  				<tr>
                   <% if(String.IsNullOrEmpty(Model.UserName))
                      { %>
							<td class="formlabel3" >User Name<font color="red" size="1" >*</font></td>
 							<td align="left" class="formvalue"><%: Html.TextBoxFor(model => model.UserName)%>     
                               <%= Html.ValidationMessageFor(model => model.UserName) %></td>
                    <%}else{ %>
                          <td class="formlabel3" >User Name</td>
                           <td align="left" class="formvalue"><%: Model.UserName%></td>
                   <%} %>
                        <td class="formlabel4"></td>
 	                    <td class="formlabel3" >Initiate Status </td>
                            <%  if (Roles.IsUserInRole("Administrator"))
                            {%>
                                    <td class="formvalue"><%: Html.DropDownList("InitiateTypeID", new SelectList(ViewData["InitiateTypes"] as IEnumerable, "ID", "Name", Model.InitiateTypeID))%>  </td>
                        <%}
                              else
                              { %>
                                    <td class="formvalue"><%: ((List<SMCHSGManager.Models.InitiateType>)ViewData["InitiateTypes"]).Single(a=>a.ID ==Model.InitiateTypeID).Name %>  </td>
                            <%} %>
					
					</tr>
	
					<tr>
						<td class="formlabel3">Name<font color="red" size="1">*</font></td>
						<td align="left" class="formvalue" >
							<%: Html.TextBoxFor(model => model.Name)%>     
                            <%= Html.ValidationMessageFor(model => model.Name) %></td>
                        <td class="formlabel4"></td>

  						<td  class="formlabel3" >Email<font color="red" size="1">*</font></td>
						<td  class="formvalue">
							<%: Html.TextBoxFor(model => model.Email)%>     
                            <%= Html.ValidationMessageFor(model => model.Email) %></td>
	                </tr>

                    <tr>
                        <td colspan="5" class="formlabel"><font color="Green" size="1">Please make sure the name is exactly same as your ID card's name if you are initiate.</font></td>
                    </tr>

                   <% if(String.IsNullOrEmpty(Model.UserName))
                      { %>
					<tr>
						<td class="formlabel3">Password<font color="red" size="1">*</font></td>
						<td class="formvalue"><%: Html.PasswordFor(m => m.Password) %>
                            <%: Html.ValidationMessageFor(m => m.Password) %></td>
                    <td class="formlabel4"></td>
	    				<td class="formlabel3">Confirm Password<font color="red" size="1">*</font></td>
						<td class="formvalue"><%: Html.PasswordFor(m => m.ConfirmPassword) %>
                            <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %></td>
					</tr>
                   <%} %>

	                <%  string[] roles = Roles.GetAllRoles();
	                       
                       if(Roles.IsUserInRole("Administrator"))
                       {
                           List<string> rolesValue = new List<string>();
                           int i = 0;
                           foreach (string role in roles)
                           {
                               rolesValue.Add((++i).ToString());
                           }
                           %> 

    				       <tr>
						        <td class="formlabel3">Role<font color="red" size="1">*</font></td>
                                <td class="formvalue" colspan = "4"><%= Html.CheckBoxList4("RoleChecks", rolesValue, roles.ToList(), null)%>  </td>
					       </tr>
 
                   <%-- <%}
                       else
                       { %>

    				<tr>
						<td class="formlabel3">Role</td>
                        <td class="formvalue" colspan = "4">
                            <% 
                           for (int i = 0; i < Model.Role.Count(); i++)
                           {%> 
                                <%:  roles[int.Parse(Model.Role[i])-1] %>   </br> 
                           <%}%>  
                        </td>
					</tr>--%>

                <%} %>
                
</table>

         


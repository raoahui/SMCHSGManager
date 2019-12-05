<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.RegisterModel>" %>


            <%-- <div class="dashedline"></div>--%>
   
   			<table style="border:0; " >
       
  				    <tr>
                        <td class="formlabel5" nowrap="nowrap">User Name</td>
                        <td class="formlabel6" nowrap="nowrap"><%: Model.UserName%></td>
  						<td  class="formlabel5" nowrap="nowrap" >Initiate Status </td>
                        <td class="formlabel6" nowrap="nowrap" ><%: ViewData["InitiateType"]%>  </td>
                    </tr>
	                  
					<tr>
						<td class="formlabel5" nowrap="nowrap">Name</td>
						<td  class="formlabel6" nowrap="nowrap" ><%: Model.Name%></td>     
  						<td class="formlabel5" nowrap="nowrap"  >Email</td>
						<td class="formlabel6" nowrap="nowrap" ><%: Model.Email%></td>
					</tr>
	                  
                    <%   if (Roles.IsUserInRole("Administrator"))
                         { %>
					<tr>	    
                        <td class="formlabel5" nowrap="nowrap">Role</td>
                        <td class="formlabel6" nowrap="nowrap" colspan="3">
                            <% for (int i = 0; i < Model.Role.Count(); i++)
                               {%> 
                                    <%: Model.Role[i].ToString()%> 
                                 <% if (i < Model.Role.Count() - 1)
                                    {%>
                                    , 
                                    <%} %>
                                <%} %>  
                        </td>				
  					</tr>
                   <%} %>
 
          
                
</table>



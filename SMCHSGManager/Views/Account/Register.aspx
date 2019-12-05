<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.RegisterModel>" %>
<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    SMCH Association Singapore - Register
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
    <div class="fullwidth">    
          
        <div class="listitem">
            <h2>New user registration</h2>
            <div class="dashedline"></div>

    <p align="center">
        Use the form below to create a new account. 
    </p>
    <p align="center">
        Passwords are required to be a minimum of <%: ViewData["PasswordLength"] %> characters in length.
    </p>
          
            <div class="dashedline"></div>

            <% using (Html.BeginForm()) { %>
                <%: Html.ValidationSummary(true, "Account creation was unsuccessful. Please correct the errors and try again.") %>


 			<table style="border:0; width:60%; " >
                  
                                     <tr>
                                        <td class="formlabel"><font color="red" size="2">*</font>User Name: </td>
                                        <td class="formvalue">
                                            <%: Html.TextBoxFor(m => m.UserName) %>
                                            <%: Html.ValidationMessageFor(m => m.UserName) %>
                                       </td>
                                    </tr>
                                    <tr>
                                        <td class="formlabel"><font color="red" size="2">*</font>Password: </td>
                                        <td class="formvalue">
                                            <%: Html.PasswordFor(m => m.Password) %>
                                            <%: Html.ValidationMessageFor(m => m.Password) %>
                                         </td>
                                    </tr>
                                    <tr>
                                        <td class="formlabel"><font color="red" size="2">*</font>Confirm Password:</td>
                                        <td class="formvalue">
                                            <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                                            <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formlabel"><font color="red" size="2">*</font>E-mail:</td>
                                        <td class="formvalue">
                                            <%: Html.TextBoxFor(m => m.Email) %>
                                            <%: Html.ValidationMessageFor(m => m.Email) %>
                                         </td>
                                    </tr>

                                    <tr>
                                        <td class="formlabel">Name:</td>
                                        <td class="formvalue">
                                            <%: Html.TextBoxFor(m => m.Name) %>
                                            <%: Html.ValidationMessageFor(m => m.Name) %>
                                         </td>
                                     </tr>

                                     <tr>
                                        <td class="formlabel">Initiate Status</td>
                                        <td class="formvalue">
                                            <%: Html.DropDownList("InitiateTypeID", new SelectList(ViewData["InitiateTypes"] as IEnumerable, "ID", "Name", Model.InitiateTypeID))%> 
                                            <%: Html.ValidationMessageFor(m => m.InitiateTypeID)%>
                                        </td>
                                     </tr>

                <%  if(Roles.IsUserInRole("Administrator"))
                       {
                           string[] roles = Roles.GetAllRoles();
                           List<string> rolesValue = new List<string>();
                           int i = 0;
                           foreach (string role in roles)
                           {
                               rolesValue.Add((++i).ToString());
                           }
                            %> 
    				        <tr>
						        <td class="formlabel">Role<font color="red" size="1">*</font></td>
                                <td class="formvalue"><%= Html.CheckBoxList4("RoleChecks", rolesValue, roles.ToList(), null)%>  </td>
					        </tr>
                    <%}%>

           </table>
           
           <div class="dashedline"></div>
           </br>

            <p align="center">     
                <input type="submit" value="Register"  class ="buttonsmall"/>
            </p>

    <% } %>

        </div>
    </div>
</div>
</asp:Content>


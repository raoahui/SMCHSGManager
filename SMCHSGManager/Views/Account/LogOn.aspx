<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    SMCH Association Singapore - Log On</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="body">
     
     <div class="fullwidth">    
         <div class="listitem">
            <h2>Login</h2>
       
            <div class="dashedline"></div>
             
      <p align="center">
        Please enter your username and password. 
       <%-- <%: Html.ActionLink("Register", "Register", "Account", new { initiateOnly = false}, new { @style = "font-weight:bolder;" })%> if you don't have an account.--%>
      </p>

          <% using (Html.BeginForm()) { %>
           
            <%--<%: Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.") %>--%>

          <div class="dashedline"></div>

  			<table style="border:0; width:60%" >
                  <%: Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.") %>
                                    <tr>
                                        <td class="formlabel2">User Name: </td>
                                    </tr>
                                    <tr>
                                        <td class="formvalue">
                                            <%: Html.TextBoxFor(m => m.UserName) %>
                                            <%: Html.ValidationMessageFor(m => m.UserName) %>
                                       </td>
                                    </tr>
                                    <tr>
                                        <td class="formlabel2">Password: </td>
                                    </tr>
                                    <tr>          
                                        <td class="formvalue">
                                            <%: Html.PasswordFor(m => m.Password) %>
                                            <%: Html.ValidationMessageFor(m => m.Password) %>
                                         </td>
                                    </tr>
 
                                     <tr>
                                        <td class="formvalue">
                                            <%: Html.CheckBoxFor(m => m.RememberMe) %>
                                            <%: Html.LabelFor(m => m.RememberMe) %>
                                      </td>
                                    </tr>
                
         </table>

        <div class="dashedline"></div>
        </br>

        <p align="center">           
                 <input type="submit" value="Login"  class ="buttonsmall"/>
        </p>

       <%}%>        
                        
     <p align="center">
     <%: Html.ActionLink("Forgot Your Password?", "RecoverPassword", "Account")%>
     </p>
        </div>
  
</div>
    </div>
</asp:Content>


                  
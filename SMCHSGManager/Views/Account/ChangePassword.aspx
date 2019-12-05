<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.ChangePasswordModel>" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
 SMCH Association Singapore - Change Password
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="body">

    <div  id="columnright"> 
        </br> </br>
       <h2>Change Password</h2>
            </br></br> <p>
        Use the form below to change your password. 
           </p><p>
        New passwords are required to be a minimum of <%: ViewData["PasswordLength"] %> characters in length.
    </p>
    </br> 
    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Password change was unsuccessful. Please correct the errors and try again.") %>
        <div>
                 
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.OldPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.OldPassword) %>
                    <%: Html.ValidationMessageFor(m => m.OldPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.NewPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.NewPassword) %>
                    <%: Html.ValidationMessageFor(m => m.NewPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                </br></br >
           <%-- </fieldset>--%>
        </div>
                 <p>
                    <input type="submit" value="Change Password"  class ="buttonsmall"/>  &nbsp; &nbsp;
                  </p>
                </br>
               <p>
                     <%: Html.ActionLink("Click here", "Index", "Home")%> to Cancel to Home Page.
                 <%--   <a href="javascript:history.go(-1)" style = "color:white;" class="buttonsmall" >cancel</a>--%>
                </p>

   <% } %>
    </div>
    </div>
</asp:Content>

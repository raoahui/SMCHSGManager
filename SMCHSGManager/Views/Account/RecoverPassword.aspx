<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.RegisterModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RecoverPassword1
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="body">
    <h5>Recover Your Password</h5>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

   <p align="center">
    Forgot Your Password?</p>
   <br />

    <p align="center">
    Enter your User Name to reset your password.
    </p>
    <p align="center">
    User Name :

                 <%: Html.TextBoxFor(model => model.UserName) %>
                <%: Html.ValidationMessageFor(model => model.UserName) %>
            <font color="red" size="2">*</font> 
     </p>
            
    <br />
            <p  align="center">
                <%: Html.ActionLink("Back", "Logon", "Account", null, new { @style = "color:white;", @class = "buttonsmall" })%>   &nbsp;&nbsp; 
                <input type="submit" value="Create"   class ="buttonsmall"/>
            </p>
  
    <% } %>

   
</div>
</asp:Content>


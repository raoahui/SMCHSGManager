<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RecoverPasswordConfirmation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2></h2>

<div id="body">

    <h5>Password Recovery Confirmation</h5>
      
    <p align="center">
    You will receive your new password at your email  <font color=green><%: ViewData["Email"] %></font>.
    </p>
	<p align="center">
    If this is not your email, please send your full name and/or username, ID card number and date of initiation to 
		<a href="mailto:admin@smchsg.com">admin@smchsg.com</a>.
    </p>

      <br />

    <h6>
		    <%: Html.ActionLink("Click here", "Index", "Home")%> to return to Home Page.
    </h6>    

</div>

</asp:Content>


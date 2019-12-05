<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
		SMCH Association Singapore - 	Register Confirmation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

    <h5>Register Confirmation</h5>

    <p align="center">
    Thank you for registering. 
    </p>
      
    <p align="center">
    You will receive an email to inform you if your account has been activated
    </p>

  <% if ((bool)ViewData["Initiate"])
       {%>
     <p align="center">
        Your Initiation status need to be confirmed with the Contact Person before your initiate account can be fully activated, in the meantime, you can use it as a Guest account.
     </p>
        <%} %>
  <%--  <br />
    Please confirm again your name, initiation date etc info, if you find any of problem of , please drop an email to admin@smchsg.com --%>
  

      <br />

    <h6>
		    <%: Html.ActionLink("Click here", "Index", "Home")%> to return to Home Page.
    </h6>    

</div>

</asp:Content>

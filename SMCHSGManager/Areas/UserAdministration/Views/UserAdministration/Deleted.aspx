<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.MemberInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="body">
    <h5>Account Deleted</h5>
   
   <div align="center" style=" margin-bottom:10px; margin-top:15px">
    
        <h4><%: Model.Name + ViewData["message"] %></h4>

        <br /> <br /> <br />

         <%: Html.ActionLink("List", "MemberInfo", new { Area = "", initiateOnly = false })%> to return to the  List of User Page.

    </div>

 </div>
 
</asp:Content>


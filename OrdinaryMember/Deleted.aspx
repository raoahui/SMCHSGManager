<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.MemberInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Deleted
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

    <h5>Ordinary Member Deleted</h5>
   
   <div align="center" style=" margin-bottom:10px; margin-top:15px">
    
       <%-- <h4>Ordinary Member was successfully deleted.</h4>--%>
        <h4> <%: ViewData["message"] %> </h4>
 
        <br /> <br /> <br />

         <%: Html.ActionLink("Click here", "index", "OrdinaryMember") %> to return to the Ordiary Member Page.
   
    </div>

 </div>

</asp:Content>


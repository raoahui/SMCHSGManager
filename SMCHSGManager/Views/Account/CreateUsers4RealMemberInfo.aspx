﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	CreateUsers4RealMemberInfo
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <%-- <h2>CreateUsers4RealMemberInfo</h2>--%>
   <div id="body">

<%if (ViewData["errorMessage"] == null)
  { %>
    <h6>Congradulation! CreateUsers from real MemberInfo table successfully.</h6>
<%}
  else
  { %>   
<h6><%: ViewData["errorMessage"]%> </h6>     
<%} %>
            
     <div align="left" style="margin-left:30px; margin-bottom:10px; margin-top:20px">
        <%: Html.ActionLink("Back to Home Page", "Index", "Home")%>
     </div>
</div>  
</asp:Content>

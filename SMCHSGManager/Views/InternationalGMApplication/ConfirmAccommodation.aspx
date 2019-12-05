<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ConfirmAccomodation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
<div class="fullwidth">      
      <% using (Html.BeginForm()) { %>
 			<h6> Do you need Accomodation? please click <%: Html.CheckBox("AccomodationCheckBox")%></h6>
			<p align="center">  <input type="submit" value="Next"  class ="buttonsmall"/> </p>
	<%} %>
</div>

</asp:Content>

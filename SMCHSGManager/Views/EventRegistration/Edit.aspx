<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.EventRegistrationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
		SMCH Association Singapore - 	Event Registration Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
   <div class="fullwidth">        
        <h2>Modify Event Registration</h2>

   <%: Html.EditorFor(model=>model, "EventRegistration", new {Mode = "Modify" })%>

    </div>

</div>  

</asp:Content>

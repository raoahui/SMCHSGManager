<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.MemberFeePaymentViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <div id="body">

	 <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

       <%: Html.EditorFor(model => model.MemberFeePayment, new { MemberInfos = Model.MemberInfos, PayMethods = Model.PayMethod, Mode = "Edit" })%>

	<div align="center">
         <input type="submit"  value="Save"  name="Save" class ="buttonsmall" /> &nbsp;
         <%: Html.ActionLink("Back to List", "Index", null, new { @style = "color:white; ", @class = "buttonsmall" })%>
	 </div>

	<%}%>
        
    </div>

</asp:Content>

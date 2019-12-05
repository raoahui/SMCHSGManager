<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.OrdinaryMemberViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

   <div class="fullwidth">         
  
    <div class="listitem" >

    <h5>User Name: <%: Model.PublicMemberInfo.UserName %></h5>
 
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm("Create", "OrdinaryMember", FormMethod.Post, new { enctype = "multipart/form-data" })){%>
        <%: Html.ValidationSummary(true) %>
        
	        <%: Html.DisplayFor(model => model.PublicMemberInfo)%>   

			<div class="editbuttons" align="center">
			<% 
				if ((bool)ViewData["IsFromLocalHost"] && Model.OrdinaryMemberInfo != null)
			{ %>
				<%: Html.DisplayFor(model => model.OrdinaryMemberInfo, new{ PayMethodName = (string)ViewData["PayMethodName"] })%>

					 <%: Html.ActionLink("Back", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
					 <%: Html.ActionLink("Edit", "Edit", new { id = Model.OrdinaryMemberInfo.IMemberID }, new { @style = "color:white;", @class = "buttonsmall" })%>
					 <%: Html.ActionLink("Delete", "Delete", new { id = Model.OrdinaryMemberInfo.IMemberID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
		  <%} else if(Model.PublicMemberInfo != null){%>
				<%: Html.ActionLink("Back", "Index", null, new { @style = "color:white;", @class = "buttonsmall" })%>
				<%: Html.ActionLink("Edit", "Edit", new { id = Model.PublicMemberInfo.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>
				<%: Html.ActionLink("Delete", "Delete", new { id = Model.PublicMemberInfo.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
			 <%}%>
			</div>

    <% } %>

        </div>
    </div>   
</div>

   

</asp:Content>


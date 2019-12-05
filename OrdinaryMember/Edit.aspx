<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.OrdinaryMemberViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Member Info Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="body">

    <h5>User Name: <%: Model.PublicMemberInfo.UserName %> (Edit)</h5>
 
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm("Edit", "OrdinaryMember", FormMethod.Post, new { enctype = "multipart/form-data" }))
	   {%>
        <%: Html.ValidationSummary(true) %>

 	          <%: Html.EditorFor(model => model.PublicMemberInfo,  new {   
                                    Genders = Model.Gender, 
									InitiateTypes = Model.InitiateType,
									Mode = "Edit" 
                                     })%>   

			<% if ((bool)ViewData["IsFromLocalHost"] && Model.OrdinaryMemberInfo != null){ %>
					<%: Html.EditorFor(model => model.OrdinaryMemberInfo, new{   
                                    Nationalities = Model.Nationality, 
                                    Races = Model.Race, 
                                    EmploymentStatuses = Model.EmploymentStatus, 
                                    PayMethods = Model.PayMethod, 
                                    })%>
						<div align="center" style="margin-bottom:20px; margin-top:20px">
							<%: Html.ActionLink("Back", "Details", new { id = Model.OrdinaryMemberInfo.IMemberID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
							&nbsp;&nbsp;&nbsp;&nbsp; <input type="submit" value="Save" class ="buttonsmall"/>
						</div>
			 <%}else{%>
						<div align="center" style="margin-bottom:20px; margin-top:20px">
							<input type="submit" value="Save" class ="buttonsmall"/>
						</div>
			 <%}%>

    <% } %>
 
</div>

</asp:Content>


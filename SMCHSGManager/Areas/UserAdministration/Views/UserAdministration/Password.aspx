<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SampleWebsite.Areas.UserAdministration.Models.UserAdministration.DetailsViewModel>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	User Details: <%: Model.DisplayName %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <%--	<link href='<% =Url.Content("~/Content/MvcMembership.css") %>' rel="stylesheet" type="text/css" />--%>
        
<div id="body">
 <div class="fullwidth">    
         <div class="listitem">

	<h2 class="mvcMembership">User Details: <%: Model.DisplayName %> [<% =Model.Status %>]</h2>
    <div class="dashedline"></div>

	<ul class="mvcMembership-tabs">
		<li><% =Html.ActionLink("Details", "Details", "UserAdministration", new { area = "UserAdministration", id = Model.User.ProviderUserKey }, null) %></li>
		<li>Password</li>
		<li><% =Html.ActionLink("Roles", "UsersRoles", "UserAdministration", new{area = "UserAdministration", id = Model.User.ProviderUserKey}, null) %></li>
	</ul>
    <div class="dashedline"></div>

	<h3 class="mvcMembership">Password</h3>
	<div class="mvcMembership-password">
		<% if(Model.User.IsLockedOut){ %>
			<p>Locked out since <% =Model.User.LastLockoutDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture) %></p>
			<% using(Html.BeginForm("Unlock", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
			<input type="submit" value="Unlock Account" class ="buttonsmall"/>
			<% } %>
		<% }else{ %>

			<% if(Model.User.LastPasswordChangedDate == Model.User.CreationDate){ %>
			<dl class="mvcMembership">
				<dt>Last Changed:</dt>
				<dd><em>Never</em></dd>
			</dl>
			<% }else{ %>
			<dl class="mvcMembership">
				<dt>Last Changed:</dt>
				<dd><% =Model.User.LastPasswordChangedDate.ToString("MMMM dd, yyyy h:mm:ss tt", CultureInfo.InvariantCulture) %></dd>
			</dl>
			<% } %>

            <div class="dashedline"></div>

			<% if(Model.CanResetPassword && Model.RequirePasswordQuestionAnswerToResetPassword){ %>
				<% using(Html.BeginForm("ResetPasswordWithAnswer", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
				<fieldset>
					<p>
						<dl class="mvcMembership">
							<dt>Password Question:</dt>
							<% if(string.IsNullOrEmpty(Model.User.PasswordQuestion) || string.IsNullOrEmpty(Model.User.PasswordQuestion.Trim())){ %>
							<dd><em>No password question defined.</em></dd>
							<% }else{ %>
							<dd><%: Model.User.PasswordQuestion %></dd>
							<% } %>
						</dl>
					</p>
					<p>
						<label for="answer">Password Answer:</label>
						<% =Html.TextBox("answer") %>
					</p>
					<input type="submit" value="Reset to Random Password and Email User" class ="buttonsmall"/>
				</fieldset>
				<% } %>
			<% }else if(Model.CanResetPassword){ %>
				<% using(Html.BeginForm("SetPassword", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
					<p>
						<label for="password">New Password:</label>
						<% =Html.TextBox("password") %>
					</p>
					<input type="submit" value="Change Password" class ="buttonsmall"/>
				<% } %>
                <br />
				<% using(Html.BeginForm("ResetPassword", "UserAdministration", new{ id = Model.User.ProviderUserKey })){ %>
					<input type="submit" value="Reset to Random Password and Email User" class ="buttonsmall"/>
				<% } %>
			<% } %>

		<% } %>
	</div>

    </div>
    </div>
 </div>

</asp:Content>
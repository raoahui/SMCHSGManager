<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SampleWebsite.Areas.UserAdministration.Models.UserAdministration.IndexViewModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
	User Administration
	<link href='<% =Url.Content("~/Content/MvcMembership.css") %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">


    <div id="body">

    <div class="fullwidth">    
         <div class="listitem">

   <%-- <h2 class="mvcMembership">Roles Administration</h2>--%>
	<h2 class="mvcMembership">User Administration</h2>
	<div class="dashedline"></div>
	<h3 class="mvcMembership">Users</h3>
	<div class="mvcMembership-allUsers">
	<% if(Model.Users.Count > 0){ %>


		<ul class="mvcMembership mvcMembership-users">
			<% foreach(var user in Model.Users){ %>
			<li>
				<span class="mvcMembership-username"><% =Html.ActionLink(user.UserName, "Details", new{ id = user.ProviderUserKey}) %></span>
				<span class="mvcMembership-email"><a href="mailto:<%: user.Email %>"><%: user.Email %></a></span>
				<% if(user.IsOnline){ %>
					<span class="mvcMembership-isOnline">Online</span>
				<% }else{ %>
					<span class="mvcMembership-isOffline">Offline for
						<%
							var offlineSince = (DateTime.Now - user.LastActivityDate);
							if (offlineSince.TotalSeconds <= 60) Response.Write("1 minute.");
							else if(offlineSince.TotalMinutes < 60) Response.Write(Math.Floor(offlineSince.TotalMinutes) + " minutes.");
							else if (offlineSince.TotalMinutes < 120) Response.Write("1 hour.");
							else if (offlineSince.TotalHours < 24) Response.Write(Math.Floor(offlineSince.TotalHours) + " hours.");
							else if (offlineSince.TotalHours < 48) Response.Write("1 day.");
							else Response.Write(Math.Floor(offlineSince.TotalDays) + " days.");
						%>
					</span>
				<% } %>
				<% if(!string.IsNullOrEmpty(user.Comment)){ %>
					<span class="mvcMembership-comment"><%: user.Comment %></span>
				<% } %>
			</li>
			<% } %>
		</ul>
		<ul class="mvcMembership mvcMembership-paging">
			<% if (Model.Users.IsFirstPage){ %>
			<li>First</li>
			<li>Previous</li>
			<% }else{ %>
			<li><% =Html.ActionLink("First", "Index") %></li>
			<li><% =Html.ActionLink("Previous", "Index", new { index = Model.Users.PageIndex - 1 })%></li>
			<% } %>

			<li>Page <% =Model.Users.PageNumber%> of <% =Model.Users.PageCount%></li>

			<% if (Model.Users.IsLastPage){ %>
			<li>Next</li>
			<li>Last</li>
			<% }else{ %>
			<li><% =Html.ActionLink("Next", "Index", new { index = Model.Users.PageIndex + 1 })%></li>
			<li><% =Html.ActionLink("Last", "Index", new { index = Model.Users.PageCount - 1 })%></li>
			<% } %>
		</ul>
	<% }else{ %>
		<p>No users have registered.</p>
	<% } %>
	</div>

	<h3 class="mvcMembership">Roles</h3>
	<div class="mvcMembership-allRoles">
	<% if(Model.Roles.Count() > 0 ){ %>
		<ul class="mvcMembership">
			<% foreach(var role in Model.Roles){ %>
			<li>
				<% =Html.ActionLink(role, "Role", new{id = role}) %>
				<% using(Html.BeginForm("DeleteRole", "UserAdministration", new{id=role})){ %>
				<input type="submit" value="Delete" class ="buttonsmall"/>
				<% } %>
			</li>
			<% } %>
		</ul>
	<% }else{ %>
		<p>No roles have been created.</p>
	<% } %>

    <div class="dashedline"></div>

	<% using(Html.BeginForm("CreateRole", "UserAdministration")){ %>
	<%--	<fieldset>--%>
    <p>
			<label for="id">Role:</label>
			<% =Html.TextBox("id") %>
			<input type="submit" value="Create Role" class ="buttonsmall"/>
    </p>
		<%--</fieldset>--%>
	<% } %>
	</div>

    </div>
  </div>
</div>
</asp:Content>
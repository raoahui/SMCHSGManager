<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<link rel="stylesheet" type="text/css" href="../../Content/Basic.css" title="style1">
	<div id="body">
		<div class="fullwidth">
			<p align="center" style="font-size: x-large; padding: 0.3em;">
				Frequently Asked Questions
				<p align="center" style="font-size: large; padding: 0.1em;">
					<ul class="helpfile">
						<li><a href="<%= Url.Action("Login", "Help") %>">How to login</a></li>
						<li><a href="<%= Url.Action("RegisterEvent", "Help") %>">How to register for an event/retreat</a></li>
						<li><a href="<%= Url.Action("ModifyRegistration", "Help") %>">How to modify your registration
							for an event/retreat</a></li>
						<li><a href="<%= Url.Action("CancelRegistration", "Help") %>">How to cancel your registration
							for an event/retreat</a></li>
						<li><a href="<%= Url.Action("UpdateAccount", "Help") %>">How to update your account
							details</a></li>
						<li><a href="<%= Url.Action("ChangeDate", "Help") %>">How to edit dates correctly when
							updating details</a></li>
						<li><a href="<%= Url.Action("UpdateAccountChinese", "Help") %>">如何更新个人资料</a></li>
						<li><a href="<%= Url.Action("MakeOrder", "Help") %>"><strong style="color: #FF0000">How to Make Orders 
							</strong></a><img src="../images/new1.jpg" /></li>
					</ul>
		</div>
	</div>
</asp:Content>

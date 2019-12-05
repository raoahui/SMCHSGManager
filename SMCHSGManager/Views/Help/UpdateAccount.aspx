<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FAQ: How to Update Your Account Details</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<link rel="stylesheet" type="text/css" href="../../Content/Basic.css" title="style1">
	<div id="body">
		<div class="fullwidth">
			<p align="center" style="font-size: x-large; padding: 0.3em;">
				How to Update Your Account Details
			</p>
			<ol class="helpfile">
				<li>You are strongly encouraged to provide your complete information through this website to
					the Singapore Center.
					<ul>
						<li>The information will be migrated from the website into secure local storage at the Singapore
							Center on a later date. </li>
						<li>There will be another announcement by email before this happens, but you still encouraged
							to update your information within 2 weeks of receiving your username and password.</li>
					</ul>
				</li>
				<li>From any page, click on the words "My Account" on the top navigation bar.
					<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/11v2.PNG" style="border: 1px solid black" width="95%" />
					</center>
				</li>
				<li>Here you will see the your account's current information stored in the system. To
					make updates, click "Edit".<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/12v2.PNG" style="border: 1px solid black;" width="95%" />
					</center>
				</li>
				<li>Here you can edit and update your information. On information how to edit dates for Date of Birth and Date of Initiation, 
					please click <a href="<%= Url.Action("ChangeDate", "Help") %>">here</a>. When you are finished, click 
					&quot;Save&quot;.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/13v2.png" style="border: 1px solid black;" />
					</center>
				</li>
				<li>You will see your updated information. You do this as many times as you like before
					we migrate the information from this website into local storage at the Singapore
					Center.
					<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/14v2.png"  style="border: 1px solid black;" />
					</center>
				</li>
			</ol>
			<p align="center" style="font-size: large; padding: 0.3em;">
				<a href="<%= Url.Action("Index", "Help") %>">Back to FAQ Index</a></p>
		</div>
	</div>
</asp:Content>

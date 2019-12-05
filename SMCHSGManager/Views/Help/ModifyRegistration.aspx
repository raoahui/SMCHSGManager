<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FAQ: How to Modify your Registration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<link rel="stylesheet" type="text/css" href="../../Content/Basic.css" title="style1">
	<div id="body">
		<div class="fullwidth">
			<p align="center" style="font-size: x-large; padding: 0.3em;">
				How to Modify your Registration
			</p>
			<ol class="helpfile">
				<li>If you have registered for a retreat/event, but changed your mind about the time you
					are leaving, meal bookings, or volunteer jobs, you can change these details any
					time you like during the period open for registration. This can be checked by clicking
					the title of the event/retreat, on the Home page or the Events page.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/9.1.PNG" style="border: 1px solid black" />
					</center>
				</li>
				<li>Here you can see the period open for registration and other information about the
					retreat. You can click the &quot;Details&quot; button to check your registration
					details.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/9.25.PNG" style="border: 1px solid black;" />
					</center>
				</li>
				<li>Alternatively, you can also click on the &quot;Details&quot; button on the homepage&#39;s
					event entry.<br />
					<br />
					<center>
						<img border="1px solid black" src="../../images/FAQscreenshots/9.2.PNG" /></center>
				</li>
				<li>This is your confirmation page for your registration. To make changes, click on
					the &quot;Modify Registration&quot; button.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/9.3.png" border="1px solid black" width="95%" />
					</center>
				</li>
				<li>Here you can change the time you leave, meal bookings, and volunteer jobs. Here,
					I have changed my volunteer job from Audio & Video to Dharma Protector. Click "Save".<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/9.4.png" border="1px solid black" width="95%" />
					</center>
				</li>
				<li>Your registration confirmation page will be updated to reflect the changes you have just
					made.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/9.5.png" border="1px solid black" width="95%" />
					</center>
				</li>
			</ol>
			<p align="center" style="font-size: large; padding: 0.3em;">
				<a href="<%= Url.Action("Index", "Help") %>">Back to FAQ Index</a>
			</p>
		</div>
	</div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FAQ: How to Register for an Event or Retreat
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<link rel="stylesheet" type="text/css" href="../../Content/Basic.css" title="style1">
	<div id="body">
		<div class="fullwidth">
			<p align="center" style="font-size: x-large; padding: 0.3em;">
				How to Register for an Event or Retreat
			</p>
			<ol class="helpfile">
				<li>After logging in, you will see a list of events displayed on the home page.
					<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/3.PNG" style="border: 1px solid black" width="95%" />
					</center>
				</li>
				<li>If there are events or retreats currently open for registration, you will see a
					&quot;Register&quot; button. Click this button to register.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/3.1.PNG" style="border: 1px solid black;" />
					</center>
				</li>
				<li>First, indicate if you are going to leave during one of the breaks before the end
					of the retreat. If you are staying for the entire retreat, you do not need to check
					any boxes here.<br />
					<br />
					<center>
						<img border="1px solid black" src="../../images/FAQscreenshots/4.PNG" width="95%" /></center>
				</li>
				<li>Next, indicate the meals you would like to book.
					<br />
					<center>
						<img src="../../images/FAQscreenshots/5.png" border="1px solid black" />
					</center>
				</li>
				<li>Finally, select the jobs you would like to volunteer for. Thank you for helping
					your brothers and sisters to enjoy a nice retreat! After this, click &quot;Save&quot;.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/6.png" border="1px solid black" />
					</center>
				</li>
				<li>The price breakdown and total charge will be shown. If you are satisfied with these,
					click &quot;Confirm&quot;.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/7.png" border="1px solid black" width="95%" />
					</center>
				</li>
				<li>This is the confirmation screen of your retreat registration. From here, you can
					cancel or modify your registration details as many times as you like during the
					period open for registration.<br /><br />
					<center>
						<img src="../../images/FAQscreenshots/8.png" border="1px solid black" width="95%" />
					</center>
				</li>
			</ol>
			<p align="center" style="font-size: large; padding: 0.3em;">
				<a href="<%= Url.Action("Index", "Help") %>">Back to FAQ Index</a>
			</p>
		</div>
	</div>
</asp:Content>

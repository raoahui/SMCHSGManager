<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FAQ: How to Login
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<link rel="stylesheet" type="text/css" href="../../Content/Basic.css" title="style1">
	<div id="body">
		<div class="fullwidth">
			<p align="center" style="font-size: x-large; padding: 0.3em;">
				How to Login
			</p>
			<ol class="helpfile">
				<li>You can login to your account once you have received an email containing your username and password.
				</li>
				<ul>
					<li>Make sure you have given your correct and active email account to the Singapore 
						Center. Make
						sure your Inbox is not full.</li>
					<li>If you have deleted this email or cannot find it, please first check the &quot;Trash&quot;
						and &quot;Junk Mail&quot; folders of your email. </li>
					<li>If you are sure that you cannot find it, please email <a href="mailto:admin@smchsg.com">
						admin@smchsg.com</a>. State in the email your full name, ID card number, and date of
						initiation. Alternatively, look for the brother/sister in charge of local 
						retreat matters after Group Meditation.</li>
				</ul>
				<li>Click on the &quot;Login&quot; button on the top right corner of the page. This is marked
					in red on the screenshot below.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/1.PNG" style="border: 1px solid black" width="95%" /><br />
					</center>
				</li>
				<li>At this Login screen, copy and paste your username and password from your email.
					You can click the &quot;Remember me?&quot; check box if you are NOT using a public
					computer, to save the hassle of logging in again next time.<br />
					<br />
					<center>
						<img border="1px solid black" src="../../images/FAQscreenshots/2.PNG" width="95%" /></center>
				</li>
				<li>You will be redirected to the home page with your username shown on the top 
					right corner when login is successful. Please change your password immediately if you are logging in for the
					first time.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/2.5.png" style="border: 1px solid black" width="95%" />
					</center>
				</li>
				<li>Your account will be locked if you enter the wrong password for 6 consecutive times.
					<ul>
						<li>Use the &quot;Forgot Your Password?&quot; link to generate a new password, which
							will be sent to your email address.</li>
						<li>If the password reset function does not work, email <a href="mailto:admin@smchsg.com">admin@smchsg.com</a>
							to unlock your account. </li>
					</ul>
					<center>
						<img src="../../images/FAQscreenshots/2.6.png" style="border: 1px solid black" width="95%" />
					</center>
				</li>
			</ol>
			<p align="center" style="font-size: large; padding: 0.3em;">
				<a href="<%= Url.Action("Index", "Help") %>">Back to FAQ Index</a>
			</p>
		</div>
	</div>
</asp:Content>

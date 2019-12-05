<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FAQ: How to Edit Dates</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<link rel="stylesheet" type="text/css" href="../../Content/Basic.css" title="style1">
	<div id="body">
		<div class="fullwidth">
			<p align="center" style="font-size: x-large; padding: 0.3em;">
				How to Edit Dates</p>
			<ol class="helpfile">
				<li>The format of any date input on this website, including updating of Date of Initiation in "My Account", need to follow the "DD Mon YYYY" format, like shown below. ("Mon" refers to months being represented in 3-letter form such as "Jan", "Feb", etc.)
					<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/15.PNG" style="border: 1px solid black" />
					</center>
				</li>
				<li>One way to edit dates is to click on the text area (cicled in red) to type in this format yourself. But an easier way is to use the date selector, which will also appear when you click in the red area.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/15.1.PNG" style="border: 1px solid black;"  />
					</center>
				</li>
				<li>Click on either the text area and the button on the right of the text area (both circled in red here) to bring up the date selector.
				<ul>
				<li>If you don't see this appearing, check if your browser have disabled scripts, in which case you should either enable them, make an exception for www.smchsg.com, or use a different browser.</li>
				<li>Otherwise, you can email <a href="mailto:admin@smchsg.com">admin@smchsg.com</a>. 
					In the email, please state what browser you are using.</li>
			</ul>
					<br />
					<center>
						<img src="../../images/FAQscreenshots/16.png" border="1px solid black" />
					</center>
				</li>
				<li>First, select the year by clicking on the arrow button next to the current year, 
					then click on the year you need. If the year you need is earlier than those 
					displayed, click on the first year in this list, then click the arrow button 
					again. Repeat this until you see the year you want, and select it.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/17.1.png" border="1px solid black" />
					</center>
				</li>
				<li>Alternatively, you can use the text area to type in your year, then use the date selector for just the month and date.
					<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/17.2.png" border="1px solid black" />
					</center>
				</li>
				<li>After selecting the year, select the month by clicking on the arrow next to the current month, then clicking on the month of your choice on the list.
					<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/17.3.png" border="1px solid black" />
					</center>
				</li>
				<li>A calendar will appear. From here, click on the date of your choice.
					<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/17.4.png" border="1px solid black" />
					</center>
				</li>
				<li>You have successfully edited the date. If you are updating your account details, do this for both Date of Birth and Date of Initiation, then save.
					<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/17.5.png" border="1px solid black" />
					</center>
				</li>
			</ol>
			<p align="center" style="font-size: large; padding: 0.3em;">
				<a href="<%= Url.Action("Index", "Help") %>">Back to FAQ Index</a></p>
		</div>
	</div>
</asp:Content>

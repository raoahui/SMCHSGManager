<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FAQ: How to Cancel your Registration</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<link rel="stylesheet" type="text/css" href="../../Content/Basic.css" title="style1">
	<div id="body">
		<div class="fullwidth">
			<p align="center" style="font-size: x-large; padding: 0.3em;">
				How to Cancel your Registration
			</p>
			<ol class="helpfile">
				<li>Follow steps 1-3 of <a href="<%= Url.Action("Modify Registration", "Help") %>">How
					to Modify Your Registration</a>, till you see the following screen. Click "Cancel
					Registration".
					<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/10.PNG" style="border: 1px solid black" width="95%" />
					</center>
				</li>
				<li>To cancel your registration, click &quot;Delete&quot;.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/10.2.PNG" style="border: 1px solid black;"
							width="95%" />
					</center>
				</li>
				<li>You will see this screen. You can re-register at any time that you like before registration
					closes.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/10.3.png" border="1px solid black" />
					</center>
				</li>
			</ol>
			<p align="center" style="font-size: large; padding: 0.3em;">
				<a href="<%= Url.Action("Index", "Help") %>">Back to FAQ Index</a></p>
		</div>
	</div>
</asp:Content>

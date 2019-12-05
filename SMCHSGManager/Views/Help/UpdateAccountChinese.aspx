<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FAQ: 如何更新个人资料</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<link rel="stylesheet" type="text/css" href="../../Content/Basic.css" title="style1">
	<div id="body">
		<div class="fullwidth">
			<p align="center" style="font-size: x-large; padding: 0.3em;">
				如何更新个人资料
			</p>
			<ol class="helpfile">
				<li>请尽快更新在网站上更新你的正确个人资料。这些信息过后将不能在网页上更新，需要到新加坡小中心内填表格。
				</li>
				<li>点击页面上方的"My Account"。
					<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/11v2.PNG" style="border: 1px solid black" width="95%" />
					</center>
				</li>
				<li>在这里你可以检查现在储存在系统里的你的资料。如果需要修改，点击“Edit”（修改）。<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/12c.PNG" style="border: 1px solid black;" width="95%" />
					</center>
				</li>
				<li>你可以在此页修改你的个人资料，如果有修改日期的疑问，可以点<a href="<%= Url.Action("ChangeDate", "Help") %>">这里</a>。<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/13c.png" border="1px solid black" />
					</center>
				</li>
				<li>局部翻译：
					<ul>
						<li>国籍：Singaporean = 新加坡人，Chinese = 中国人</li>
						<li>性别：Female = 女性，Male = 男性</li>
						<li>种族：Chinese = 华族</li>
						<li>雇用情况：Employed = 就业，Umemployed = 失业，Retired = 退休，Housewife = 家庭主妇，Student = 
							学生，Others = 其他</li>
						<li>月份（出生日期和印心日期）：Jan = 一月，Feb = 二月，Mar = 三月，Apr = 四月，May = 五月，Jun = 六月，Jul = 七月，Aug 
							= 八月，Sep = 九月，Oct = 十月，Nov = 十一月，Dec = 十二月。</li>
&nbsp;</ul>
				</li>
				<li>点击“Save”（保存）将会保存你刚刚输入的资料。</li>
			</ol>
			<p align="center" style="font-size: large; padding: 0.3em;">
				<a href="<%= Url.Action("Index", "Help") %>">Back to FAQ Index</a></p>
		</div>
	</div>
</asp:Content>

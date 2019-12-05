<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FAQ: How to Make Orders
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<link rel="stylesheet" type="text/css" href="../../Content/Basic.css" title="style1">
	<div id="body">
		<div class="fullwidth">
			<p align="center" style="font-size: x-large; padding: 0.3em;">
				How to Make Orders
			</p>
			<ol class="helpfile">
				<li>After logging in, click the Order icon in the top menu.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/11.0.PNG" style="border: 1px solid black" width="95%" /><br />
					</center>
				</li>
				<li>There will be a few different categories of products to order. Click on the 
					&quot;Order&quot; button of the category you want. If you want to order items of different 
					categories, you will create a different order for each category.<br />
					<br />
					<center>
						<img border="1px solid black" src="../../images/FAQscreenshots/11.1.PNG" width="95%" /></center>
				</li>
				<li>Browse through this list of products, or use the search function at the top of 
					the page. To view the details of a product or make an order, click on its name, 
					as shown on the screenshot.<br />
					<br />
					<center>
						<img src="../../images/FAQscreenshots/11.2.png" style="border: 1px solid black" width="95%" />
					</center>
				</li>
				<li>Here you will be able to see the discounted price (if any), a larger image of 
					the product (if any), and the avaliable sizes.<br />
					<br />
					To make an order of this product:<ol>
						<li>Click the &quot;Quantity&quot; text box next to the size you want.</li>
						<li>Type the quantity you want into the box.</li>
						<li>Click the &quot;Add To Order Bag&quot; button.</li>
					</ol>
					<center>
						<img src="../../images/FAQscreenshots/11.3.png" style="border: 1px solid black" width="95%" />
					</center>
				</li>
				<li>You will be taken to your Order Bag page. <br />
					<br />
					From this page, you can:<ul>
						<li>Click the &quot;More Order&quot; button to order more items. You will be taken to the 
							screen in step 3.</li>
						<li>Click the &quot;Remove&quot; link next to an ordered product to remove it from your order 
							bag.</li>
						<li>Click the &quot;Edit&quot; link next to an ordered product, to go back to the details 
							screen of the product, and change the quantity and/or size.</li>
						<li>Click the &quot;Cancel Order&quot; button to remove your order completely.</li>
					</ul>
					<center>
						<img src="../../images/FAQscreenshots/11.4.png" style="border: 1px solid black" width="95%" />
					</center>
				</li>
			</ol>
			<p align="center" style="font-size: large; padding: 0.3em;">
				<a href="<%= Url.Action("Index", "Help") %>">Back to FAQ Index</a>
			</p>
		</div>
	</div>
</asp:Content>

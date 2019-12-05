<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	PlayAll
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

<div style="text-align:center; padding-top:10px; font-size:1.3em; color:#307815; font-weight:700; font-family:Bookman Old Style">
	<%= (string)ViewData["ChannelTitle"] %>
</div>
 
<div style="text-align:center; padding-top:10px; padding-bottom:10px; z-index:0">
<% string playListID = (string)ViewData["PlayList"]; %>
	<iframe width="700" height="440" src="http://www.youtube.com/embed/videoseries?list=<%: playListID%>&amp;hl=en_US&amp;hd=1&amp;autoplay=1&amp;wmode=opaque" frameborder="0" allowfullscreen></iframe>
</div>
         
</div>

</asp:Content>

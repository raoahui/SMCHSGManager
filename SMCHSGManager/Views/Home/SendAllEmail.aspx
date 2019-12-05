<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SendAllEmail
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
<script src="/Scripts/MicrosoftAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>    

<script type="text/javascript">

    function AnimateRSVPMessage() {
        $("#rsvpmsg").animate({ fontSize: "1.5em" }, 400);
    }

</script>
    
<div id="body" align="center">    

  <h5>Send Email to All?</h5>

   <div id="rsvpmsg">
      <%: Ajax.ActionLink("Confirm", "SendAllEmail", null, new AjaxOptions { UpdateTargetId = "rsvpmsg", OnSuccess = "AnimateRSVPMessage" }, new { @style = "color:white;", @class = "buttonsearch" })%>         
   </div>  

   <br />
  <%: Html.ActionLink("Click Here", "Index",  "Home")%>
             to return to the Home Page.
</div>

</asp:Content>

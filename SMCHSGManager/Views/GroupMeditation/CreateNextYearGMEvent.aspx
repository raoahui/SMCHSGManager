<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.GroupMeditation>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	CreateGMEventYearly
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <script src="/Scripts/MicrosoftAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>    

<script type="text/javascript">

    function AnimateRSVPMessage() {
        $("#rsvpmsg").animate({ fontSize: "1.5em" }, 400);
    }

</script>

<div id="body">
    <h2>CreateGMEventYearly</h2>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</div>
</asp:Content>



<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.InternationalGMApplicationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
  
<% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

    <%: Html.EditorFor(model => model.InternationalGMApplicationInfo, 
    										new { Mode = "Create", 
													 AshramAndCenterInfos = Model.AshramAndCenterInfos, 
													 MeetRequire = (bool)ViewData["MeetRequire"]})%>
 
	<div class="actionbuttons">
                    <input type="submit"  value="Next"  name="Next" class ="buttonsmall" /> &nbsp;
    </div>

 <% } %>  

</div>

</asp:Content>

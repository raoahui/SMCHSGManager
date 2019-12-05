<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.InternationalGMApplicationTransportInfo>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SelectAshram
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">      

        <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
		<h6> Please confirm your transportation </h6>
			<%: Html.EditorFor(model => model, "InternationalGMApplicationTransportInfo", new { InternationalTransports = ViewData["InternationalTransports"] })%>

			 <div class="actionbuttons">
                   <input type="submit"  value="Submit"  name="Save" class ="buttonsmall" /> &nbsp;
            </div>	

   <%} %>


</div>
 
</asp:Content>


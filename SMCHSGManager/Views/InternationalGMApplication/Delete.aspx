<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.InternationalGMApplicationInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	ApplicationInternationGMInfo Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

   <h5>Delete Confirmation</h5>

        <div align="center">
            <h4>Are you sure you want to delete your International GM Application to <%: Model.AshramAndCenterInfo.Name %>(time from :   
             <strong><%: String.Format("{0:d-MMM-yyyy}", Model.ArrivalDate)%> to   <%: String.Format("{0:d-MMM-yyyy}", Model.DepartureDate)%></strong>) ?
            </h4>
            </br>

             <% using (Html.BeginForm()) { %>
                <div class="editbuttons">
		            <input type="submit" value="Delete"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Details", new { id = Model.ID }, new { @style = "color:white;", @class = "buttonsmall" })%>
                </div>
             <% } %>
        </div>
        
</div>

</asp:Content>

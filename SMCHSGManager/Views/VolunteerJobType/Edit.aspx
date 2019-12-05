<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.VolunteerJobType>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="body">
   <h5>Edit Volunteer Job</h5>
    <div class="centerblock">

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
           
                <%: Html.EditorFor(model => model.Name) %>
                <%: Html.EditorFor(model => model.Remark) %>
            
            <p>
                <input type="submit" value="Save"  class ="buttonsmall"/>
            </p>
  
    <% } %>

        <%: Html.ActionLink("Back to List", "Index") %>
    </div>
</div>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.EventPrice>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
    <h5>Edit</h5>

     <div class="centerblock">

     <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

            <div class="editor-label">
                <%: Html.DisplayFor(model => model.EventActivity.Name) %>
             </div>
            </br>

             <div class="editor-label">
                <%: Html.LabelFor(model => model.UnitPrice) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.UnitPrice)%>
                <%: Html.ValidationMessageFor(model => model.UnitPrice)%>
            </div>

               <div class="editbuttons" align="center">
		            <input type="submit" value="Save"  class ="buttonsmall"/>  &nbsp;&nbsp;&nbsp;&nbsp;
                    <%: Html.ActionLink("Cancel", "Index", new { eventID = Model.EventID }, new{ @style = "color:white;", @class = "buttonsmall" })%>
             </div>
         

    <% } %>
   
     </div>
</div>

</asp:Content>


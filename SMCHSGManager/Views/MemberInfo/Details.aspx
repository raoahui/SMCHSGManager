<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.PublicMemberInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

   <h5>Member Information Details</h5>
      
      <div class="fullwidth">    
          
        <div class="listitem">

			<%: Html.DisplayFor(model => model)%>   
  
        </div>
    </div>
 
</div>
  <%--  <p>
         <%: Html.ActionLink("Back to List", "Index") %>
    </p>--%>

    <div class="editbuttons" align="center">
        <%: Html.ActionLink("Back", "Index", new { @style = "color:white;", @class = "buttonsmall" })%>
       <%-- <%: Html.ActionLink("Delete", "Delete", "OrdinaryMember", new { id = Model.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> --%>
    </div>


</asp:Content>


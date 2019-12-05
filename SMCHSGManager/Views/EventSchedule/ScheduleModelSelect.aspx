<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ScheduleModelSelect
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">
   <div class="fullwidth">     

    <h5>Local Retreat Schedule Model Select</h5>

   <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm())
       {%>
         <%: Html.ValidationSummary(true)%>
        <p align="center">
             <%: Html.DropDownList("LocalRetreatScheduleModel", (IEnumerable<SelectListItem>)ViewData["LocalRetreatScheduleModelSelectLists"])%>
        </p>

        <p align="center">
             <input type="submit"  value="Confirm" class ="buttonsmall" />
         </p>
    <%} %>

    </div>
</div>
</asp:Content>

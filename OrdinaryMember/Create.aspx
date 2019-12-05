<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.OrdinaryMemberViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="body">

   <h5>New Ordinary Member Information</h5>
 
  <div class="fullwidth">         
  
    <div class="listitem" >
 
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm("Create", "OrdinaryMember", FormMethod.Post, new { enctype = "multipart/form-data" })){%>
        <%: Html.ValidationSummary(true) %>
        
  	        <%: Html.EditorFor(model => model.PublicMemberInfo,  new {   
                                    Genders = Model.Gender,
									InitiateTypes = Model.InitiateType,
									Mode = "Create" 
                                     })%>   

            <%: Html.EditorFor(model => model.OrdinaryMemberInfo,  new {   
                                    Nationalities = Model.Nationality, 
                                    Races = Model.Race, 
                                    EmploymentStatuses = Model.EmploymentStatus, 
                                    PayMethods = Model.PayMethod,
										 
                                    })%>
  
            <div align="center" style=" margin-bottom:20px; margin-top:20px">
                  <%: Html.ActionLink("Back", "Index", "OrdinaryMember", null, new { @style = "color:white;", @class = "buttonsmall" })%> 
                 <input type="submit" value="Create" class ="buttonsmall"/>
          </div>
 
    <% } %>

        </div>
    </div>   
</div>

</asp:Content>


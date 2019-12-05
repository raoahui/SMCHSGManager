<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.MemberInfoShortListViewModel>>" %>
<%@ Import Namespace = "SMCHSGManager.ViewModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	 Member Signature List (Paged)	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="body">
    <h6>Time: <%: ViewData["Now"] %></h6>
      <%
          if (Model.Count() == 0 && ViewData["searchContent"] == null)
          {
            %> <h5>There is no event need to signature currently.</h5> <%
          }
          else
          {%>

  
   <div align="center" style=" margin-top:20px; ">
       
    <h2>Signature Name List </h2> <h4><%: ViewData["Title"]%> </h4>
    <br />
        
   </div>
   
         <% using (Html.BeginForm())
            {%>
            <div class="actionbuttons">
 
               Name:  <%: Html.TextBox("searchContent")%>  
               <input type="submit" value="Search" class ="buttonsearch" name="Search" />
   
           </div>
            
              <%=Html.DataGrid<MemberInfoShortListViewModel>()%>
   
    <% } %>

    <div align="center"  style="margin-top:10px" > 

             <%=Html.PageLink((int)ViewData["CurrentPage"], (int)ViewData["TotalPages"], p => Url.Action("SignatureList4GMAndLocalRetreat", new { page = p, searchContent = ViewData["searchContent"], eventTypeID = (int)ViewData["EventTypeID"], eventID = (int)ViewData["EventID"]}))%>
 
    </div>

 <%} %>
</div>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.EventHistoryListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Event History
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
    <h5><%: ViewData["EventTypeName"] %> History</h5>
    
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
      <%: Html.ValidationSummary(true) %>

	  <%  int eventTypeID = (int)ViewData["EventTypeID"];
	   if (eventTypeID == 2)
	   { %>
          <div align="center">
                              
                                    Start : <%: Html.EditorFor(model=>model.StartDate, "Date")%>    &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                    End :   <%: Html.EditorFor(model=>model.EndDate, "Date")%>      &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                <input type="submit" value="Search"   class ="buttonsearch"/>
          </div>
	<%}%>

    <table>
        <tr>
            <th>
                No
           </th>
           <th>
                Name 姓名
           </th>
          
           <th>
                ID Card No </br>识别证编号
           </th>
           
         <%--  <th>
                IC/Passport No </br>身份证或护照编号
           </th>--%>

          <th> Times attended <%: ViewData["EventTypeName"] %>
          </th>
       
        </tr>
        
    <% int i = 1;
        foreach (var item in Model.MemberRegisterList) { %>
    
        <tr>
           
            <td> 
                  <%: (i++).ToString() %> 
           </td>

            <td >
			<% 
	   if (eventTypeID == 1)
	   {%>
               <%:  Html.ActionLink(item.Name, "MyEvent", new { memberID = item.ID, startDate = Model.StartDate, endDate = Model.EndDate }, null)%>
			   <%}
	   else if (eventTypeID == 2 || eventTypeID == 3)
	   { %>
                <%:  Html.ActionLink(item.Name, "MyGroupMeditation", "GroupMeditation", new { memberID = item.ID, startDate = Model.StartDate, endDate = Model.EndDate }, null)%>
				<%} %>
          </td>
          
           <td >
                <%: item.IDCardNo%>
           </td>

      <%--     <td >
                <%: item.ICOrPassportNo%>
           </td>--%>
        
           <td><%: item.Count %>
           </td>
      
        </tr>
    
    <% } %>

    </table>
  <% } %>
  
    <div align="center" style="margin-bottom:20px; margin-top:20px">
      <%: Html.ActionLink("Click here", "Index", "Home")%> to return to Home Page.
         <%-- <a href="javascript:history.go(-1)" style = "color:white;" class="buttonsmall" >Back</a>--%>
    </div>

</div>

</asp:Content>

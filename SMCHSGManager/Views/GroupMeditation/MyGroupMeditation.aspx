<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.GroupMeditationAttendance>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
SMCH Association Singapore - 	My Group Meditation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

<% if (Model.Count() == 0) {%> 
            <h5> You don't have any past Group Meditation .</h5> 
    <%}else{%>
            <h5>You have attended Group Meditation List</h5>

     <h6>Name 姓名: <%: Model.FirstOrDefault().MemberInfo.Name%></h6>
     <h6>ID Card No 识别证编号 : <%: Model.FirstOrDefault().MemberInfo.IDCardNo%></h6>
    <table>
        <tr>
           <th>No</th>
        
          <th>Date</th>
           <th>Time</th>
   
           <th>Volunteer Job</th>
      
        </tr>
        
    <% int i = 0;
       foreach (var item in Model)
       { %>
    
        <tr>
             <td><%: (++i).ToString()%></td>
       
          <td> <%: String.Format("{0:ddd, MMM d yyyy}", item.GroupMeditation.StartDateTime)%></td>
           <td> <%: String.Format("{0:HH:mm}", item.GroupMeditation.StartDateTime) + " ~ " + String.Format("{0:HH:mm}", item.GroupMeditation.EndDateTime) %></td>
        

           <td nowrap="nowrap" >
               <% if(item.GroupMeditation.DPMemberID == item.MemberID)
                    { %>
                        DP
						<%}else if(item.GroupMeditation.AudioMemberID == item.MemberID) 
				  {%>
                        Audio
                    <%}%>
          </td>
       
       </tr>
    
    <% } %>

    </table>

<%} %>

</div>

    <div align="center" style="margin-bottom:20px; margin-top:20px">
        <%: Html.ActionLink("Click here", "Index", "Home")%> to return to Home Page.
       <%--   <a href="javascript:history.go(-1)" style = "color:white;" class="buttonsmall" >Back</a>--%>
    </div>

  <%--  <h2>MyGroupMeditation</h2>

    <table>
        <tr>
            <th></th>
            <th>
                ID
            </th>
            <th>
                GroupMeditationID
            </th>
            <th>
                MemberID
            </th>
            <th>
                CheckInTime
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.ID }) %> |
                <%: Html.ActionLink("Details", "Details", new { id=item.ID })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id=item.ID })%>
            </td>
            <td>
                <%: item.ID %>
            </td>
            <td>
                <%: item.GroupMeditationID %>
            </td>
            <td>
                <%: item.MemberID %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.CheckInTime) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>
--%>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.EventRegistration>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
		SMCH Association Singapore - 	Event Registration List By Event
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="body">
 
   <h5><%:Model.FirstOrDefault().Event.Title  + " (" + Model.FirstOrDefault().Event.EventType.Name + ')'%> Registration List</h5>
   <h6>Start at <%= Html.DisplayFor(model=>model.FirstOrDefault().Event.StartDateTime) %>, end at<%= Html.DisplayFor(model => model.FirstOrDefault().Event.EndDateTime)%> </h6>
    
    <table>
        <tr>
           <th>No</th>
           <th>Name</th>
           <th>Member No.</th>
           <th>IDCard No</th>
           <th>Register Time</th>
           <th>Check-in Time</th>
        </tr>
        
    <% int i = 0;
        foreach (var item in Model) { %>
    
        <tr>
 
             <td><%: (++i).ToString()%></td>

            <td nowrap="nowrap">
                <%: item.MemberInfo.Name%>
            </td>
            
            <td nowrap="nowrap">
                <%: item.MemberInfo.MemberNo %>
            </td>

              <td nowrap="nowrap">
                <%: item.MemberInfo.IDCardNo%>
            </td>

             <td nowrap="nowrap">
             <% if(item.RegisterTime.HasValue){ %>
                <%: string.Format("{0:d MMM yyyy HH:mm}", item.RegisterTime)%>
                <%} %>
            </td>
 
              <td nowrap="nowrap">
              <% if(item.SignTime.HasValue){ %>
                <%: string.Format("{0:d MMM yyyy HH:mm}", item.SignTime)%>
                <%} %>
            </td>
     
        </tr>
    
    <% } %>

    </table>

</div>

</asp:Content>


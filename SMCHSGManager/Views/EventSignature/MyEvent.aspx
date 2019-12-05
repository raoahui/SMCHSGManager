<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.ViewModel.EventRegistrationViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
		SMCH Association Singapore - 	Event Registration List By Name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="body">

<% if ((int)ViewData["LocalRetreatRegistrationCount"] == 0) {%> 
            <h5> You don't have any past event .</h5> 
    <%}else{%>
            <h5>Your <%: Model.FirstOrDefault().EventRegistration.Event.EventType.Name %> List</h5>

     <h6>Name 姓名: <%: Model.FirstOrDefault().EventRegistration.MemberInfo.Name%></h6>
     <h6>ID Card No 识别证编号 : <%: Model.FirstOrDefault().EventRegistration.MemberInfo.IDCardNo%></h6>
    <table>
        <tr>
           <th>No</th>
           <% if (Model.FirstOrDefault().EventRegistration.Event.EventTypeID == 1)
              { %>
           <th>Title</th>
           <th>Duration</th>
           <%}
              else if (Model.FirstOrDefault().EventRegistration.Event.EventTypeID == 2)
              { %>
           <th>Date</th>
           <th>Time</th>
           <%} %>

           <th>Volunteer Job</th>
            
            <% if (Model.FirstOrDefault().EventRegistration.Event.EventTypeID == 1)
               { %>
			<th>Meals</th>
			<th>Remark</th>
			<th>RegisterTime</th>
			<th>SignTime</th>
          <%} %>

        </tr>
        
    <% int i = 0;
       foreach (var item in Model)
       { %>
    
        <tr>
             <td><%: (++i).ToString()%></td>

           <% if (Model.FirstOrDefault().EventRegistration.Event.EventTypeID == 1)
              { %>
                <td nowrap="nowrap"> <%: item.EventRegistration.Event.Title%> </td>
            
             <td >
                From:  <%: String.Format("{0:MMM d yyyy HH:mm}", item.EventRegistration.Event.StartDateTime)%></br>
                To:  <%: String.Format("{0:MMM d yyyy HH:mm}", item.EventRegistration.Event.EndDateTime)%>
            </td>
               <%}
              else if (Model.FirstOrDefault().EventRegistration.Event.EventTypeID == 2)
              { %>
          <td> <%: String.Format("{0:ddd, MMM d yyyy}", item.EventRegistration.Event.StartDateTime)%></td>
           <td> <%: String.Format("{0:HH:mm}", item.EventRegistration.Event.StartDateTime) + " ~ " + String.Format("{0:HH:mm}", item.EventRegistration.Event.EndDateTime) %></td>
           <%} %>

           <td nowrap="nowrap" >
               <%   int j = 0;
                    foreach (string s in item.EventVolunteerJobBookingLabels)
                    {
                        string s1 = item.EventVolunteerJobBookingValues.ElementAt(j++); %>
                        <%: s%> : <%: s1%>
                        <% if (j < item.EventVolunteerJobBookingLabels.Count())
                            {%> </br><%}
                    }%>
          </td>

           <% if (Model.FirstOrDefault().EventRegistration.Event.EventTypeID == 1)
              { %>
          <td nowrap="nowrap" >
               <%   int k = 0;
                    foreach (string s in item.LocalRetreatMealBookingLabels)
                    {
                        string s1 = item.LocalRetreatMealBookingValues.ElementAt(k++);
                        string s2 = s1.Remove(s1.IndexOf('('));%>
                        <%: s%> : <%: s2%>
                        <% if (k < item.LocalRetreatMealBookingLabels.Count())
                           {%> </br> <%}
                    }%>
          </td>

         <td nowrap="nowrap" >
             <%if (item.EventRegistration.BackDateTime != null && DateTime.Compare((DateTime)item.EventRegistration.BackDateTime, item.EventRegistration.Event.EndDateTime) < 0)
               { %>
                 <%: Html.DisplayFor(LocalRetreatRegistrationMetaDate => item.EventRegistration.BackDateTime)%>
                 <%} %>
            </td>

			<td><%: String.Format("{0:MMM d yyyy HH:mm}", item.EventRegistration.RegisterTime)%></td>
			<td><%: String.Format("{0:MMM d yyyy HH:mm}", item.EventRegistration.SignTime)%></td>
      <%} %>

        </tr>
    
    <% } %>

    </table>

<%} %>

</div>



    <div align="center" style="margin-bottom:20px; margin-top:20px">
        <%: Html.ActionLink("Click here", "Index", "Home")%> to return to Home Page.
       <%--   <a href="javascript:history.go(-1)" style = "color:white;" class="buttonsmall" >Back</a>--%>
    </div>



</asp:Content>

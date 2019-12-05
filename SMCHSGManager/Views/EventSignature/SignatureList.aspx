<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<List<SMCHSGManager.ViewModel.MemberInfoShortListViewModel>>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Signature Event List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Scripts/MicrosoftAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>    

<script type="text/javascript">

    function AnimateRSVPMessage() {
        $("#dialog").animate({ fontSize: "1.5em" }, 400);
    }   

    $().ready(function () {
        $('#dialog').jqm();

        $('#jqmOpen').click(function () {
            $('#dialog').jqmShow();
            return false;
        });
    });

</script>

<div id="body">
    <%
        List<SMCHSGManager.Models.Event> eventList = (List<SMCHSGManager.Models.Event>)(ViewData["EventTypeID"]);
        if (eventList.Count() == 0)
        {
            %> <h5>There is no event need to signature currently.</h5> <%
        }
        else
        {%>

          <%  using (Html.BeginForm())
            {%>
            <div class="actionbuttons">
                <%: Html.TextBox("searchContent")%>  
                <input type="submit" value="Search"  class ="buttonsearch" name="Search" />
          </div>
    <% }

            int index = 0;
            foreach (var signatureList in Model)
            {
          %>
             <%if (signatureList.Count() > 0)
               {%>

             <h5><%: eventList[index].Title%>   ( <%: eventList[index].EventType.Name%> ) Signature List </h5>
             <h6>Start at <%= string.Format("{0:d MMM yyyy HH:mm}", eventList[index].StartDateTime)%>, end at <%=string.Format("{0:d MMM yyyy HH:mm}", eventList[index].EndDateTime)%> 共修</h6>
  <table>
        <tr>
          <%--  <th rowspan="2"></th>--%>
            <%if (eventList[index].EventTypeID == 1 || eventList[index].EventTypeID == 2)
              {%>
              <th colspan="3" >
                  Member Info
            </th>
            <%} %>

            <th rowspan="2">
                Name
            </th>
         
            <%if (eventList[index].EventTypeID == 1 || eventList[index].EventTypeID == 2)
              {%>
            <th rowspan="2">
                IDCardNo
            </th>
            <th rowspan="2">
                ICOrPassportNo
            </th>
            <%} %>
            <th rowspan="2">
                Signature
            </th>

            <%if (eventList[index].EventTypeID == 1)
              {%>
            <th rowspan="2">
                The Fee need to pay
            </th>
             <th rowspan="2">
                Name
            </th>
            <%} %>

         </tr>

       <%if (eventList[index].EventTypeID == 1 || eventList[index].EventTypeID == 2)
         {%>       
          <tr>
          <th>Type</th>
          <th>No
          </th>
          <th>Fee Expired Date</th>
          </tr>
          <%} %>

    <% int i = 1;
       foreach (var item in signatureList)
       { %>
    
        <tr>
           <%-- <td>
               <%: (i++).ToString()%>
            </td>--%>
              <%if (eventList[index].EventTypeID == 1 || eventList[index].EventTypeID == 2)
                {%>
            <td>
                  <%: item.MemberType%>
            </td>
            <td>
            <%if (item.MemberNo != null)
              { %>
                  <%: item.MemberNo.Value%>
                  <%} %>
            </td>
            <td>
               <%if (item.MemberNo != null)
                 { %>
                  <%: item.MemberFeeExpiredDate.Value%>
                  <%} %>
            </td>
            <%} %>

           <td>
                <%: item.Name%>
            </td>
             <%if (eventList[index].EventTypeID == 1 || eventList[index].EventTypeID == 2)
               {%>
             <td>
                <%: item.IDCardNo%>
            </td>
            <%--<td>
                <%: item.ICOrPassportNo%>
            </td>--%>
             <%} %>

              <td>
                  <%: Html.ActionLink("Please click to sign", "Signature", new { ID = item.ID, eventID = eventList[index].ID })%>         
              </td>

            <%if (eventList[index].EventTypeID == 1)
              {%>
            <td>
                <%: string.Format("SGD{0:C}", item.Money)%>
            </td>
             <td>
                <%: item.Name%>
            </td>
           <%} %>

        </tr>
    
    <% } %>

    </table>
     
            <%} %>
   
   <% index++;
            } %>
   <%} %>

</div>

</asp:Content>

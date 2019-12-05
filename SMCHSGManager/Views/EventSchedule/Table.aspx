<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.EventSchedule>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Local Retreat Table
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="body">
   <h5><%: Model.FirstOrDefault().Event.Title + ' ' + Model.FirstOrDefault().Event.EventType.Name%>  Schedule </h5>
   
 <table style="font-size: 14px;">
        <tr>
            <th></th>
            <th>
                Local Retreat Activity
            </th>
            <th>
                From
            </th>
            <th>
                To
            </th>
<%--            <th>
                Remark
            </th>--%>
        </tr>

    <% int i=0;
       foreach (var item in Model)
       {
           if (!item.EventActivity.Name.StartsWith("Bless"))
           {%>
    
        <tr>
             <td><%: (++i).ToString()%></td>
            <td>
                <%: item.EventActivity.Name%>
            </td>
			<% if (i == 1){ %>
           <td colspan =2 align=center>
                <%= Html.DisplayFor(EventScheduleMetaDate => item.DateTimeFrom)%>
           </td>
		   <%}else{ %>
            <td>
                <%= Html.DisplayFor(EventScheduleMetaDate => item.DateTimeFrom)%>
           </td>
            <td>
            <% DateTime DateTimeTo = item.DateTimeFrom.AddHours(item.ScheduleOffset.OffsetHours); %>
                <%= Html.Encode(String.Format("{0:d MMM yyyy HH:mm }", DateTimeTo))%> 
            </td>
			<%} %>
        </tr>

		<% if (i == 2)
		 { %> 
		 <tr>
             <td><%: (++i).ToString()%></td>
            <td style="color:Navy"><strong> Door Closes</strong>
            </td>
            <td colspan =2 align=center style="color:Navy">
			  <% DateTime DateTimeTo = item.DateTimeFrom.AddHours(item.ScheduleOffset.OffsetHours); %>
                <strong> <%= Html.Encode(String.Format("{0:d MMM yyyy HH:mm }", DateTimeTo))%> </strong>
           </td>
        </tr>

		<%} %>

    <% }
       }%>

    </table>

	  <div style="margin-left: 20%; margin-right: 20%; margin-top: 20px; font-size:medium; font-weight:500"><font color="red">*</font>Note:  
	 <div style="margin-bottom: 8px; margin-top:8px">1. Participants are encouraged to volunteer themselves as Dharma Protector, Cleaner or Operator of Audio/Video Equipment.</div>
	  <div style="margin-bottom: 3px; margin-top:8px">2. Initiates are to sign-up even if only attending the 1st session.</div>
	  <div style="margin-bottom: 3px; margin-top:8px">3. Half-initiates are NOT allowed to join the Retreat.</div>
	  <div style="margin-bottom: 3px; margin-top:8px">4. Initiates who are sick are NOT allowed to join the Retreat.</div>
	 </div>

    <div align="center" style="margin-bottom:20px; margin-top:20px">
          <a href="javascript:history.go(-1)" style = "color:white;" class="buttonsmall" >Back</a>
    </div>

</div>
	</div>
</asp:Content>


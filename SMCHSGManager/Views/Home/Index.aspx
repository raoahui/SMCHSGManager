<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.HomeViewModel>" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - Home Page</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="body">
		<!-- Left column   -->
		<div id="columnleft">
			<div class="leftblock">
				<table style="border: 0;">
					<tr>
						<td style="border: 0; width: 174px; text-align: center">
							<a href="http://www.suprememastertv.com" target="_blank">
								<img src="../images/UrgentMsgImage.png" align="" height="221" width="160">
							</a>
						</td>
					</tr>
					<%--<tr>
						<td style="border: 0; width: 174px; text-align: center">
							<a href="http://classic.godsdirectcontact.org.tw/eng/news/208/index.htm" target="_blank">
								<b class="next">News Magazine</b></a>
						</td>
					</tr>--%>
				</table>
				<%if (!Request.IsAuthenticated)
	  {%>
				<p style="font-size: 1em; font-weight: bold">
					We are open to the public between 1:30pm to 6:00pm on Sundays.
				</p>
				<%--     <p> Please use "Login" button on the top right to login.</p>--%>
				<%}
	  else
	  {%>
				<%-- <div class="dashedline"> </div>--%>
				<h4>
					<script type="text/javascript" language="JavaScript1.2">

					    /*
					    Neon Lights Text
					    By JavaScript Kit (http://javascriptkit.com)
					    For this script, TOS, and 100s more DHTML scripts,
					    Visit http://www.dynamicdrive.com
					    */

					    var message = "For your account's safety, if this is the first time you have logged in, click here:"
					    var neonbasecolor = "gray"
					    var neontextcolor = "green"
					    var flashspeed = 70  //in milliseconds

					    ///No need to edit below this line/////

					    var n = 0
					    if (document.all || document.getElementById) {
					        document.write('<font color="' + neonbasecolor + '">')
					        for (m = 0; m < message.length; m++)
					            document.write('<span id="neonlight' + m + '">' + message.charAt(m) + '</span>')
					        document.write('</font>')
					    }
					    else
					        document.write(message)

					    function crossref(number) {
					        var crossobj = document.all ? eval("document.all.neonlight" + number) : document.getElementById("neonlight" + number)
					        return crossobj
					    }

					    function neon() {

					        //Change all letters to base color
					        if (n == 0) {
					            for (m = 0; m < message.length; m++)
					            //eval("document.all.neonlight"+m).style.color=neonbasecolor
					                crossref(m).style.color = neonbasecolor
					        }

					        //cycle through and change individual letters to neon color
					        crossref(n).style.color = neontextcolor

					        if (n < message.length - 1)
					            n++
					        else {
					            n = 0
					            clearInterval(flashing)
					            setTimeout("beginneon()", 1500)
					            return
					        }
					    }

					    function beginneon() {
					        if (document.all || document.getElementById)
					            flashing = setInterval("neon()", flashspeed)
					    }
					    beginneon()


					</script>
				</h4>
				<font style="color: Green; font-size: small; font-weight: bold">
					<%: Html.ActionLink("Change Your Password", "ChangePassword", "Account")%>
				</font>
				<%} %>
				<%-- <div class="dashedline"> </div>--%>
			</div>
			<div class="leftblock">
				<table style="border: 0;">
					<tr>
						<td style="border: 0; width: 174px; text-align: center">
							<script type="text/javascript" src="../../Scripts/triptracker_slide.js"></script>
							<script type="text/javascript">
                    <!--
							    var viewer = new PhotoViewer();
							    viewer.add('../../images/Fliers/veg_benefits_eng.png');
							    viewer.add('../../images/Fliers/notable_veg_eng.png');
							    viewer.add('../../images/Fliers/veg_benefits_cn.png');
							    viewer.add('../../images/Fliers/notable_veg_cn.png');
                    //--></script>
							<a href="javascript:void(viewer.show(0))">
								<img src="../../images/Fliers/veg_benefits_eng.png" alt="Vegetarian Benefits Fliers"
									height="221" width="160"/>
							</a>
						</td>
					</tr>
					<tr>
						<td style="border: 0; width: 174px; text-align: center">
							<a href="../../images/Fliers/print_quality.zip" target="_blank"><b class="next">Vegetarian
								Benefits Fliers - Click here to download original resolution files</b></a>
						</td>
					</tr>
				</table>
			</div>
			<% if (Page.User.Identity.Name == "rao hui")
	  {%>
			<div class="leftblock">
				<%: Html.ActionLink("mass-email-send-out", "SendAllEmail", "Home", null, new { @style = "font-weight:bolder;" })%>
			</div>
			<%}%>
		</div>
		<!-- right column-->
		<div id="columnright">
            <% if (Model.GroupMeditation != null) {
                var aEvent = Model.GroupMeditation;
                string dString = String.Format("{0:ddd, d MMM yyyy  }", aEvent.StartDateTime) + String.Format("{0:HH:mm}", aEvent.StartDateTime) + " to " + String.Format("{0:HH:mm}", aEvent.EndDateTime) + ' ' + "Group Meditation"; %>
			<div class="rightblock" align="center">
				<h2>Group Meditation Registration </h2> 
                <div class="dashedlineIndex"></div>
                <h4><%: dString%> </h4>
                <% if (Model.GroupMeditationAttendances.Count > 0)
                   { %>
                   <br />
                    <table>
                        <tr>
                            <th></th>
                            <th>Member No</th>
                            <th>Name</th>
                            <th>IDCard Number</th>
                            <th>Checking Time</th>
                        </tr>
                        <%  int ii = 1;
                            foreach (var item in Model.GroupMeditationAttendances)
                            { %>
                        <tr>
                            <td><%: (ii++).ToString()%></td>
                            <td align="left"><%: item.MemberInfo.MemberNo.ToString()%></td>
                            <td align="left"><%: item.MemberInfo.Name%></td>
                            <td><%: item.MemberInfo.IDCardNo%></td>
                            <td><%: string.Format("{0:d MMM yyyy HH:mm}", item.CheckInTime)%></td>
                        </tr>
                        <% } %>
                    </table>
                <% } %>
                <br />
                <div class="dashedlineIndex"></div>
                 <%  if (User.IsInRole("Administrator"))
                     {%>
                    <%: Html.ActionLink("Register", "AddNewAttend", "GroupMeditation", new { GMID = aEvent.ID, checkTime = DateTime.Now }, new { @style = "color:white;", @class = "buttonsearch" })%> 
                 <%} %>
	        </div>
		    <%}
               else
               { %>

			<div class="rightblock">
				<h2>
					Recent Announcements</h2>
				<% int i = 0;
       foreach (var item in Model.Announcements)
       { %>
				<div class="rightlistitem">
					<div class="dashedlineIndex">
					</div>
					<div class="thumbnail">
						<%--  <%: Html.ActionLink(" ", "Details", new { id = item.ID })%> --%>
						<% if (!string.IsNullOrEmpty(Model.AnnouncementImages[i]))
         { %>
						<img src="<%: Model.AnnouncementImages[i] %>" height="50" alt="" />
						<%} %>
					</div>
					<p>
						<%: String.Format("{0:ddd, MMM d yyyy}", item.AnnounceDate)%>
						<%  if (!string.IsNullOrEmpty(item.StaticURL))
          {%>
						<a style="font-weight: bolder" href="<%: item.StaticURL %>">
							<%: item.Name%></a>
						<%}
          else
          { %>
						<%: Html.ActionLink(item.Name, "Details", "Announcement", new { id = item.ID }, new { @style = "font-weight:bolder;" })%>
						<%} %>
					</p>
					<% string des = SMCHSGManager.Models.GeneralFunction.RemoveHtmlFormat(item.Description); %>
					<p>
						<%: Html.Truncate(des, 40)%>
						<%: Html.ActionLink("read more", "Details", "Announcement", new { id = item.ID }, null)%>
						&raquo
					</p>
					<div class="clearlist">
					</div>
				</div>
				<% i++;
       } %>
				<div class="dashedline">
				</div>
				<%: Html.ActionLink("Read all Announcements", "Index", "Announcement")%>
				&raquo;
			</div>
			<div class="rightblock">
				<h2>
					Upcoming Events</h2>
				<div class="dashedline">
				</div>
				<% 
                if (Model.UpcomingEvents.Count() > 0)
                {
				%>
				<table style="border: 0; width: 100%">
					<% int itemNo = 0;
        foreach (var item in Model.UpcomingEvents)
        {
            itemNo++;
            string eventTitle = item.Title;
            if (item.EventTypeID == 1)
            {
                eventTitle += ' ' + item.EventType.Name;
            } 
					%>
					<tr>
						<td class="eventview">
							<%: Html.ActionLink(eventTitle, "Details", "Event", new { id = item.ID }, new { @style = "font-weight:bolder;" })%>
						</td>
						<td class="eventview" align="right">
							From
							<%: String.Format("{0:ddd, MMM d yyyy HH:mm}", item.StartDateTime)%>
							to
							<%: String.Format("{0:ddd, MMM d yyyy HH:mm}", item.EndDateTime)%>
						</td>
					</tr>
					<%if (DateTime.Compare(item.RegistrationOpenDate, DateTime.Today.ToUniversalTime().AddHours(8)) <= 0 && DateTime.Compare(DateTime.Today.ToUniversalTime().AddHours(8), item.RegistrationCloseDate) <= 0)
       {%>
					<tr>
						<td class="eventview" colspan="2">
							<% Html.RenderPartial("RSVPStatus", item); %>
						</td>
					</tr>
					<%}%>
					<tr>
						<td class="eventview" colspan="2">
							<% string des = SMCHSGManager.Models.GeneralFunction.RemoveHtmlFormat(item.Description); %>
							<%: Html.Truncate(des, 50)%>
							<%: Html.ActionLink("read more", "Details", "Event", new { id = item.ID }, null)%>
							&raquo
						</td>
					</tr>
					<% if (itemNo < Model.UpcomingEvents.Count())
        {%>
					<tr>
						<td colspan="2" class="eventview">
							<div class="dashedline">
							</div>
						</td>
					</tr>
					<%} %>
					<% } %>
				</table>
				<% } %>
				<div class="dashedline">
				</div>
				<%: Html.ActionLink("View all events", "Index", "Event")%>
				&raquo;
			</div>

            <%} %>
		</div>
		<div class="clear2column">
		</div>
	</div>
</asp:Content>

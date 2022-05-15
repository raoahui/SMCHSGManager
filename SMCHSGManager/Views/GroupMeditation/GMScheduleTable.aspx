<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    SMCH Association Singapore - 	 Group Meditation Schedule Table
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="body">
    <h5>Group Meditation Schedule</h5>

   
    <table style="border:0">
    <tr><td colspan=5 style="border:0"> 1) Evening / Morning. For full & half initiates, on 3rd floor.</td></tr>
        <tr>
            <th>Day</th>
            <th>Door Opens</th>
            <th>Door Closes</th>
            <th>Video Watching</th>
            <th>Group Meditation</th>
        </tr>
        <tr>
            <td>Monday, Tuesday, Wednesday & Thursday</td>
            <td>6:00pm<font color="red" size="2">*</font></td>
            <td>7:30pm</td>
            <td>6:30pm - 7:00pm</td>
            <td>7:00pm - 9:30pm</td>
        </tr>
        <tr>
            <td>Saturday</td>
            <td>6:00pm<font color="red" size="2">*</font></td>
            <td>7:00pm</td>
            <td>6:00pm - 7:00pm</td>
            <td>7:00pm - 9:30pm</td>
        </tr>
        <tr>
            <td>Sunday</td>
            <td>8:00am<font color="red" size="2">**</font></td>
            <td>9:00am</td>
            <td>8:00am - 9:00am</td>
            <td>9:00am - 1:00pm</td>
        </tr>

    </table>
    </br>

     <table style="border:0">
     <tr><td colspan=5 style="border:0"> 2) Overnight. For full initiates only, on 3rd floor.</td></tr>
    <tr>
            <th>Day</th>
            <th>Door Opens</th>
            <th>Door Closes</th>
            <th>Video Watching</th>
            <th>Group Meditation</th>
        </tr>
       <tr>
            <td>Wednesday</td>
            <td>11:00pm<font color="red" size="2">**</font></td>
            <td>12:00 midnight</td>
            <td>11:00pm - 12:00am</td>
            <td>12:00am - 6:00am</td>
        </tr>
         <tr>
            <td><span class="Apple-style-span" 
                    style="border-collapse: separate; color: rgb(0, 0, 0); font-family: 'Times New Roman'; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: 2; text-align: -webkit-auto; text-indent: 0px; text-transform: none; white-space: normal; word-spacing: 0px; -webkit-border-horizontal-spacing: 0px; -webkit-border-vertical-spacing: 0px; -webkit-text-decorations-in-effect: none; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; font-size: medium; ">
                <span class="Apple-style-span" 
                    style="border-collapse: collapse; color: rgb(42, 64, 19); font-family: Verdana, Helvetica, Helvetica-Narrow, Tahoma, sans-serif; font-size: 12px; -webkit-border-horizontal-spacing: 2px; -webkit-border-vertical-spacing: 2px; ">
                Saturday</span></span></td>
            <td>11:00pm<font color="red" size="2">**</font></td>
            <td>12:00 midnight</td>
            <td>11:00pm - 12:00am</td>
            <td>12:00am - 7:00am</td>
        </tr>    </table>
</br>
    <table style="border:0">
        <tr><td colspan=5 style="border:0">3) Convenient  Method  (2nd floor)</td></tr>
        <tr>
            <th>Day</th>
            <th>Door Opens</th>
            <th>Door Closes</th>
            <th>Video Watching</th>
            <th>Group Meditation</th>
        </tr>
        <tr>
            <td>Friday</td>
            <td>6:30pm</td>
            <td>8:00pm</td>
            <td>7:00pm - 8:00pm</td>
            <td>8:00pm - 8:30pm</td>
        </tr>
 </table>

   </br>

 <table style="border:0">
		<tr><td colspan=5 style="border:0">
			Note:
		</td></tr>
		<tr><td colspan=5 style="border:0">
			<font color="red" size="2">*</font> Door will only open at the times stated in this schedule by the Dharma Protector (DP).
		</td></tr>
		<tr><td colspan=5 style="border:0">
			<font color="red" size="2">**</font> Upon arrival, the DP to clear 3rd floor to ensure every initiate sign-in / log-in his/her attendance
		</td></tr>
		<tr><td colspan=5 style="border:0">
			 at the	entrance. All initiates are to co-operate with the DP.
		</td></tr>
  </table>


</div>
</asp:Content>

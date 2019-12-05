<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.VideoFile>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div id="body">
<% if (Model.FirstOrDefault().VideoCategoryID == 1)
   { %>
    <h5>Health Living 健康生活</h5>
    <%}
   else
   { %>
   <h5> Climate Change 全球暖化</h5>
   <%} %>
     <script language="javascript" type="text/javascript">
     function playIt(filename) {
         document.getElementById('Player').URL = filename;
     }
</script>

 <%-- <% foreach (var item in Model) { %>
        <a href="<%: item.FileName%>"> 
            <%: item.EngTitle %></br>
            <%: item.ChiTitle %>
        </a>
   <% } %>--%>
   <p align="center">
             <object id="Player" width="640" height="480" classid="CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6" codebase="http://www.microsoft.com/Windows/MediaPlayer/">
                <param name="volume" value="100" />
                   <param name="AutoStart" value="true"/>
                   <param name="BufferingTime" value="2"/>
                   <param name="ShowStatusBar" value="true"/>
                   <param name="InvokeURLs" value="false"/>
                <param name="stretchToFit" value="-1" />
            </object>

           <%--<object classid="clsid:22D6F312-B0F6-11D0-94AB-0080C74C7E95" width="640" height="480" codebase="http://www.microsoft.com/Windows/MediaPlayer/">
                <param name="Filename" value="../../Video/Dr. James Hansen.wmv" />
                <param name="AutoStart" value="true"/>
                <param name="ShowControls" value="true"/>
                <param name="BufferingTime" value="2"/>
                <param name="ShowStatusBar" value="true"/>
                <param name="AutoSize" value="true"/>
                <param name="InvokeURLs" value="false"/>
                 <embed src="../../Video/Dr. James Hansen.wmv" type="application/x-mplayer2" pluginspage="http://www.microsoft.com/Windows/MediaPlayer/" CODEBASE="http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=6,0,0,0" autostart="1" enabled="1" showstatusbar="1" showdisplay="1" showcontrols="1" width="480" height="360"></embed>
            </object>--%>
  </p>

   <table>
    <tr>
                    <th>No</th>
                    <th>Playlist</th>
    </tr>

            <% int i = 0;
                foreach (var item in Model)
               { %>
                <tr>
                     <td>
                        <%: (++i).ToString() %>
                    </td>
                    <td>
                        <%  string fileName = "Video/";  //string fileName = "../../Video/"; 
                            if (item.VideoCategory.Name == "HealthLive")
                           {
                               fileName += "Health/" + item.FileName;
                           }
                           else
                           {
                               fileName += "ClimateChange/" + item.FileName;
                           }%> 
                        <a href="#" onclick="playIt('<%:fileName %>')"><%:item.EngTitle + "(" + item.ChiTitle+")" %> </a>
                     </td>
               </tr>
            <% } %>

</table>
<br />
 
 <% if(Roles.IsUserInRole("Administrator")){ %>
    <p align="center">
       <%: Html.ActionLink("Create New", "Create", null, new { @style = "color:white;", @class = "buttonsmall" })%>
     <%} %>
     </p>
</div>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMTVManager.ViewModel.YouTubePlayerListViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
SMTVShows - YouTube Player
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<style>
    div#test
    {
    	position:relative;
    }
div#testtext {
    position:absolute;
    font-size: 80%;
    width: 33px;
    left: 64px;
    top: 48px;
     background-color:Black;
     color:White;
}

div.demo {
/*background:#CCCCCC none repeat scroll 0 0;*/
position:relative;
overflow:auto; 
height: 530px; 
text-align:left;  
}
  p { margin:10px;padding:5px;border:2px solid #666;width:1000px;height:1000px; }

</style>

 <div id="body">
 <%  
	//Google.YouTube.Video curVideo = ( Google.YouTube.Video)ViewData["CurVideo"];
	 SMCHSGManager.ViewModel.YouTubePlayerViewModel curVideo = (SMCHSGManager.ViewModel.YouTubePlayerViewModel)ViewData["CurVideo"];
     TempData["PrevVideo"] = curVideo;
     %>

  <table style="border:0; background-color:#81CF58"> 
     <tr style="height:560px">
        <td style="border:0">
           <table style="border:0;">
                 <tr style="height:10px">
                    <td style="border:0;" colspan=2> 
                        <%: ViewData["YouTubeChannelTitle"] %>   <%: " - " + curVideo.title %>
                    </td>
                </tr>
                <tr>
                    <td style="border:0; margin-top:0" colspan=2>
							<%--<% Html.RenderPartial("YouTubePlayer", curVideo.videoID, new ViewDataDictionary { {"autoPlay", ViewData["autoPlay"]} }); %>--%>
 					 
						 <%= Html.YouTubePlayer("test", curVideo.videoID, new YouTubePlayerOption { Width = 660, Height = 410, PrimaryColor = System.Drawing.Color.Black, SecondaryColor = System.Drawing.Color.Pink, Border = false, autoPlay = (int)ViewData["autoPlay"] })%>
                    </td>
                </tr>
                <tr style="height:6px;" >
                        <td style=" margin-left:0; border:0" ><%= Resources.Strings.Uploadedby%><font size="2"  ><%:  curVideo.author%> </font> <%:  " | " + string.Format("{0:ddd, d MMM yyyy  }", curVideo.update) + " | " + curVideo.viewCount.ToString() + " views" %>
						</td>
						<td style=" margin-right:0; border:0" >
                              <% if (!string.IsNullOrEmpty(curVideo.prev))
                                { %>
                                      <a href='<%: Url.Action("index", "YouTubePlayer",  new { videoID = curVideo.prev, youTubeChannelID = ViewData["YouTubeChannelID"]}) %>' > 
                                      <img src="../images/previous.gif" alt=""  width="16" height="18" title="<%: GetGlobalResourceObject("Strings","PreviousPart") %>" /></a> 
                               <%} %>

                              <% if (!string.IsNullOrEmpty(curVideo.next))
                                { %>
                                         <a href='<%: Url.Action("index", "YouTubePlayer",  new { videoID = curVideo.next, youTubeChannelID = ViewData["YouTubeChannelID"]}) %>' > 
                                        <img src="../images/next.gif" alt=""  width="16" height="18" title="<%: GetGlobalResourceObject("Strings","NextPart") %>" /></a>
                              <%} %>

                                <% if (!string.IsNullOrEmpty(curVideo.scriptDownloadLink))
                                  { %>
                                       <a href="<%: curVideo.scriptDownloadLink %>" target="_blank" >
                                       <img src="../images/icon_download_script-ovr.gif" alt=""  width="16" height="18" title="<%: GetGlobalResourceObject("Strings","TranscriptDownloadLink") %>"/></a>
                              <%} %>

                              <% if (!string.IsNullOrEmpty(curVideo.videoDownloadLink))
                                 { %>
                                       <a href="<%: curVideo.videoDownloadLink %>" target="_blank" >
                                       <img src="../images/icon_download_video-ovr.gif" alt=""  width="16" height="18" title="<%: GetGlobalResourceObject("Strings","VideoDownloadLink") %>" /></a>
                              <%} %>

                        </td>
				</tr>                          
                <tr style=" vertical-align:top; background:#307815 url(../../images/background_left_gg.jpg) repeat-y">  <%--background-color:#FFD966;--%> 
						<td colspan=2 style=" margin-left:0; border:0">
							<div style="padding:1px; overflow:auto;  height: 105px;  "> 
								<a href="http://SupremeMasterTV.com" target="_blank" title="http://SupremeMasterTV.com" rel="nofollow" dir="ltr" >http://SupremeMasterTV.com</a>
								<% int index = curVideo.description.IndexOf(';') + 1;
									string airdate = curVideo.description.Substring(0, index);						%>
									 <%: airdate%>
									<br />
									<%: curVideo.description.Substring(index)%>
									<br />
									Tags: <%: curVideo.tag %>
							</div>
						</td>
				</tr>
            </table>
        </td>
        <td style="border:0; width:310px; ">
            <table style=" border:0; width:310px; margin-bottom:0; margin-left:0 ">
                <tr style="height:50px">
                    <td style="overflow:auto; border:0">   <%= Resources.Strings.Videos%> (<%: Model.Count()%>) 
					  <% if ((string)ViewData["PlayList"] != null){ %>
							  <a href='<%: Url.Action("PlayAll", "YouTubePlayer",  new { channelID = ViewData["YouTubeChannelID"] }) %>'>
                                                    <img src="../../images/Play-All.png"  Title="Play All" alt=" "/>
                              </a>
					  <%} %>
                         <% using (Html.BeginForm()) {%>
						 <table style=" border:0; margin-bottom:0; margin-left:0 ">
							 <tr>
								<td style=" border:0; ">
									<%: Html.TextBox("searchContent")%>  
								</td>
								
							  <% if ((int)ViewData["YouTubeChannelID"] <= (int)ViewData["MaxYouTubeChannelCount"])
								{ %>
								<td style=" border:0; ">
										<input type="image" src="../../images/search.jpg" value="<%: GetGlobalResourceObject("Strings","SearchButton") %>"  title="Search Current Channel" onsubmit="submit-form(); ">
								</td>
							  <%} %>
 										<%--  <input type="submit"    value="<%: GetGlobalResourceObject("Strings","SearchAllButton") %>" />--%>
								<td style=" border:0; ">
									<input type="image" src="../../images/btn-search.jpg"  value="<%: GetGlobalResourceObject("Strings","SearchAllButton") %>" title="Search All Channels"  onsubmit="submit-form(); ">
								</td>
							</tr>
					   </table>
					    <% } %>
                    </td> 
                </tr>
			
                <tr style="background:#307815 url(../../images/background_left_gg.jpg) repeat-y"> 
                    <td  style="overflow:auto; border:0">
						<div class = "demo" style="overflow:auto; height: 530px; text-align:left;  position:relative; "> 
							<% if ((int)ViewData["autoPlay"] == 0)
								 { %>
									 No videos were found!
								 <%}%>
								 <table style="border:0; width:280px;">
								 <%  	index = 0; int currentItem = index;
								 	foreach (var item in Model) {
									if (item.youtubeVideoIDs.Contains(curVideo.videoID))
                                    {
										currentItem = index; %>
                                        <tr style="background-color:#6bbb42;  border-bottom:thick solid #6bbb42;" >
                                        <%}
                                           else
                                           { %>
                                        <tr style="border-bottom:thick solid #6bbb42; ">
                                        <%}
											index++; %>
                                           <td  style="border:0; ">
                                            <div style="position: relative;">
                                                <div  id = "testtext">
												<%
													int seconds = int.Parse(item.duration);
													int minutes = seconds / 60;
													string curDuration = minutes.ToString();
													seconds = seconds - minutes * 60;
													if (seconds < 10)
													{
														curDuration += ":0" + seconds.ToString();
													}
													else
													{
														curDuration += ':' + seconds.ToString();
													}
												 %>
                                                 <%: curDuration %>
                                                </div>
                                            </div>
                                                 <a href='<%: Url.Action("index", "YouTubePlayer",  new { videoID = item.youtubeVideoIDs.ToList()[0], youTubeChannelID = ViewData["YouTubeChannelID"] }) %>'>
                                                    <img src="<%: item.thumbnailURL %>" width="98" height="60" alt="" />
                                                 </a>
                                          </td>
                                          <td  style="border:0; vertical-align:top">
										 
                                                <%: Html.ActionLink(Html.Truncate(item.title, 40), "index", "YouTubePlayer", new { videoID = item.youtubeVideoIDs.ToList()[0], youTubeChannelID = ViewData["YouTubeChannelID"] }, new { @style = "font-weight:bolder; font-size:smaller; text-decoration:none" })%>    
                                                <br />

  											 <% if (item.youtubeVideoIDs.Count() > 1)
												{
													for (int i = 0; i < item.youtubeVideoIDs.Count(); i++)
													{
														if (curVideo.videoID == item.youtubeVideoIDs.ToList()[i])
														{%>
														<%: (i+1).ToString()%>
													<%}
														else
														{ %>
														<%: Html.ActionLink((i + 1).ToString(), "index", "YouTubePlayer", new { videoID = item.youtubeVideoIDs.ToList()[i], youTubeChannelID = ViewData["YouTubeChannelID"] }, new { title = "Part"+(i+1).ToString() })%>    
												<%}
													}
											} %>

                                            </td>
                                        </tr>
										
                             <% } 
                             	currentItem = currentItem * 52; %>
                        </table>
                        </div>

						<script>
							var $pos = '<%= currentItem.ToString() %>';
							$("div.demo").scrollTop($pos);

						</script>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
  </table>

</div>

</asp:Content>

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.YouTube; 
using System.Net;



/// <summary>
/// Summary description for VideoList
/// </summary>
public static class ListVideos
{
    public static IEnumerable<Video> MostPopular(string userName, string password)
    {
        return GetVideos(YouTubeQuery.MostViewedVideo, userName, password);
    }

    public static IEnumerable<Video> YourVideos(string userName, string password)
    {
        return GetVideos(YouTubeQuery.DefaultUploads, userName, password);
    }

    public static IEnumerable<Video> MostCommented(string userName, string password)
    {
        return GetVideos(YouTubeQuery.MostDiscussedVideo, userName, password);
    }

    public static void Update(Video v, string userName, string password)
    {
        GetRequest( userName, password).Update(v);
    }



    public static IEnumerable<Playlist> PlayLists(string userName, string password)
    {
        Feed<Playlist> feed = null;
        YouTubeRequest request = GetRequest(userName, password);
        

        try
        {
            feed = request.GetPlaylistsFeed(null);
        }
        catch (GDataRequestException gdre)
        {
            HttpWebResponse response = (HttpWebResponse)gdre.Response;
        }
        return feed != null ? feed.Entries : null;
    }

    
    public static YouTubeRequest GetRequest(string userName, string password)
    {
        // let's see if we get a valid authtoken back for the passed in credentials....
        YouTubeRequestSettings settings = new YouTubeRequestSettings("YouTubeAspSample",
                                            "AI39si4v3E6oIYiI60ndCNDqnPP5lCqO28DSvvDPnQt-Mqia5uPz2e4E-gMSBVwHXwyn_LF1tWox4LyM-0YQd2o4i_3GcXxa2Q",
                                            userName, password);
        YouTubeRequest request = new YouTubeRequest(settings);
        string authToken = null;
        try
        {
            authToken = request.Service.QueryClientLoginToken();
         }
        catch
        {
        }
        request.Service.SetAuthenticationToken(authToken);
    
        //YouTubeRequest request = HttpContext.Current.Session["YTRequest"] as YouTubeRequest;
        //if (request == null)
        //{
        //    YouTubeRequestSettings settings = new YouTubeRequestSettings("YouTubeAspSample",
        //                                    "AI39si4v3E6oIYiI60ndCNDqnPP5lCqO28DSvvDPnQt-Mqia5uPz2e4E-gMSBVwHXwyn_LF1tWox4LyM-0YQd2o4i_3GcXxa2Q",
        //                                    HttpContext.Current.Session["token"] as string
        //                                    );
        //    settings.AutoPaging = true;
        //    request = new YouTubeRequest(settings);
        //    HttpContext.Current.Session["YTRequest"] = request;
        //}
        return request;
    }

    public static IEnumerable<Video> Search(string videoQuery, string author, string orderby, bool racy, string time, string category, string userName, string password)
    {
        YouTubeQuery query = new YouTubeQuery(YouTubeQuery.TopRatedVideo);
        if (String.IsNullOrEmpty(videoQuery) != true)
        {
            query.Query = videoQuery;
        }
        if (String.IsNullOrEmpty(author) != true)
        {
            query.Author = author;
        }
        if (String.IsNullOrEmpty(orderby) != true)
        {
            query.OrderBy = orderby;
        }
        if (racy == true)
        {
            query.SafeSearch = YouTubeQuery.SafeSearchValues.None;
        }
        if (String.IsNullOrEmpty(time) != true)
        {
            if (time == "All Time")
                query.Time = YouTubeQuery.UploadTime.AllTime;
            else if (time == "Today")
                query.Time = YouTubeQuery.UploadTime.Today;
            else if (time == "This Week")
                query.Time = YouTubeQuery.UploadTime.ThisWeek;
            else if (time == "This Month")
                query.Time = YouTubeQuery.UploadTime.ThisMonth;
        }
        if (String.IsNullOrEmpty(category) != true)
        {
            QueryCategory q  = new QueryCategory(new AtomCategory(category));
            query.Categories.Add(q);
        }
        return ListVideos.GetVideos(query, userName, password);
    }



    private static IEnumerable<Video> GetVideos(string videofeed, string userName, string password)
    {
        YouTubeQuery query = new YouTubeQuery(videofeed);
        return ListVideos.GetVideos(query, userName, password);
    }

    private static IEnumerable<Video> GetVideos(YouTubeQuery q, string userName, string password)
    {
        YouTubeRequest request = GetRequest(userName, password);
        Feed<Video> feed = null; 


        try
        {
            feed = request.Get<Video>(q);
        }
        catch (GDataRequestException gdre)
        {
            HttpWebResponse response = (HttpWebResponse)gdre.Response;
        }
        return feed != null ? feed.Entries : null;
    }

}

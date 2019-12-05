using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace System.Web.Mvc.Html
{
     public static class YouTubePlayerHelper
    {
        public static string YouTubePlayer(this HtmlHelper helper,string playerID, string mediaFile, YouTubePlayerOption youtubePlayerOption)
        {

            string BaseURL = "http://www.youtube.com/v/";

            // YouTube Embedded Code
            string player = @"<div id=""YouTubePlayer_{7}"" style=""width:{1}px; height:{2}px;"">
                                 <object width=""{1}"" height=""{2}"">
                                 <param name=""movie"" value=""{6}{0}&fs=1&autoplay={8}&border={3}&color1={4}&color2={5}""></param>
                                 <param name=""allowFullScreen"" value=""true""></param>
                                 <embed src=""{6}{0}&fs=1&autoplay={8}&border={3}&color1={4}&color2={5}"" 
                                  type = ""application/x-shockwave-flash""
                                 width=""{1}"" height=""{2}"" allowfullscreen=""true""></embed>
                                 </object>
                             </div>";

            // Replace All The Value
            player = String.Format(player, mediaFile, youtubePlayerOption.Width, youtubePlayerOption.Height, (youtubePlayerOption.Border ? "1" : "0"), ConvertColorToHexa.ConvertColorToHexaString(youtubePlayerOption.PrimaryColor), ConvertColorToHexa.ConvertColorToHexaString(youtubePlayerOption.SecondaryColor), BaseURL, playerID, youtubePlayerOption.autoPlay);

            //Retrun Embedded Code
            return player;
        }
    }

    public class YouTubePlayerOption
    {
        int width = 425;
        int height = 355;
        Color Color1 = Color.Black;
        Color Color2 = Color.Aqua;
        bool border = false;
        int autoplay = 1;

        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public Color PrimaryColor { get { return Color1; } set { Color1 = value; } }
        public Color SecondaryColor { get { return Color2; } set { Color2 = value; } }
        public bool Border { get { return border; } set { border = value; } }
        public int autoPlay { get { return autoplay; } set { autoplay = value; } }
    }

    public class ConvertColorToHexa
    {      
        static char[] hexDigits = {
         '0', '1', '2', '3', '4', '5', '6', '7',
         '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};


        public ConvertColorToHexa()
        {
        }

        public static string ConvertColorToHexaString(Color color)
        {
            byte[] bytes = new byte[3];
            bytes[0] = color.R;
            bytes[1] = color.G;
            bytes[2] = color.B;
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }


    }        

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_upload_youtube_console.Model
{
    public class RandomUploadJobInput
    {
        public string UserId { get; set; }
        public string ProfilePath { get; set; }
        public double Timer { get; set; }
    }

    public class UploadJobInput
    {
        public string ProfilePath { get; set; }
        public string ExecutablePath { get; set; }
        public string ChannelName { get; set; }
        public string ChannelID { get; set; }
        public string VideoFilePath { get; set; }
        public string ImageFilePath { get; set; }

        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        public string? PlaylistName { get; set; }
        public bool? IsForKid { get; set; }
        public bool? IsLimitedOld { get; set; }
        public List<string> Tags { get; set; }
        public string? Category { get; set; }
        public string? VideoLanguage { get; set; }
        public DisplayVideoModeType DisplayVideoMode { get; set; }
        public string? Schedule { get; set; }

        //Proxy
        public string? IP { get; set; }
        public string? Port { get; set; }
        public string? ProxyUsername { get; set; }
        public string? ProxyPassword { get; set; }

        // For tool install on vps
        public string ImageThumbGoogleDriveURL { get; set; }
        public string VideoGoogleDriveURl { get; set; }
        public string CookieYoutube { get; set; }
    }

    public class Input
    {
        public string serviceName { get; set; }
        public string jobId { get; set; }
        public UploadJobInput input { get; set; }
        public int timeoutMilisecond { get; set; }
    }

    public enum DisplayVideoModeType
    {
        Private,
        Public,
        Unlisted
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_upload_youtube_console.Model
{
    public class RequestDataVideoModel
    {
        public int actionState { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int itemCount { get; set; }
        public int pageCount { get; set; }
        public Items[] items { get; set; }
    }

    public class Items
    {
        public int productVideoId { get; set; }
        public ProducVideo productVideo { get; set; }
        public int youtubeChannelId { get; set; }
        public YoutubeChannelId youtubeChannel { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string tags { get; set; }
        public string userId { get; set; }
        public string user { get; set; }
        public int uploadState { get; set; }
    }

    public class ProducVideo
    {
        public string name { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public int id { get; set; }
        public DateTime createDate { get; set; }
        public string userId { get; set; }
        public string user { get; set; }
        //public int uploadState { get; set; }
    }

    public class YoutubeChannelId
    {
        public bool isDeleted { get; set; }
        public string channelName { get; set; }
        public string channelId { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public int id { get; set; }
        public DateTime createDate { get; set; }
        public string userId { get; set; }
        public string user { get; set; }
    }
}

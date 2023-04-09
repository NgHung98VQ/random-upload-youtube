using Newtonsoft.Json;
using random_upload_youtube_console.AutoUpload;
using random_upload_youtube_console.Model;
using random_upload_youtube_console.RequestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace random_upload_youtube_console.RandomJob
{
    public class RandomProcess
    {

        private RandomUploadJobInput _inputRandom;
        private List<Detail> listDetail;
        private List<string> _listChannelID = new List<string>();
        public RandomProcess(RandomUploadJobInput inputRandom)
        {
            _inputRandom = inputRandom;
        }

        public async Task RandomUploadPost(Detail detail, string channelID)
        {
            string tag = detail.tags;
            List<string> listTags = new List<string>();
            string[] tags = tag.Split(',');
            foreach (var h in tags)
            {
                listTags.Add(h);
            }
            var uploadJobInput = new UploadJobInput();
            uploadJobInput.ProfilePath = "C:\\RandomUpload\\ProfileCookie";
            uploadJobInput.ChannelName = null;
            uploadJobInput.ChannelID = channelID;
            uploadJobInput.VideoFilePath = null;
            uploadJobInput.VideoTitle = detail.title;
            uploadJobInput.VideoDescription = detail.description;
            uploadJobInput.PlaylistName = null;
            uploadJobInput.IsForKid = true;
            uploadJobInput.IsLimitedOld = true;

            uploadJobInput.Tags = listTags;
            uploadJobInput.Category = null;
            uploadJobInput.VideoLanguage = null;
            uploadJobInput.DisplayVideoMode = DisplayVideoModeType.Public;
            uploadJobInput.Schedule = null;
            uploadJobInput.IP = null;
            uploadJobInput.Port = null;
            uploadJobInput.ProxyUsername = null;
            uploadJobInput.ImageThumbGoogleDriveURL = detail.linkImgThumb;
            uploadJobInput.VideoGoogleDriveURl = detail.linkVideo;
            uploadJobInput.CookieYoutube = _inputRandom.ProfilePath + "\\" + channelID + ".txt";
            uploadJobInput.ProxyPassword = null;


            UploadJob uploadJob = new UploadJob(uploadJobInput);
            uploadJob.RunUploadJob();

        }

        public async Task RunRandomUpload()
        {
            //Request link google drive
            Console.WriteLine("Đang lấy link Google drive.....!");

            var userId = _inputRandom.UserId;
            var request = new RequestDetail();
            listDetail = await request.RunRequest(userId, _listChannelID.Count);
            if (_inputRandom.Timer != 0)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();

                Task timerTask = RunPeriodically(sendRequest, TimeSpan.FromSeconds(_inputRandom.Timer), tokenSource.Token);
            }
            else
            {
                Console.WriteLine("Đang lấy link Google drive.....!");
                sendRequest();
            }
        }

        static async Task RunPeriodically(Action action, TimeSpan interval, CancellationToken token)
        {
            while (true)
            {
                action();
                await Task.Delay(interval, token);
            }
        }

        public async void sendRequest()
        {
            try
            {
                _listChannelID = File.ReadAllLines("C:\\RandoUpload\\list-channel-id.txt").ToList();
                
                foreach (var channelID in _listChannelID)
                {
                    Console.WriteLine($"Đang chọn random 1 video lên kênh {channelID}.....!");

                    var index = _listChannelID.IndexOf(channelID); 

                    Detail item = listDetail[index];

                    await RandomUploadPost(item, channelID);
                    //random video 
                }

            }
            catch (Exception ex)
            {
                LogMessage.Log(ex.Message);
            }
           
        }

        public async Task PostRequest(string url, StringContent data)
        {
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            string result = response.Content.ReadAsStringAsync().Result;

        } 
    }
}

using Newtonsoft.Json;
using random_upload_youtube_console.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace random_upload_youtube_console.RequestData
{
    public class RequestDetail
    {
        public async Task<List<Detail>> RunRequest(string userId, int maxDetail)
        {
            List<Detail> detailVideoLists = new List<Detail>();
            try
            {
                var url = "https://183.80.50.40:8443/api/video-release/youtube?pageNumber=1&pageSize=999&userId=" + userId;
                var data = await Get(url);

                var jobjectData = JsonConvert.DeserializeObject<RequestDataVideoModel>(data);

                var listData = jobjectData.data.items.ToList();


                for (int i = 0; i < maxDetail; i++)
                {
                    Detail detail = new Detail();
                    int state = 0;
                    int index = 0;

                    do
                    {
                        var random = new Random();
                        index = random.Next(listData.Count);

                        state = listData[index].uploadState;
                        Console.WriteLine("Randoming index");

                    } while (state != 1);


                    var urlVideo = $"https://183.80.50.40:8443/api/product-video?userId={userId}&id={listData[index].productVideoId}";
                    var dataGoogleDrive = await Get(urlVideo);

                    var jObjectLink = JsonConvert.DeserializeObject<RequestLinkGoogleDriveModel>(dataGoogleDrive);

                    //Detail detail = new Detail();

                    detail.title = listData[index].title;
                    detail.description = listData[index].description;
                    detail.tags = listData[index].tags;
                    detail.linkVideo = $"https://drive.google.com/file/d/{jObjectLink.data.items[0].googleDriveFileId}/view";
                    detail.linkImgThumb = jObjectLink.data.items[0].googleDriveThumbFileId;

                    detailVideoLists.Add(detail);


                }


                return detailVideoLists;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải file: {ex.Message}");
            }
        }

        private async Task<string> Get(string url)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient httpClient = new HttpClient(clientHandler);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errMess = await response.Content.ReadAsStringAsync();
                    throw new Exception(errMess);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

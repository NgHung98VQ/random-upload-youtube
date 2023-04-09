using random_upload_youtube_console.Download;
using random_upload_youtube_console.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_upload_youtube_console.AutoUpload
{
    public class UploadJob
    {

        private UploadJobInput _input;

        public UploadJob(UploadJobInput input)
        {
            _input = input;
        }


        public async void RunUploadJob()
        {
            try
            {

                // Validate input
                if (string.IsNullOrEmpty(_input.ImageThumbGoogleDriveURL) || string.IsNullOrEmpty(_input.VideoGoogleDriveURl))
                {
                    throw new Exception("Đầu vào không hợp lệ");
                }

                //if (string.IsNullOrEmpty(_input.ProfilePath))
                //{
                //    throw new Exception("Thiếu profile path");
                //}

                // Download Video
                Console.WriteLine("Đang download video ...");
                var imageThumbGGDriveURL = _input.ImageThumbGoogleDriveURL;
                var videoGGDriveURL = _input.VideoGoogleDriveURl;
                var downloadProcess = new DownloadProcess(imageThumbGGDriveURL, videoGGDriveURL);
                await downloadProcess.Run();

                _input.VideoFilePath = Path.GetFullPath(downloadProcess.VideoFilePath);
                _input.ImageFilePath = Path.GetFullPath(downloadProcess.ImageThumbFilePath);

                // Upload Youtube
                Console.WriteLine("Đang download video ...");
                YoutubeAutoUploadProcess uploadYoutubeProcess = new YoutubeAutoUploadProcess(_input);
                await uploadYoutubeProcess.Run();

            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                // release resources
            }
        }
    }
}

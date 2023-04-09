using OpenQA.Selenium;
using random_upload_youtube_console.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_upload_youtube_console.Download
{
    public class DownloadJobInput
    {
        public string GoogleDriveLink { get; set; }
        public string GoogleDriveImageLink { get; set; }
    }

    public class DownloadJob
    {
        private DownloadJobInput _input;

        public DownloadJob(DownloadJobInput input)
        {
            _input = input;
        }

        public async void ExecuteProcessAsync()
        {
            try
            {

                // execute job
                var imageThumbGGDriveURL = _input.GoogleDriveImageLink;
                var videoGGDriveURL = _input.GoogleDriveLink;
                var downloadProcess = new DownloadProcess(imageThumbGGDriveURL, videoGGDriveURL);
                await downloadProcess.Run();

            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // release resources
            }
        }
    }
}

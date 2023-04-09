using random_upload_youtube_console.Model;
using random_upload_youtube_console.RandomJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


RandomUploadJobInput randomInput = new RandomUploadJobInput();
randomInput.UserId = "801391c1-ac24-4593-b97e-7575752a3fb5";
randomInput.Timer = 1000;
randomInput.ProfilePath = "C:\\RandomUpload";
RandomProcess randomProcess = new RandomProcess(randomInput);
await randomProcess.RunRandomUpload();
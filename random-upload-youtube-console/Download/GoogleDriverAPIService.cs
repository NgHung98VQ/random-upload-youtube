using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Newtonsoft.Json;

namespace random_upload_youtube_console.Download
{
    public class GoogleDriverAPIService
    {
        // gmail HG IT
        private const string _clientId = "84590236707-4lmfh3kbv9pjlhi27g7p08b3avspevgq.apps.googleusercontent.com";
        private const string _clientSecret = "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";
        private const string _applicationName = "hg upload video";

        public delegate void ProgressChangedEventHandler(Google.Apis.Download.IDownloadProgress obj);
        public event ProgressChangedEventHandler DownloadProcessChange;

        private UserCredential GetCredential()
        {
            var credentialStr = File.ReadAllText("data/google-drive-api-credential.txt");

            var googleAuthFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
            {
                ClientSecrets = new ClientSecrets()
                {
                    ClientId = _clientId,
                    ClientSecret = _clientSecret,
                }
            });

            dynamic token = JsonConvert.DeserializeObject<dynamic>(credentialStr);
            TokenResponse responseToken = new TokenResponse()
            {
                AccessToken = token.access_token,
                ExpiresInSeconds = Convert.ToInt64(token.expires_in),
                RefreshToken = token.refresh_token,
                Scope = token.scope,
                TokenType = token.token_type,
            };

            var credential = new UserCredential(googleAuthFlow, "", responseToken);

            return credential;
        }

        public long GetFileSize(string address)
        {
            var fileId = GetGoogleDriveFileId(address);

            var credential = GetCredential();
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });
            var request = service.Files.Get(fileId);
            request.Fields = "*";
            var file = request.Execute();
            return (long)file.Size;
        }

        public async Task<string> DownloadFile(string address, string filePath, CancellationToken cancellationToken)
        {
            var fileId = GetGoogleDriveFileId(address);

            var credential = GetCredential();
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });

            if (File.Exists(filePath))
            {
                return filePath;
            }
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                var process = service.Files.Get(fileId);
                process.MediaDownloader.ProgressChanged += MediaDownloader_ProgressChanged;
                var dl = await process.DownloadAsync(fileStream, cancellationToken);
                return filePath;
            }
        }

        private void MediaDownloader_ProgressChanged(Google.Apis.Download.IDownloadProgress obj)
        {
            if (DownloadProcessChange != null)
            {
                DownloadProcessChange(obj);
            }
        }

        // Handles the following formats (links can be preceeded by https://):
        // - drive.google.com/open?id=FILEID
        // - drive.google.com/file/d/FILEID/view?usp=sharing
        // - drive.google.com/uc?id=FILEID&export=download
        private string GetGoogleDriveFileId(string address)
        {
            int index = address.IndexOf("id=");
            int closingIndex;
            if (index > 0)
            {
                index += 3;
                closingIndex = address.IndexOf('&', index);
                if (closingIndex < 0)
                    closingIndex = address.Length;
            }
            else
            {
                index = address.IndexOf("file/d/");
                if (index < 0) // address is not in any of the supported forms
                    return string.Empty;

                index += 7;

                closingIndex = address.IndexOf('/', index);
                if (closingIndex < 0)
                {
                    closingIndex = address.IndexOf('?', index);
                    if (closingIndex < 0)
                        closingIndex = address.Length;
                }
            }

            return address.Substring(index, closingIndex - index);
        }
    }
}

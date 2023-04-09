using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using random_upload_youtube_console.Config;
using random_upload_youtube_console.Constant;
using random_upload_youtube_console.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_upload_youtube_console.AutoUpload
{
    public class YoutubeAutoUploadProcess
    {
        // Public 
        public bool IsCompleted { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        // Private
        UploadJobInput _input;
        ChromeDriver _driver;

        public YoutubeAutoUploadProcess(UploadJobInput input)
        {
            _input = input;
            _driver = InitChromeDriverWithCookie(_input);
        }

        public async Task Run()
        {
            try
            {
                await UploadAuto(_driver, _input);
                ReleaseDriver();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UploadAuto(ChromeDriver driver, UploadJobInput input)
        {
            try
            {
                IsCompleted = false;
                IsSuccessful = false;
                Message = "Đang mở youtube studio";

                // Set timeout for wait 30s
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                driver.Url = AppConstant.YoutubeStudioURL;
                driver.Navigate();

                // Check tài khoản bị xác minh danh tính
                if (IdentityVerification(driver))
                {
                    IsCompleted = true;
                    IsSuccessful = false;
                    Message = "Tài khoản bị xác minh danh tính";
                    throw new Exception(Message);
                }

                CancelUploadVideo(driver);

                var createVideoBtnStatus = driver.FindElement(By.Id(YoutubeConfiguration.CreateVideoBtn_ID));
                createVideoBtnStatus.Click();

                var uploadVideoBtnStatus = driver.FindElement(By.Id(YoutubeConfiguration.UploadVideoBtn_ID));
                uploadVideoBtnStatus.Click();

                // Upload video
                IWebElement fileInput = driver.FindElement(By.Name(YoutubeConfiguration.FileInput_Name));
                fileInput.SendKeys(input.VideoFilePath);

                // Input title
                var tittleInput = driver.FindElement(By.CssSelector(YoutubeConfiguration.TitleInput_CSS));
                tittleInput.Click();
                Thread.Sleep(5000);
                tittleInput.SendKeys(Keys.Control + "a");
                Thread.Sleep(2000);
                TextCopy.ClipboardService.SetText(input.VideoTitle);
                //tittleInput.SendKeys(input.VideoTitle);
                tittleInput.SendKeys(Keys.Control + "v");

                if (CheckInvalidInputText(driver, input.VideoTitle, ExceptionVideo.MaxTitle, ExceptionVideo.LambdaTitle, ExceptionVideo.CounterTitleVideo_CSS, YoutubeConfiguration.TitleInput_CSS))
                {
                    IsCompleted = true;
                    IsSuccessful = false;
                    Message = "Video Title quá dài";
                    throw new Exception(Message);
                }

                // Input Description
                var description = driver.FindElement(By.CssSelector(YoutubeConfiguration.DescriptionInput_CSS));
                description.Click();
                Thread.Sleep(5000);
                description.SendKeys(Keys.Control + "a");
                Thread.Sleep(2000);
                TextCopy.ClipboardService.SetText(input.VideoDescription);
                //description.SendKeys(input.VideoDescription);
                description.SendKeys(Keys.Control + "v");
                if (CheckInvalidInputText(driver, input.VideoDescription, ExceptionVideo.MaxDescription, ExceptionVideo.LambdaDescription, ExceptionVideo.CounterDescriptionVideo_CSS, YoutubeConfiguration.DescriptionInput_CSS))
                {
                    IsCompleted = true;
                    IsSuccessful = false;
                    Message = "Video Desciption quá dài";
                    throw new Exception(Message);
                }

                // Upload thumb
                var thumbImg = driver.FindElement(By.Id(YoutubeConfiguration.ThumbImage_ID));
                thumbImg.SendKeys(input.ImageFilePath);

                // Select playlist
                if (input.PlaylistName != null)
                {
                    var playlistsDropdown = driver.FindElement(By.CssSelector(YoutubeConfiguration.PlaylistsDropdown_CSS));
                    playlistsDropdown.Click();
                    var selectPll = driver.FindElement(By.XPath($"//*[text()='{input.PlaylistName}']"));
                    selectPll.Click();
                    var donePllBtn = driver.FindElement(By.CssSelector(YoutubeConfiguration.DonePLLBtn_CSS));
                    donePllBtn.Click();
                }

                // Allow Children
                // Default not for kid
                var isNotforKid = IsElementPresent(driver, By.Name(YoutubeConfiguration.VideoNotForKid_Name));
                var isNotfoKidChecked = driver.FindElement(By.Name(YoutubeConfiguration.VideoNotForKid_Name)).GetAttribute("aria-checked");
                if (isNotforKid)
                {
                    if (isNotfoKidChecked == "false")
                    {
                        var notForKid = driver.FindElement(By.Name(YoutubeConfiguration.VideoNotForKid_Name));
                        notForKid.Click();
                    }
                }

                // Show more
                var showMoreBtn = driver.FindElement(By.CssSelector(YoutubeConfiguration.ShowMoreBtn_CSS));
                showMoreBtn.Click();

                // Input tag
                if (input.Tags.Count() > ExceptionVideo.MaxTag)
                {
                    input.Tags = input.Tags.Select((x, i) => new { Index = i, Value = x })
                        .GroupBy(x => x.Index < ExceptionVideo.MaxTag)
                        .Select(x => x.Select(v => v.Value)).ToList()
                        .First()
                        .ToList();
                }

                //Check tag cache
                var isExistClearTagBtn = IsElementPresent(driver, By.Id(YoutubeConfiguration.ClearTagCacheBtn_ID));
                if (isExistClearTagBtn)
                {
                    var clearTagBtn = driver.FindElement(By.Id(YoutubeConfiguration.ClearTagCacheBtn_ID));
                    clearTagBtn.Click();
                    Thread.Sleep(1000);
                }
                var addTag = driver.FindElement(By.CssSelector(YoutubeConfiguration.TagInput_CSS));
                addTag.SendKeys(String.Join(",", input.Tags));

                // Select Category
                //var listCategory = driver.FindElement(By.CssSelector(YoutubeConfiguration.CategoryBtn_CSS));
                //listCategory.Click();
                //var category = driver.FindElement(By.XPath($"//*[text()='{input.Category}']"));
                //category.Click();

                //RIGHTS_MANAGEMENT
                var checkRightsManagement = IsElementPresent(driver, By.CssSelector(YoutubeConfiguration.RightsManagementBtn_CSS));

                if (checkRightsManagement)
                {
                    var nextBtnRight = driver.FindElement(By.Id(YoutubeConfiguration.NextBtn_ID));
                    nextBtnRight.Click();

                    var assetInfoDropdown = driver.FindElement(By.CssSelector(YoutubeConfiguration.AssetInformationDrop_CSS));
                    assetInfoDropdown.Click();
                    var chooseAssetInfo = driver.FindElement(By.CssSelector(YoutubeConfiguration.WebAssetInformation_CSS));
                    chooseAssetInfo.Click();

                    var nextBtnRight1 = driver.FindElement(By.Id(YoutubeConfiguration.NextBtn_ID));
                    nextBtnRight1.Click();
                    Thread.Sleep(5000);

                }


                //Check monetization
                bool checkMonetization = IsElementPresent(driver, By.CssSelector(YoutubeConfiguration.TittleMonetization_CSS));
                if (checkMonetization)
                {
                    var nextBtnMonetization = driver.FindElement(By.Id(YoutubeConfiguration.NextBtn_ID));
                    nextBtnMonetization.Click();
                    var nextBtnMonetization1 = driver.FindElement(By.Id(YoutubeConfiguration.NextBtn_ID));
                    nextBtnMonetization1.Click();
                    var nextBtnMonetization2 = driver.FindElement(By.Id(YoutubeConfiguration.NextBtn_ID));
                    nextBtnMonetization2.Click();
                    //Suitability ads
                    bool checkallNoneCheckBox = IsElementPresent(driver, By.CssSelector(YoutubeConfiguration.AllNoneCheckBox_CSS));
                    if (checkallNoneCheckBox)
                    {
                        var allNoneCheckBox = driver.FindElement(By.CssSelector(YoutubeConfiguration.AllNoneCheckBox_CSS));
                        allNoneCheckBox.Click();
                        //Send content 
                        var sendContentBtn = driver.FindElement(By.Id(YoutubeConfiguration.SendContentBtn_ID));
                        sendContentBtn.Click();

                        Thread.Sleep(5000);
                    }

                }
                else
                {
                    //Next button
                    var nextBtn1 = driver.FindElement(By.Id(YoutubeConfiguration.NextBtn_ID));
                    nextBtn1.Click();
                    var nextBtn2 = driver.FindElement(By.Id(YoutubeConfiguration.NextBtn_ID));
                    nextBtn2.Click();
                    var nextBtn3 = driver.FindElement(By.Id(YoutubeConfiguration.NextBtn_ID));
                    nextBtn3.Click();
                }

                // Select language
                //var listLanguage = driver.FindElement(By.CssSelector(YoutubeConfiguration.ListLanguageBtn_CSS));
                //listLanguage.Click();
                //var language = "Tiếng Việt";
                //var chooseLanguage = driver.FindElement(By.XPath($"//*[text()='{language}']"));
                //chooseLanguage.Click();

                // Select display mode
                IWebElement displayMode;
                switch (input.DisplayVideoMode)
                {
                    case DisplayVideoModeType.Private:
                        displayMode = driver.FindElement(By.Name(DisplayVideoMode.Private_Name));
                        displayMode.Click();
                        break;
                    case DisplayVideoModeType.Public:
                        displayMode = driver.FindElement(By.Name(DisplayVideoMode.Public_Name));
                        displayMode.Click();
                        break;
                    case DisplayVideoModeType.Unlisted:
                        displayMode = driver.FindElement(By.Name(DisplayVideoMode.Unlisted_Name));
                        displayMode.Click();
                        break;
                }

                // Schedule for public
                // If had schedule then display mode will set PUBLIC
                if (input.Schedule != null)
                {
                    var calenderBtn = driver.FindElement(By.Name(YoutubeConfiguration.CalenderBtn_Name));
                    calenderBtn.Click();
                    var dateBtn = driver.FindElement(By.Id(YoutubeConfiguration.DateBtn_ID));
                    dateBtn.Click();
                    var setDate = driver.FindElement(By.CssSelector(YoutubeConfiguration.DateInput_CSS));
                    setDate.Clear();
                    var dayToNow = DateTime.UtcNow;
                    var nextDay = dayToNow.AddDays(1);
                    var convertDay = nextDay.ToString("dd/MM/yyyy");
                    //setDate.SendKeys("23/09/2022");
                    setDate.SendKeys(Keys.Enter);
                    var setTime = driver.FindElement(By.CssSelector(YoutubeConfiguration.TimeInput_CSS));
                    setTime.Clear();
                    setTime.SendKeys(input.Schedule);

                    //var timeZoneBtn = driver.FindElement(By.Id(YoutubeConfiguration.TimeZoneBtn_ID));
                    //timeZoneBtn.Click();
                    //var timeZoneText = "(GMT-08:00) Anchorage";
                    //var timeZone = driver.FindElement(By.XPath($"//*[text()='{timeZoneText}']"));
                    //timeZone.Click();
                    //var checkBoxPremiereVideo = driver.FindElement(By.Id(YoutubeConfiguration.PremiereVideoCheckbox_ID));
                    //checkBoxPremiereVideo.Click();
                }

                // Complete
                Thread.Sleep(10000);
                var doneBtn = driver.FindElement(By.Id(YoutubeConfiguration.DoneBtn_ID));
                doneBtn.Click();

                //Check notification export
                var exportNotificationDisplayed = IsElementPresent(driver, By.Id(YoutubeConfiguration.ExportBtn_ID));
                if (exportNotificationDisplayed)
                {
                    var publishBtn = driver.FindElement(By.Id(YoutubeConfiguration.ExportBtn_ID));
                    publishBtn.Click();
                }

                //Check dialog upload video
                var dialogTittleUploadVideo = IsElementPresent(driver, By.CssSelector(YoutubeConfiguration.DialogUploadVideo_CSS));
                if (dialogTittleUploadVideo)
                {
                    var closeBtn = driver.FindElement(By.CssSelector(YoutubeConfiguration.CloseBtn_CSS));
                    closeBtn.Click();
                }

                ////Close progress

                var _exit_closeProgress = IsElementPresent(driver, By.CssSelector(YoutubeConfiguration.CloseProgressPopup_CSS));
                if (_exit_closeProgress)
                {
                    var closeProgress = driver.FindElement(By.CssSelector(YoutubeConfiguration.CloseProgressPopup_CSS));
                    closeProgress.Click();

                }

                var contentStudio = driver.FindElement(By.CssSelector(YoutubeConfiguration.ContentStudioBtn_CSS));
                contentStudio.Click();

                var count_upload_badge = driver.FindElements(By.CssSelector(YoutubeConfiguration.UploadBadge_CSS)).Count;

                if (count_upload_badge == 1)
                {
                    bool _exit_upload_badge;
                    do
                    {
                        _exit_upload_badge = IsElementPresent(driver, By.CssSelector(YoutubeConfiguration.UploadBadge_CSS));

                    } while (_exit_upload_badge);
                }
                if (count_upload_badge > 1)
                {
                    bool _exit_check_badge;
                    do
                    {
                        _exit_check_badge = IsElementPresent(driver, By.CssSelector(YoutubeConfiguration.CheckBadge_CSS));

                    } while (!_exit_check_badge);
                }

                Thread.Sleep(10000);

                IsCompleted = true;
                IsSuccessful = true;
                Message = "Đã upload video thành công";

            }
            catch (Exception ex)
            {
                await RandomUploadPost(input.ChannelID);

                IsCompleted = true;
                IsSuccessful = false;
                Message = ex.Message;
                ReleaseDriver();

                throw new Exception(ex.Message);
            }
        }

        public void CancelUploadVideo(ChromeDriver driver)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                driver.Url = AppConstant.YoutubeStudioURL;
                driver.Navigate();

                // Check tài khoản bị xác minh danh tính
                if (IdentityVerification(driver))
                {
                    IsCompleted = true;
                    IsSuccessful = false;
                    Message = "Tài khoản bị xác minh danh tính";
                    throw new Exception(Message);
                }

                var contentStudio = driver.FindElement(By.CssSelector(YoutubeConfiguration.ContentStudioBtn_CSS));
                contentStudio.Click();

                var cancelBtn = driver.FindElements(By.CssSelector(YoutubeConfiguration.CancelUploadBtn_CSS));
                if (cancelBtn.Count != 0)
                {
                    foreach (var btn in cancelBtn)
                    {
                        btn.Click();
                        var confirmCancelBtn = driver.FindElement(By.CssSelector(YoutubeConfiguration.ConfirmCancelUploadBtn_CSS));
                        confirmCancelBtn.Click();
                    }
                }
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {

            }

        }

        private ChromeDriver InitChromeDriverWithCookie(UploadJobInput input)
        {
            try
            {
                string cookieStr = File.ReadAllText(input.CookieYoutube);
                //var myAppsetting = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var chromeFilePath = input.ExecutablePath;
                var chromeOption = new ChromeOptions();
                chromeOption.BinaryLocation = chromeFilePath;
                var driver = new ChromeDriver(chromeOption);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                driver.Navigate().GoToUrl("https://www.youtube.com/");
                var cookieInfos = cookieStr.Split(";", StringSplitOptions.RemoveEmptyEntries);
                foreach (var cookieInfo in cookieInfos)
                {
                    try
                    {
                        int equalIndex = cookieInfo.IndexOf("=");
                        if (equalIndex == -1)
                        {
                            continue;
                        }
                        string key = cookieInfo.Substring(0, equalIndex).Trim();
                        string value = cookieInfo.Substring(equalIndex + 1).Trim();
                        OpenQA.Selenium.Cookie coo = new OpenQA.Selenium.Cookie(key, value, domain: ".youtube.com", path: "/", null);
                        driver.Manage().Cookies.AddCookie(coo);
                    }
                    catch
                    {
                        //throw new Exception("Không add được cookie: " + ex.Message);
                    }
                }
                driver.Navigate().Refresh();
                Thread.Sleep(5000);
                driver.Navigate().GoToUrl("https://www.youtube.com/");
                return driver;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ReleaseDriver()
        {
            try
            {
                if (_driver != null)
                {
                    _driver.Close();
                    _driver.Quit();
                    _driver.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SendKeysCopyPaste(ChromeDriver driver, By by, string text)
        {
            try
            {
                TextCopy.ClipboardService.SetText(text);
                driver.FindElement(by).SendKeys(Keys.Control + "v");
            }
            catch (Exception ex)
            {
                throw new Exception($"SendKeysCopyPaste failed with exception: {ex.Message}");
            }
        }

        private void CloseProgressPopup(ChromeDriver driver)
        {
            try
            {
                var closeProgress = driver.FindElement(By.CssSelector(YoutubeConfiguration.CloseProgressPopup_CSS));
                closeProgress.Click();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Exception khi mở Youtube Studio
        // 1. Xác minh danh tính - IdentityVerification
        // 2. Kênh bị logout - LogoutVerification
        // 3. Kênh bị die - DieChannelVerification
        private bool IdentityVerification(ChromeDriver driver)
        {
            try
            {
                var email = driver.FindElement(By.CssSelector(ExceptionChannel.Identity_Verification_Email_CSS)).Text;
                var errorMessage = driver.FindElement(By.CssSelector(ExceptionChannel.Identity_Verification_Err_Mess_CSS)).Text;
                if (errorMessage != null)
                {
                    var mess = ExceptionChannel.Identity_Verification_Mess + $" Email: {email}";
                    throw new Exception(mess);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void LogoutVerification()
        {
            try
            {

            }
            catch
            {
            }
        }

        private void DieChannelVerification()
        {
            try
            {

            }
            catch
            {
            }
        }

        // Exception Video
        private bool CheckInvalidInputText(ChromeDriver driver, string input, int maxInput, int lamda, string counterCSS, string inputTextCSS)
        {
            bool _isWarring = false;
            bool _isValid = false;
            int _maxInput = maxInput;
            // Cho những trường hợp với kí tự đặc biệt mà Length không giải quyết được
            int _lamda = lamda;
            do
            {
                try
                {
                    _isWarring = true;
                    var counter = driver.FindElement(By.CssSelector(counterCSS)).Text;
                    if (counter != null)
                    {
                        var titleInputText = driver.FindElement(By.CssSelector(inputTextCSS));
                        titleInputText.Clear();
                        titleInputText.SendKeys(input.Substring(0, _maxInput));
                        _maxInput -= _lamda;
                    }
                }
                catch
                {
                    _isValid = !_isValid;
                    _isWarring = false;
                }
            } while (_isValid);
            return _isWarring;
        }

        //Check exist elements
        public bool IsElementPresent(ChromeDriver driver, By by)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            try
            {
                var ele = driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public async Task PostRequest(string url, StringContent data)
        {
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            string result = response.Content.ReadAsStringAsync().Result;

        }
        public async Task RandomUploadPost(string channelID)
        {

            Random random = new Random();

            List<string> listChannelID = new List<string>();

            listChannelID.Add(channelID);

            const string chars = "qwertyuiopasdfghjklzxcvbnm0123456789";
            string stringRandomId = new string(Enumerable.Repeat(chars, 20).Select(s => s[random.Next(s.Length)]).ToArray());

            var randomUploadJobInput = new RandomUploadJobInput();
            randomUploadJobInput.UserId = "801391c1-ac24-4593-b97e-7575752a3fb5";
            randomUploadJobInput.ProfilePath = _input.ProfilePath;
            randomUploadJobInput.Timer = 0;

        }

    }
}

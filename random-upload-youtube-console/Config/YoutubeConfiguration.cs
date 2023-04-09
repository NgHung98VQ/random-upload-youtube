using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace random_upload_youtube_console.Config
{
    public static class YoutubeConfiguration
    {
        public static string CreateVideoBtn_ID = "create-icon";
        public static string UploadVideoBtn_ID = "text-item-0";
        public static string FileInput_Name = "Filedata";
        public static string ProgressStatus_ID = "progress-status-0";
        public static string TitleInput_CSS = "ytcp-social-suggestions-textbox#title-textarea div#textbox";
        public static string DescriptionInput_CSS = "ytcp-social-suggestions-textbox#description-textarea div#textbox";
        public static string ThumbImage_ID = "file-loader";
        public static string PlaylistsDropdown_CSS = "ytcp-dropdown-trigger[class='use-placeholder style-scope ytcp-text-dropdown-trigger style-scope ytcp-text-dropdown-trigger']";
        public static string DonePLLBtn_CSS = "ytcp-button[class='done-button action-button style-scope ytcp-playlist-dialog']";
        public static string VideoNotForKid_Name = "VIDEO_MADE_FOR_KIDS_NOT_MFK";
        public static string VideoForKid_Name = "VIDEO_MADE_FOR_KIDS_MFK";
        public static string ShowMoreBtn_CSS = "ytcp-button#toggle-button";
        public static string TagInput_CSS = "ytcp-form-input-container#tags-container input";
        public static string CategoryBtn_CSS = "ytcp-form-select#category";
        public static string NextBtn_ID = "next-button";
        public static string CalenderBtn_Name = "SCHEDULE";
        public static string DateBtn_ID = "datepicker-trigger";
        public static string DateInput_CSS = "div[class='style-scope ytcp-date-picker'] input";
        public static string TimeInput_CSS = "ytcp-form-input-container#time-of-day-container input";
        public static string TimeZoneBtn_ID = "timezone-select-button";
        public static string PremiereVideoCheckbox_ID = "schedule-type-checkbox";
        public static string DoneBtn_ID = "done-button";
        public static string CloseBtn_CSS = "div[class='footer style-scope ytcp-dialog'] ytcp-button#close-button";
        public static string ProgressElement_CSS = "tp-yt-paper-progress[class='style-scope ytcp-multi-progress-monitor']";
        public static string ProgressElement_ID = "progress-status-0";
        public static string CloseProgressPopup_CSS = "ytcp-icon-button[class='style-scope ytcp-multi-progress-monitor']";
        public static string ClearTagCacheBtn_ID = "clear-button";
        public static string TittleMonetization_CSS = "button[test-id='MONETIZATION']";
        public static string AllNoneCheckBox_CSS = "ytcp-checkbox-lit[class='all-none-checkbox style-scope ytpp-self-certification-questionnaire']";
        public static string SendContentBtn_ID = "submit-questionnaire-button";
        public static string ExportBtn_ID = "secondary-action-button";
        public static string DialogUploadVideo_CSS = "div[class='video-thumbnail-container style-scope ytcp-video-thumbnail-with-info']";
        public static string ListLanguageBtn_CSS = "ytcp-form-language-input#language-input";
        public static string RightsManagementBtn_CSS = "button[test-id='RIGHTS_MANAGEMENT']";
        public static string AssetInformationDrop_CSS = "#trigger div.has-label.container.style-scope.ytcp-dropdown-trigger.style-scope.ytcp-dropdown-trigger";
        public static string WebAssetInformation_CSS = "tp-yt-paper-item[test-id='ASSET_TYPE_WEB']";
        public static string MusicVideoAssetInformation_CSS = "tp-yt-paper-item[test-id='ASSET_TYPE_MUSIC_VIDEO']";
        public static string TVEpisodeAssetInformation_CSS = "tp-yt-paper-item[test-id='ASSET_TYPE_EPISODE']";
        public static string MovieAssetInformation_CSS = "tp-yt-paper-item[test-id='ASSET_TYPE_MOVIE']";
        public static string ContentStudioBtn_CSS = "#menu-paper-icon-item-1";
        public static string UploadBadge_CSS = "ytcp-video-row #uploading-badge";
        public static string CheckBadge_CSS = "ytcp-video-row #checks-badge";
        public static string CancelUploadBtn_CSS = "ytcp-button.cancel-upload-button.style-scope.ytcp-video-list-cell-actions";
        public static string EditDrafBtn_CSS = "ytcp-button.edit-draft-button.style-scope.ytcp-video-list-cell-actions";
        public static string ConfirmCancelUploadBtn_CSS = "#confirm-button";
    }

    public static class DisplayVideoMode
    {
        public static string Private_Name = "PRIVATE";
        public static string Public_Name = "PUBLIC";
        public static string Unlisted_Name = "UNLISTED";
    }

    public static class ExceptionChannel
    {
        public static string Identity_Verification_Email_CSS = "div[class='wLBAL']";
        public static string Identity_Verification_Err_Mess_CSS = "h1[class='ahT6S '] span";
        public static string Identity_Verification_Mess = "Xác minh danh tính của bạn";
    }

    public static class ExceptionVideo
    {
        public static string CounterTitleVideo_CSS = "ytcp-social-suggestions-textbox[id='title-textarea'] div[class='char-counter style-scope ytcp-social-suggestions-textbox']";
        public static string CounterDescriptionVideo_CSS = "ytcp-social-suggestions-textbox[id='description-textarea'] div[class='char-counter style-scope ytcp-social-suggestions-textbox']";
        public static int MaxTag = 500;
        public static int MaxTitle = 100;
        public static int MaxDescription = 5000;
        public static int LambdaTitle = 10;
        public static int LambdaDescription = 500;
    }
}

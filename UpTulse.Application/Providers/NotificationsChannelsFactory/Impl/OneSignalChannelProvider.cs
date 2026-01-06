using System.Text;

using Newtonsoft.Json;

using UpTulse.Application.Models;
using UpTulse.Application.Providers.NotificationsFactory;
using UpTulse.DataAccess.EnvironmentVariables;

namespace UpTulse.Application.Providers.NotificationsChannelsFactory.Impl
{
    public class OneSignalChannelProvider : INotificationChannelProvider
    {
        private readonly string _androidChannelId;
        private readonly string _androidCriticalChannelId;
        private readonly string _oneSignalApiKey;
        private readonly string _oneSignalAppId;
        private readonly string _oneSignalIosCategory;
        private readonly string _oneSignalUrl;

        public OneSignalChannelProvider()
        {
            _oneSignalApiKey = Environment.GetEnvironmentVariable(NotificationsEnv.ONESIGNAL_API_KEY) ?? string.Empty;
            _oneSignalAppId = Environment.GetEnvironmentVariable(NotificationsEnv.ONESIGNAL_APP_ID) ?? string.Empty;
            _oneSignalUrl = Environment.GetEnvironmentVariable(NotificationsEnv.ONESIGNAL_URL) ?? string.Empty;
            _androidChannelId = Environment.GetEnvironmentVariable(NotificationsEnv.ANDROID_CHANNEL_ID) ?? string.Empty;
            _androidCriticalChannelId = Environment.GetEnvironmentVariable(NotificationsEnv.ANDROID_CRITICAL_CHANNEL_ID) ?? string.Empty;
            _oneSignalIosCategory = Environment.GetEnvironmentVariable(NotificationsEnv.IOS_CATEGORY) ?? string.Empty;
        }

        public async Task<bool> SendNotification(NotificationContext context)
        {
            var jsonPayload = BuildAndGetJsonOfRequestModel(context);

            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(6000)
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, _oneSignalUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Authorization", _oneSignalApiKey);
            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        private string BuildAndGetJsonOfRequestModel(NotificationContext context)
        {
            var isCritical = context.IsUp ? context.IsAvailabilityCritical : context.IsUnavailabilityCritical;

            var payload = new PushNotificationRequest
            {
                AppId = _oneSignalAppId,
                Contents = new Contents { En = context.Body },
                Headings = new Headings { En = context.Subject },
                Name = context.Subject,
                Subtitle = new Subtitle { En = context.Body },
                IosCategory = _oneSignalIosCategory,
                AndroidChannelId = isCritical ? _androidCriticalChannelId : _androidChannelId,
                IosSound = isCritical ? "critical.wav" : "default"
            };

            return JsonConvert.SerializeObject(payload);
        }
    }
}
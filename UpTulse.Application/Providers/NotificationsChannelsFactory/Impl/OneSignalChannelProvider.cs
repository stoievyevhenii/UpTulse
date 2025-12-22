using UpTulse.Application.EnvironmentVariables;
using UpTulse.Application.Models;
using UpTulse.Application.Providers.NotificationsFactory;

namespace UpTulse.Application.Providers.NotificationsChannelsFactory.Impl
{
    public class OneSignalChannelProvider : INotificationChannelProvider
    {
        private readonly string _oneSignalApiKey;
        private readonly string _oneSignalAppId;
        private readonly string _oneSignalUrl;

        public OneSignalChannelProvider()
        {
            _oneSignalApiKey = Environment.GetEnvironmentVariable(NotificationsEnv.ONESIGNAL_API_KEY) ?? string.Empty;
            _oneSignalAppId = Environment.GetEnvironmentVariable(NotificationsEnv.ONESIGNAL_APP_ID) ?? string.Empty;
            _oneSignalUrl = Environment.GetEnvironmentVariable(NotificationsEnv.ONESIGNAL_URL) ?? string.Empty;
        }

        public Task<bool> SendNotification(NotificationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
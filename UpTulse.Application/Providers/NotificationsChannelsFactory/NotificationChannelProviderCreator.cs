using UpTulse.Application.Providers.NotificationsFactory;

namespace UpTulse.Application.Providers.NotificationsChannelsFactory
{
    public abstract class NotificationChannelProviderCreator
    {
        private INotificationChannelProvider _notificationChannelsProvider = default!;

        public abstract INotificationChannelProvider CreateNotificationChannelsProvider();

        public Task SendMessage(string message)
        {
            _notificationChannelsProvider ??= CreateNotificationChannelsProvider();
            return _notificationChannelsProvider.SendNotification(message);
        }
    }
}
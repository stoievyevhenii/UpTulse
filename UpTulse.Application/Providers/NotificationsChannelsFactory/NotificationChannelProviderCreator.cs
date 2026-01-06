using UpTulse.Application.Models;
using UpTulse.Application.Providers.NotificationsFactory;

namespace UpTulse.Application.Providers.NotificationsChannelsFactory
{
    public abstract class NotificationChannelProviderCreator
    {
        private INotificationChannelProvider _notificationChannelsProvider = default!;

        public abstract INotificationChannelProvider CreateNotificationChannelsProvider();

        public Task SendMessageAsync(NotificationContext context)
        {
            _notificationChannelsProvider ??= CreateNotificationChannelsProvider();
            return _notificationChannelsProvider.SendNotification(context);
        }
    }
}
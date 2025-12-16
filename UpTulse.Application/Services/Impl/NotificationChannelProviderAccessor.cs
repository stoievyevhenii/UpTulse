using UpTulse.Application.Providers.NotificationsChannelsFactory;
using UpTulse.Application.Providers.NotificationsChannelsFactory.Creators;
using UpTulse.Shared.Enums;

namespace UpTulse.Application.Services.Impl
{
    public class NotificationChannelProviderAccessor : INotificationChannelProviderAccessor
    {
        public NotificationChannelProviderCreator GetProviderCreator(NotificationChannel channel)
        {
            return channel switch
            {
                NotificationChannel.Email => new EmailChannelProviderCreator(),
                NotificationChannel.None => new EmptyChannelProviderCreator(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
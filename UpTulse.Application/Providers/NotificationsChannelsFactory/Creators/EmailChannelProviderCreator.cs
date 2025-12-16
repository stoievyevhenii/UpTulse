using UpTulse.Application.Providers.NotificationsChannelsFactory.Impl;
using UpTulse.Application.Providers.NotificationsFactory;

namespace UpTulse.Application.Providers.NotificationsChannelsFactory.Creators
{
    public class EmailChannelProviderCreator : NotificationChannelProviderCreator
    {
        public override INotificationChannelProvider CreateNotificationChannelsProvider()
        {
            return new EmailChannelProvider();
        }
    }
}
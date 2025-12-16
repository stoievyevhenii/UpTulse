using UpTulse.Application.Providers.NotificationsChannelsFactory;
using UpTulse.Shared.Enums;

namespace UpTulse.Application.Services
{
    public interface INotificationChannelProviderAccessor
    {
        NotificationChannelProviderCreator GetProviderCreator(NotificationChannel channel);
    }
}
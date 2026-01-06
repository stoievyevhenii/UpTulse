using UpTulse.Application.Models;

namespace UpTulse.Application.Providers.NotificationsFactory
{
    public interface INotificationChannelProvider
    {
        Task<bool> SendNotification(NotificationContext context);
    }
}
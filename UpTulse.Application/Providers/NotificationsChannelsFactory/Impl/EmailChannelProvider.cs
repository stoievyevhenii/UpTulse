using UpTulse.Application.Providers.NotificationsFactory;

namespace UpTulse.Application.Providers.NotificationsChannelsFactory.Impl
{
    public class EmailChannelProvider : INotificationChannelProvider
    {
        public Task<bool> SendNotification(string message)
        {
            Console.WriteLine($"Email notification sent with message: {message}");
            return Task.FromResult(true);
        }
    }
}
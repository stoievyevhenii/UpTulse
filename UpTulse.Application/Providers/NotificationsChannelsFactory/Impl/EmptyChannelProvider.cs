using UpTulse.Application.Providers.NotificationsFactory;

namespace UpTulse.Application.Providers.NotificationsChannelsFactory.Impl
{
    public class EmptyChannelProvider : INotificationChannelProvider
    {
        public Task<bool> SendNotification(string message)
        {
            Console.WriteLine($"No notification will be send");
            return Task.FromResult(true);
        }
    }
}
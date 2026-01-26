using UpTulse.Application.Providers.NotificationsChannelsFactory.Impl;
using UpTulse.Application.Providers.NotificationsFactory;
using UpTulse.Application.Services;

namespace UpTulse.Application.Providers.NotificationsChannelsFactory.Creators
{
    public class EmailChannelProviderCreator : NotificationChannelProviderCreator
    {
        private readonly IMonitoringTargetService _monitoringTargetService;

        public EmailChannelProviderCreator(IMonitoringTargetService monitoringTargetService)
        {
            _monitoringTargetService = monitoringTargetService;
        }

        public override INotificationChannelProvider CreateNotificationChannelsProvider()
        {
            return new EmailChannelProvider(_monitoringTargetService);
        }
    }
}
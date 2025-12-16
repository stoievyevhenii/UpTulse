using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Application.Providers.NotificationsFactory
{
    public interface INotificationChannelProvider
    {
        Task<bool> SendNotification(string message);
    }
}
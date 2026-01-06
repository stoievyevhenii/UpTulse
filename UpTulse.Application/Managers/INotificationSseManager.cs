using System.Runtime.CompilerServices;
using System.Threading.Channels;

using UpTulse.Application.Models;

namespace UpTulse.Application.Managers
{
    public interface INotificationSseManager
    {
        Task BroadcastAsync(MonitoringResult result);

        IAsyncEnumerable<MonitoringResult> ReadStream(Guid clientId, ChannelReader<MonitoringResult> reader, CancellationToken ct);

        IAsyncEnumerable<MonitoringResult> Subscribe(CancellationToken ct);
    }
}
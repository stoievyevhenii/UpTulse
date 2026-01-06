using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

using UpTulse.Application.Models;

namespace UpTulse.Application.Managers.Impl
{
    public class NotificationSseManager : INotificationSseManager
    {
        private readonly ConcurrentDictionary<Guid, ChannelWriter<MonitoringResult>> _clients = new();

        public async Task BroadcastAsync(MonitoringResult result)
        {
            foreach (var client in _clients)
            {
                await client.Value.WriteAsync(result);
            }
        }

        public async IAsyncEnumerable<MonitoringResult> ReadStream(Guid clientId, ChannelReader<MonitoringResult> reader, [EnumeratorCancellation] CancellationToken ct)
        {
            try
            {
                while (await reader.WaitToReadAsync(ct))
                {
                    while (reader.TryRead(out var message))
                    {
                        yield return message;
                    }
                }
            }
            finally
            {
                _clients.TryRemove(clientId, out _);
            }
        }

        public IAsyncEnumerable<MonitoringResult> Subscribe(CancellationToken ct)
        {
            var clientId = Guid.NewGuid();
            var channel = Channel.CreateUnbounded<MonitoringResult>();

            _clients.TryAdd(clientId, channel.Writer);

            return ReadStream(clientId, channel.Reader, ct);
        }
    }
}
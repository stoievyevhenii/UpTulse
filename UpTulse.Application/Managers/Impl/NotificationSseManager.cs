using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace UpTulse.Application.Managers.Impl
{
    public class NotificationSseManager : INotificationSseManager
    {
        private readonly ConcurrentDictionary<Guid, ChannelWriter<string>> _clients = new();

        public async Task BroadcastAsync(string eventData)
        {
            foreach (var client in _clients)
            {
                await client.Value.WriteAsync(eventData);
            }
        }

        public async IAsyncEnumerable<string> ReadStream(Guid clientId, ChannelReader<string> reader, [EnumeratorCancellation] CancellationToken ct)
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

        public IAsyncEnumerable<string> Subscribe(CancellationToken ct)
        {
            var clientId = Guid.NewGuid();
            var channel = Channel.CreateUnbounded<string>();

            _clients.TryAdd(clientId, channel.Writer);

            return ReadStream(clientId, channel.Reader, ct);
        }
    }
}
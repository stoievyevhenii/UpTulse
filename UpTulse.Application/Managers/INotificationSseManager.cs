using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace UpTulse.Application.Managers
{
    public interface INotificationSseManager
    {
        Task BroadcastAsync(string eventData);

        IAsyncEnumerable<string> ReadStream(Guid clientId, ChannelReader<string> reader, [EnumeratorCancellation] CancellationToken ct);

        IAsyncEnumerable<string> Subscribe(CancellationToken ct);
    }
}
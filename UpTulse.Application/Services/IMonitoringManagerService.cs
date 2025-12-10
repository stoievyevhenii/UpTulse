using System.Threading.Channels;

using UpTulse.Application.Models;

namespace UpTulse.Application.Services
{
    public interface IMonitoringManagerService
    {
        ChannelReader<MonitoringOperation> OperationReader { get; }

        Task AddTargetAsync(MonitoringTargetRequest target);

        IEnumerable<MonitoringTargetRequest> GetTargets();

        Task RemoveTargetAsync(string name);
    }
}
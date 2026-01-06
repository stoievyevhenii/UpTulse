using System.Threading.Channels;

using UpTulse.Application.Models;

namespace UpTulse.Application.Managers
{
    public interface IMonitoringTargetsManager
    {
        ChannelReader<MonitoringOperation> OperationReader { get; }

        Task AddOrUpdateExistTargetAsync(MonitoringTargetRequest target);

        IEnumerable<MonitoringTargetRequest> GetTargets();

        Task RemoveTargetAsync(string name);
    }
}
using System.Collections.Concurrent;
using System.Threading.Channels;

using UpTulse.Application.Enums;
using UpTulse.Application.Models;

namespace UpTulse.Application.Managers.Impl
{
    public class MonitoringTargetsManager : IMonitoringTargetsManager
    {
        private readonly Channel<MonitoringOperation> _channel;
        private readonly ConcurrentDictionary<string, MonitoringTargetRequest> _targets;

        public MonitoringTargetsManager()
        {
            _targets = [];
            _channel = Channel.CreateUnbounded<MonitoringOperation>();
        }

        public ChannelReader<MonitoringOperation> OperationReader => _channel.Reader;

        public async Task AddOrUpdateExistTargetAsync(MonitoringTargetRequest target)
        {
            _targets.AddOrUpdate(target.Name, target, (_, _) => target);
            await _channel.Writer.WriteAsync(new MonitoringOperation(target, OperationType.Add));
        }

        public IEnumerable<MonitoringTargetRequest> GetTargets() => _targets.Values;

        public async Task RemoveTargetAsync(string name)
        {
            if (_targets.TryRemove(name, out var target))
            {
                await _channel.Writer.WriteAsync(new MonitoringOperation(target, OperationType.Remove));
            }
        }
    }
}
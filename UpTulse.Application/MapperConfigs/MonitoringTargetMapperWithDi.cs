using Facet.Extensions;
using Facet.Mapping;

using UpTulse.Application.Models;
using UpTulse.Application.Services;
using UpTulse.Core.Entities;

namespace UpTulse.Application.MapperConfigs
{
    public class MonitoringTargetMapperWithDi : IFacetMapConfigurationAsyncInstance<MonitoringTarget, MonitoringTargetResponse>
    {
        private readonly IMonitoringGroupService _monitoringGroupService;

        public MonitoringTargetMapperWithDi(IMonitoringGroupService monitoringGroupService)
        {
            _monitoringGroupService = monitoringGroupService;
        }

        public async Task MapAsync(MonitoringTarget source, MonitoringTargetResponse target, CancellationToken cancellationToken = default)
        {
            if (source.GroupId == Guid.Empty)
            {
                return;
            }

            var group = await _monitoringGroupService.GetByIdAsync(source.GroupId);

            if (group is null)
            {
                return;
            }

            var monitoringGroupFromFacetToSource = new MonitoringGroup();

            monitoringGroupFromFacetToSource.ApplyFacet(group);

            target.Group = monitoringGroupFromFacetToSource;
        }
    }
}
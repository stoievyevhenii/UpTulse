using UpTulse.Core.Entities;
using UpTulse.DataAccess.Persistence;

namespace UpTulse.DataAccess.Repositories.Impl
{
    public class MonitoringGroupRepository : BaseRepository<MonitoringGroup>, IMonitoringGroupRepository
    {
        public MonitoringGroupRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
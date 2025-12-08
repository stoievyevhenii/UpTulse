using UpTulse.Core.Entities;
using UpTulse.DataAccess.Persistence;

namespace UpTulse.DataAccess.Repositories.Impl
{
    public class MonitoringTargetRepository : BaseRepository<MonitoringTarget>, IMonitoringTargetRepository
    {
        public MonitoringTargetRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
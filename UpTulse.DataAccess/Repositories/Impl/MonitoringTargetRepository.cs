using UpTulse.Core.Entities;
using UpTulse.DataAccess.Persistence;

namespace UpTulse.DataAccess.Repositories.Impl
{
    public class MonitoringTargetRepository : BaseRepository<MonitoringTarget>, IMonitoringTargetRepository
    {
        protected MonitoringTargetRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
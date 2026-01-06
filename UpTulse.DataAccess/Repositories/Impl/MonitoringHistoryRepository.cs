using UpTulse.Core.Entities;
using UpTulse.DataAccess.Persistence;

namespace UpTulse.DataAccess.Repositories.Impl
{
    public class MonitoringHistoryRepository : BaseRepository<MonitoringHistory>, IMonitoringHistoryRepository
    {
        public MonitoringHistoryRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
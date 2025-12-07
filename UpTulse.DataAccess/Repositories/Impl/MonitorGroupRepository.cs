using UpTulse.Core.Entities;
using UpTulse.DataAccess.Persistence;

namespace UpTulse.DataAccess.Repositories.Impl
{
    public class MonitorGroupRepository : BaseRepository<MonitoringGroup>, IMonitorGroupRepository
    {
        protected MonitorGroupRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
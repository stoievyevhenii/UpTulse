using UpTulse.Core.Entities;
using UpTulse.DataAccess.Persistence;

namespace UpTulse.DataAccess.Repositories.Impl
{
    public class SettingRepository : BaseRepository<Settings>, ISettingRepository
    {
        public SettingRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
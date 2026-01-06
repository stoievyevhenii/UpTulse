using System.Reflection;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using UpTulse.Core.Common;
using UpTulse.Core.Entities;
using UpTulse.DataAccess.Identity;
using UpTulse.Shared.Services;

namespace UpTulse.DataAccess.Persistence
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IClaimService _claimService;

        public DatabaseContext(DbContextOptions options, IClaimService claimService) : base(options)
        {
            _claimService = claimService;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<MonitoringGroup> MonitoringGroups { get; set; }
        public DbSet<MonitoringHistory> MonitoringHistories { get; set; }
        public DbSet<MonitoringTarget> MonitoringTargets { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _claimService.GetUserId();
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
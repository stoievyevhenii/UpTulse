using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UpTulse.Core.Entities;

namespace UpTulse.DataAccess.Persistence.Configurations
{
    internal class MonitoringGroupConfiguration : IEntityTypeConfiguration<MonitoringGroup>
    {
        public void Configure(EntityTypeBuilder<MonitoringGroup> builder)
        {
            builder.Property(s => s.Name)
                .IsRequired();
        }
    }
}
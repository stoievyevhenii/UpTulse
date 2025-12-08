using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UpTulse.Core.Entities;

namespace UpTulse.DataAccess.Persistence.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.Property(s => s.Name)
                .IsRequired();

            builder.Property(s => s.Value)
                .IsRequired();
        }
    }
}
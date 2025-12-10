using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UpTulse.Core.Entities;

namespace UpTulse.DataAccess.Persistence.Configurations
{
    public class MonitoringTargetConfiguration : IEntityTypeConfiguration<MonitoringTarget>
    {
        public void Configure(EntityTypeBuilder<MonitoringTarget> builder)
        {
            builder.Property(s => s.Address)
                .IsRequired();

            builder.Property(s => s.Name)
                .IsRequired();
        }
    }
}
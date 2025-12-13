using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpTulse.DataAccess.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedMonitoringHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitoringHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    IsUp = table.Column<bool>(type: "boolean", nullable: false),
                    MonitoringTargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponseTimeInMs = table.Column<float>(type: "real", nullable: false),
                    TimeStamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringHistories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitoringHistories");
        }
    }
}

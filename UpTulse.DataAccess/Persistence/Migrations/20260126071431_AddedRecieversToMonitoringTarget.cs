using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpTulse.DataAccess.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedRecieversToMonitoringTarget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "Receivers",
                table: "MonitoringTargets",
                type: "text[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receivers",
                table: "MonitoringTargets");
        }
    }
}

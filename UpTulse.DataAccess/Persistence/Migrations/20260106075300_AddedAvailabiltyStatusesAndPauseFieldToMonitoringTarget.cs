using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpTulse.DataAccess.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedAvailabiltyStatusesAndPauseFieldToMonitoringTarget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailabilityCritical",
                table: "MonitoringTargets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUnavailabilityCritical",
                table: "MonitoringTargets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Paused",
                table: "MonitoringTargets",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailabilityCritical",
                table: "MonitoringTargets");

            migrationBuilder.DropColumn(
                name: "IsUnavailabilityCritical",
                table: "MonitoringTargets");

            migrationBuilder.DropColumn(
                name: "Paused",
                table: "MonitoringTargets");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpTulse.DataAccess.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecieversToMonitoringTargetNull : Migration
    {
        /// <inheritdoc />
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receivers",
                table: "MonitoringTargets");
        }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "Receivers",
                table: "MonitoringTargets",
                type: "text[]",
                defaultValue: new List<string> { "" },
                nullable: true);
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UpTulse.DataAccess.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "MonitoringHistories",
                newName: "StartTimeStamp");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndTimeStamp",
                table: "MonitoringHistories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTimeStamp",
                table: "MonitoringHistories");

            migrationBuilder.RenameColumn(
                name: "StartTimeStamp",
                table: "MonitoringHistories",
                newName: "TimeStamp");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChgManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWebsiteSettingsServirce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WednesdayService",
                table: "WebsiteSettings",
                newName: "Tuesday_Thursday_Service");

            migrationBuilder.RenameColumn(
                name: "FridayService",
                table: "WebsiteSettings",
                newName: "SaturdayService");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tuesday_Thursday_Service",
                table: "WebsiteSettings",
                newName: "WednesdayService");

            migrationBuilder.RenameColumn(
                name: "SaturdayService",
                table: "WebsiteSettings",
                newName: "FridayService");
        }
    }
}

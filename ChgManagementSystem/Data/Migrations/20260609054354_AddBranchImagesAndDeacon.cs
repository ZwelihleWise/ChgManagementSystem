using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChgManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchImagesAndDeacon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeaconEmail",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeaconImagePath",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeaconName",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeaconPhone",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeaconEmail",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DeaconImagePath",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DeaconName",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DeaconPhone",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Branches");
        }
    }
}

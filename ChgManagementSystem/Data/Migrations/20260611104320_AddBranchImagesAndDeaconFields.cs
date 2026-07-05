using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChgManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchImagesAndDeaconFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeaconEmail",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "DeaconPhone",
                table: "Branches",
                newName: "DeaconInformation");

            migrationBuilder.RenameColumn(
                name: "DeaconName",
                table: "Branches",
                newName: "BranchNewsletter");

            migrationBuilder.RenameColumn(
                name: "DeaconImagePath",
                table: "Branches",
                newName: "BranchImage");

            migrationBuilder.CreateTable(
                name: "BranchLeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchLeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchLeaders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchLeaders_BranchId",
                table: "BranchLeaders",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchLeaders");

            migrationBuilder.RenameColumn(
                name: "DeaconInformation",
                table: "Branches",
                newName: "DeaconPhone");

            migrationBuilder.RenameColumn(
                name: "BranchNewsletter",
                table: "Branches",
                newName: "DeaconName");

            migrationBuilder.RenameColumn(
                name: "BranchImage",
                table: "Branches",
                newName: "DeaconImagePath");

            migrationBuilder.AddColumn<string>(
                name: "DeaconEmail",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

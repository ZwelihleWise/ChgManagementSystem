using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChgManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMonthlyOfferings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyOfferings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    OfferingTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyOfferings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyOfferings_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOfferings_OfferingTypes_OfferingTypeId",
                        column: x => x.OfferingTypeId,
                        principalTable: "OfferingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOfferings_MemberId",
                table: "MonthlyOfferings",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOfferings_OfferingTypeId",
                table: "MonthlyOfferings",
                column: "OfferingTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyOfferings");
        }
    }
}

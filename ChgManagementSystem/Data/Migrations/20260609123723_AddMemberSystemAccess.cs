using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChgManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberSystemAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSystemAccess",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSystemAccess",
                table: "Members");
        }
    }
}

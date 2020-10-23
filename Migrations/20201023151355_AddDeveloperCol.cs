using Microsoft.EntityFrameworkCore.Migrations;

namespace Blocks.Migrations
{
    public partial class AddDeveloperCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Developer",
                table: "EmailUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Developer",
                table: "EmailUsers");
        }
    }
}

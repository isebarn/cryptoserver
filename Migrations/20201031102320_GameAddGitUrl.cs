using Microsoft.EntityFrameworkCore.Migrations;

namespace Blocks.Migrations
{
    public partial class GameAddGitUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Github",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Github",
                table: "Games");
        }
    }
}

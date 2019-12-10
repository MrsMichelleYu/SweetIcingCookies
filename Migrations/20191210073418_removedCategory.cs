using Microsoft.EntityFrameworkCore.Migrations;

namespace CSharpProject.Migrations
{
    public partial class removedCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Products",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CSharpProject.Migrations
{
    public partial class ExpDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "Customers",
                newName: "Year");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Customers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Customers",
                newName: "ExpirationDate");
        }
    }
}

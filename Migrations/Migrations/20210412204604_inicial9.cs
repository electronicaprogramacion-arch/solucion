using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Manufacturer");

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "Manufacturer",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delete",
                table: "Manufacturer");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Manufacturer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

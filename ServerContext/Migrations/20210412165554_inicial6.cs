using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}

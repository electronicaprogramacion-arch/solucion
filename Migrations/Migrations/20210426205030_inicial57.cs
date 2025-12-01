using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial57 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poscition",
                table: "TestPoint");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "TestPoint",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "TestPoint");

            migrationBuilder.AddColumn<int>(
                name: "Poscition",
                table: "TestPoint",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

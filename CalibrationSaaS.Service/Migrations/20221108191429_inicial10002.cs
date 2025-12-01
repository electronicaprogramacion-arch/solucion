using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial10002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Equipmet",
                table: "RockwellResult");

            migrationBuilder.DropColumn(
                name: "FS",
                table: "RockwellResult");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Equipmet",
                table: "RockwellResult",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "FS",
                table: "RockwellResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

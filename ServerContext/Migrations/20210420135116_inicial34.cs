using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToleranceAsFound",
                table: "BasicCalibrationResult");

            migrationBuilder.DropColumn(
                name: "ToleranceAsLeft",
                table: "BasicCalibrationResult");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ToleranceAsFound",
                table: "BasicCalibrationResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ToleranceAsLeft",
                table: "BasicCalibrationResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

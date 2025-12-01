using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Multiplier",
                table: "WorkOrderDetail",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MaxToleranceAsLeft",
                table: "Linearity",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinToleranceAsLeft",
                table: "Linearity",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Multiplier",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "MaxToleranceAsLeft",
                table: "Linearity");

            migrationBuilder.DropColumn(
                name: "MinToleranceAsLeft",
                table: "Linearity");
        }
    }
}

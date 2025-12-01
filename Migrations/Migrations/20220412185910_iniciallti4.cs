using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class iniciallti4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeightApplied",
                table: "ForceResult");

            migrationBuilder.AddColumn<double>(
                name: "TemperatureAfter",
                table: "WorkOrderDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemperatureAfter",
                table: "WorkOrderDetail");

            migrationBuilder.AddColumn<double>(
                name: "WeightApplied",
                table: "ForceResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

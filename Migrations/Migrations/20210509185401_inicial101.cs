using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "EquipmentTemplate");

            migrationBuilder.AddColumn<int>(
                name: "ClassHB44",
                table: "WorkOrderDetail",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassHB44",
                table: "EquipmentTemplate",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassHB44",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "ClassHB44",
                table: "EquipmentTemplate");

            migrationBuilder.AddColumn<int>(
                name: "Class",
                table: "WorkOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Class",
                table: "EquipmentTemplate",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

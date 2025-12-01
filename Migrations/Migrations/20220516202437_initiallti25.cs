using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class initiallti25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForceCalibrationSubTypeId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForceSequenceID",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForceWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Weight_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                columns: new[] { "ForceSequenceID", "ForceCalibrationSubTypeId", "ForceWorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Weight_Force_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                columns: new[] { "ForceSequenceID", "ForceCalibrationSubTypeId", "ForceWorkOrderDetailId" },
                principalTable: "Force",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Weight_Force_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Weight_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "ForceCalibrationSubTypeId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "ForceSequenceID",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "ForceWorkOrderDetailId",
                table: "CalibrationSubType_Weight");
        }
    }
}

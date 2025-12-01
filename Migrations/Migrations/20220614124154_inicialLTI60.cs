using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicialLTI60 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalibrationSubTypeID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CalibrationTypeID",
                table: "EquipmentType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentType_CalibrationTypeID",
                table: "EquipmentType",
                column: "CalibrationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_CalibrationTypeId",
                table: "CalibrationSubType",
                column: "CalibrationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_CalibrationType_CalibrationTypeId",
                table: "CalibrationSubType",
                column: "CalibrationTypeId",
                principalTable: "CalibrationType",
                principalColumn: "CalibrationTypeId",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_EquipmentType_CalibrationType_CalibrationTypeID",
            //    table: "EquipmentType",
            //    column: "CalibrationTypeID",
            //    principalTable: "CalibrationType",
            //    principalColumn: "CalibrationTypeId",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_CalibrationType_CalibrationTypeId",
                table: "CalibrationSubType");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentType_CalibrationType_CalibrationTypeID",
                table: "EquipmentType");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentType_CalibrationTypeID",
                table: "EquipmentType");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_CalibrationTypeId",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "CalibrationSubTypeID",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "CalibrationTypeID",
                table: "EquipmentType");
        }
    }
}

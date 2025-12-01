using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicialLTI61 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_EquipmentType_CalibrationType_CalibrationTypeID",
            //    table: "EquipmentType");

            migrationBuilder.AlterColumn<int>(
                name: "CalibrationTypeID",
                table: "EquipmentType",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentType_CalibrationType_CalibrationTypeID",
                table: "EquipmentType",
                column: "CalibrationTypeID",
                principalTable: "CalibrationType",
                principalColumn: "CalibrationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentType_CalibrationType_CalibrationTypeID",
                table: "EquipmentType");

            migrationBuilder.AlterColumn<int>(
                name: "CalibrationTypeID",
                table: "EquipmentType",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentType_CalibrationType_CalibrationTypeID",
                table: "EquipmentType",
                column: "CalibrationTypeID",
                principalTable: "CalibrationType",
                principalColumn: "CalibrationTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

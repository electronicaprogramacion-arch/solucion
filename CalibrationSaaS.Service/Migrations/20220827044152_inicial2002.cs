using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial2002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCode_UnitOfMeasure_UnitOfMeasureID",
                table: "TestCode");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrderDetail_TestCode_TestCodeID",
                table: "WorkOrderDetail");

            migrationBuilder.AlterColumn<int>(
                name: "TestCodeID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UnitOfMeasureID",
                table: "TestCode",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TestCode_CalibrationTypeID",
                table: "TestCode",
                column: "CalibrationTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCode_CalibrationType_CalibrationTypeID",
                table: "TestCode",
                column: "CalibrationTypeID",
                principalTable: "CalibrationType",
                principalColumn: "CalibrationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCode_UnitOfMeasure_UnitOfMeasureID",
                table: "TestCode",
                column: "UnitOfMeasureID",
                principalTable: "UnitOfMeasure",
                principalColumn: "UnitOfMeasureID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrderDetail_TestCode_TestCodeID",
                table: "WorkOrderDetail",
                column: "TestCodeID",
                principalTable: "TestCode",
                principalColumn: "TestCodeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCode_CalibrationType_CalibrationTypeID",
                table: "TestCode");

            migrationBuilder.DropForeignKey(
                name: "FK_TestCode_UnitOfMeasure_UnitOfMeasureID",
                table: "TestCode");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrderDetail_TestCode_TestCodeID",
                table: "WorkOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_TestCode_CalibrationTypeID",
                table: "TestCode");

            migrationBuilder.AlterColumn<int>(
                name: "TestCodeID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitOfMeasureID",
                table: "TestCode",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TestCode_UnitOfMeasure_UnitOfMeasureID",
                table: "TestCode",
                column: "UnitOfMeasureID",
                principalTable: "UnitOfMeasure",
                principalColumn: "UnitOfMeasureID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrderDetail_TestCode_TestCodeID",
                table: "WorkOrderDetail",
                column: "TestCodeID",
                principalTable: "TestCode",
                principalColumn: "TestCodeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

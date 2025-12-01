using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3124 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationCalibrationSubTypeId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationSequenceID",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationWorkOrderDetailId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationCalibrationSubTypeId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationSequenceID",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationCalibrationSubTypeId",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationSequenceID",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationWorkOrderDetailId",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GenericCalibration",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    NumberOfSamples = table.Column<int>(type: "int", nullable: false),
                    TestPointID = table.Column<int>(type: "int", nullable: true),
                    UnitOfMeasureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericCalibration", x => new { x.SequenceID, x.CalibrationSubTypeId, x.WorkOrderDetailId });
                    table.ForeignKey(
                        name: "FK_GenericCalibration_TestPoint_TestPointID",
                        column: x => x.TestPointID,
                        principalTable: "TestPoint",
                        principalColumn: "TestPointID");
                });

            migrationBuilder.CreateTable(
                name: "GenericCalibrationResult",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Resolution = table.Column<double>(type: "float", nullable: false),
                    DecimalNumber = table.Column<int>(type: "int", nullable: false),
                    Object = table.Column<string>(type: "varchar(max)", unicode: false, maxLength: 10000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericCalibrationResult", x => new { x.SequenceID, x.CalibrationSubTypeId, x.WorkOrderDetailId });
                    table.ForeignKey(
                        name: "FK_GenericCalibrationResult_GenericCalibration_SequenceID_CalibrationSubTypeId_WorkOrderDetailId",
                        columns: x => new { x.SequenceID, x.CalibrationSubTypeId, x.WorkOrderDetailId },
                        principalTable: "GenericCalibration",
                        principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightSet_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderDetailId",
                table: "WeightSet",
                columns: new[] { "GenericCalibrationSequenceID", "GenericCalibrationCalibrationSubTypeId", "GenericCalibrationWorkOrderDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Weight_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderDet~",
                table: "CalibrationSubType_Weight",
                columns: new[] { "GenericCalibrationSequenceID", "GenericCalibrationCalibrationSubTypeId", "GenericCalibrationWorkOrderDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Standard_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderD~",
                table: "CalibrationSubType_Standard",
                columns: new[] { "GenericCalibrationSequenceID", "GenericCalibrationCalibrationSubTypeId", "GenericCalibrationWorkOrderDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_GenericCalibration_TestPointID",
                table: "GenericCalibration",
                column: "TestPointID");

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Standard_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCa~",
                table: "CalibrationSubType_Standard",
                columns: new[] { "GenericCalibrationSequenceID", "GenericCalibrationCalibrationSubTypeId", "GenericCalibrationWorkOrderDetailId" },
                principalTable: "GenericCalibration",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Weight_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCali~",
                table: "CalibrationSubType_Weight",
                columns: new[] { "GenericCalibrationSequenceID", "GenericCalibrationCalibrationSubTypeId", "GenericCalibrationWorkOrderDetailId" },
                principalTable: "GenericCalibration",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WeightSet_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrder~",
                table: "WeightSet",
                columns: new[] { "GenericCalibrationSequenceID", "GenericCalibrationCalibrationSubTypeId", "GenericCalibrationWorkOrderDetailId" },
                principalTable: "GenericCalibration",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Standard_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCa~",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Weight_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCali~",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightSet_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrder~",
                table: "WeightSet");

            migrationBuilder.DropTable(
                name: "GenericCalibrationResult");

            migrationBuilder.DropTable(
                name: "GenericCalibration");

            migrationBuilder.DropIndex(
                name: "IX_WeightSet_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Weight_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderDet~",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Standard_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderD~",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "GenericCalibrationCalibrationSubTypeId",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "GenericCalibrationSequenceID",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "GenericCalibrationWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "GenericCalibrationCalibrationSubTypeId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "GenericCalibrationSequenceID",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "GenericCalibrationWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "GenericCalibrationCalibrationSubTypeId",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "GenericCalibrationSequenceID",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "GenericCalibrationWorkOrderDetailId",
                table: "CalibrationSubType_Standard");
        }
    }
}

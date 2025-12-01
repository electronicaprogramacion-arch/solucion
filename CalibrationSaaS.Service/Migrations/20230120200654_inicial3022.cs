using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MicroCalibrationSubTypeId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicroSequenceID",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicroWorkOrderDetailId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicroCalibrationSubTypeId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicroSequenceID",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicroWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicroCalibrationSubTypeId",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicroSequenceID",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MicroWorkOrderDetailId",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Micro",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    NumberOfSamples = table.Column<int>(type: "int", nullable: false),
                    TestPointID = table.Column<int>(type: "int", nullable: true),
                    UnitOfMeasureId = table.Column<int>(type: "int", nullable: true),
                    CalibrationUncertaintyValueUnitOfMeasureId = table.Column<int>(type: "int", nullable: true),
                    UncertaintyID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Micro", x => new { x.SequenceID, x.CalibrationSubTypeId, x.WorkOrderDetailId });
                    table.ForeignKey(
                        name: "FK_Micro_TestPoint_TestPointID",
                        column: x => x.TestPointID,
                        principalTable: "TestPoint",
                        principalColumn: "TestPointID");
                });

            migrationBuilder.CreateTable(
                name: "WOD_Standard",
                columns: table => new
                {
                    WorkOrderDetailID = table.Column<int>(type: "int", nullable: false),
                    PieceOfEquipmentID = table.Column<string>(type: "varchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOD_Standard", x => new { x.WorkOrderDetailID, x.PieceOfEquipmentID });
                    table.ForeignKey(
                        name: "FK_WOD_Standard_PieceOfEquipment_PieceOfEquipmentID",
                        column: x => x.PieceOfEquipmentID,
                        principalTable: "PieceOfEquipment",
                        principalColumn: "PieceOfEquipmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WOD_Standard_WorkOrderDetail_WorkOrderDetailID",
                        column: x => x.WorkOrderDetailID,
                        principalTable: "WorkOrderDetail",
                        principalColumn: "WorkOrderDetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MicroResult",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    NominalValue = table.Column<double>(type: "float", nullable: false),
                    Test1X = table.Column<double>(type: "float", nullable: false),
                    Test1Y = table.Column<double>(type: "float", nullable: false),
                    Test2X = table.Column<double>(type: "float", nullable: false),
                    Test2Y = table.Column<double>(type: "float", nullable: false),
                    Test3X = table.Column<double>(type: "float", nullable: false),
                    Test3Y = table.Column<double>(type: "float", nullable: false),
                    Test4X = table.Column<double>(type: "float", nullable: false),
                    Test4Y = table.Column<double>(type: "float", nullable: false),
                    Test5X = table.Column<double>(type: "float", nullable: false),
                    Test5Y = table.Column<double>(type: "float", nullable: false),
                    Error = table.Column<double>(type: "float", nullable: false),
                    ErrorPer = table.Column<double>(type: "float", nullable: false),
                    Repeateability = table.Column<double>(type: "float", nullable: false),
                    RepeateabilityPer = table.Column<double>(type: "float", nullable: false),
                    Uncertanty = table.Column<double>(type: "float", nullable: false),
                    Standard = table.Column<double>(type: "float", nullable: false),
                    UnBias = table.Column<double>(type: "float", nullable: false),
                    Average = table.Column<double>(type: "float", nullable: false),
                    AveragePer = table.Column<double>(type: "float", nullable: false),
                    Resolution = table.Column<double>(type: "float", nullable: false),
                    DecimalNumber = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Max = table.Column<double>(type: "float", nullable: false),
                    Min = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicroResult", x => new { x.SequenceID, x.CalibrationSubTypeId, x.WorkOrderDetailId });
                    table.ForeignKey(
                        name: "FK_MicroResult_Micro_SequenceID_CalibrationSubTypeId_WorkOrderDetailId",
                        columns: x => new { x.SequenceID, x.CalibrationSubTypeId, x.WorkOrderDetailId },
                        principalTable: "Micro",
                        principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightSet_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "WeightSet",
                columns: new[] { "MicroSequenceID", "MicroCalibrationSubTypeId", "MicroWorkOrderDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Weight_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                columns: new[] { "MicroSequenceID", "MicroCalibrationSubTypeId", "MicroWorkOrderDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Standard_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "CalibrationSubType_Standard",
                columns: new[] { "MicroSequenceID", "MicroCalibrationSubTypeId", "MicroWorkOrderDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_Micro_TestPointID",
                table: "Micro",
                column: "TestPointID");

            migrationBuilder.CreateIndex(
                name: "IX_WOD_Standard_PieceOfEquipmentID",
                table: "WOD_Standard",
                column: "PieceOfEquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Standard_Micro_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "CalibrationSubType_Standard",
                columns: new[] { "MicroSequenceID", "MicroCalibrationSubTypeId", "MicroWorkOrderDetailId" },
                principalTable: "Micro",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Weight_Micro_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                columns: new[] { "MicroSequenceID", "MicroCalibrationSubTypeId", "MicroWorkOrderDetailId" },
                principalTable: "Micro",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WeightSet_Micro_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "WeightSet",
                columns: new[] { "MicroSequenceID", "MicroCalibrationSubTypeId", "MicroWorkOrderDetailId" },
                principalTable: "Micro",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Standard_Micro_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Weight_Micro_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightSet_Micro_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropTable(
                name: "MicroResult");

            migrationBuilder.DropTable(
                name: "WOD_Standard");

            migrationBuilder.DropTable(
                name: "Micro");

            migrationBuilder.DropIndex(
                name: "IX_WeightSet_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Weight_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Standard_MicroSequenceID_MicroCalibrationSubTypeId_MicroWorkOrderDetailId",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "MicroCalibrationSubTypeId",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "MicroSequenceID",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "MicroWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "MicroCalibrationSubTypeId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "MicroSequenceID",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "MicroWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "MicroCalibrationSubTypeId",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "MicroSequenceID",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "MicroWorkOrderDetailId",
                table: "CalibrationSubType_Standard");
        }
    }
}

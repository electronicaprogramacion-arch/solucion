using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class InitialLTI3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForceCalibrationSubTypeId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForceSequenceID",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForceWorkOrderDetailId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "DeviceClass",
            //    table: "EquipmentTemplate",
            //    type: "varchar(100)",
            //    unicode: false,
            //    maxLength: 100,
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "PlatformSize",
            //    table: "EquipmentTemplate",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "ForceResult",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    FS = table.Column<double>(type: "float", nullable: false),
                    Equipmet = table.Column<int>(type: "int", nullable: false),
                    Nominal = table.Column<double>(type: "float", nullable: false),
                    RUN1 = table.Column<double>(type: "float", nullable: false),
                    RUN2 = table.Column<double>(type: "float", nullable: false),
                    RUN3 = table.Column<double>(type: "float", nullable: false),
                    RUN4 = table.Column<double>(type: "float", nullable: false),
                    Error = table.Column<double>(type: "float", nullable: false),
                    ErrorPer = table.Column<double>(type: "float", nullable: false),
                    Uncertanty = table.Column<double>(type: "float", nullable: false),
                    RelativeIndicationError = table.Column<double>(type: "float", nullable: false),
                    RelativeRepeatabilityError = table.Column<double>(type: "float", nullable: false),
                    Class = table.Column<double>(type: "float", nullable: false),
                    Resolution = table.Column<double>(type: "float", nullable: false),
                    DecimalNumber = table.Column<int>(type: "int", nullable: false),
                    WeightApplied = table.Column<double>(type: "float", nullable: false),
                    IncludeASTM = table.Column<double>(type: "float", nullable: false),
                    ErrorRun1 = table.Column<double>(type: "float", nullable: false),
                    ErrorRun2 = table.Column<double>(type: "float", nullable: false),
                    ErrorRun3 = table.Column<double>(type: "float", nullable: false),
                    ErrorRun4 = table.Column<double>(type: "float", nullable: false),
                    RelativeIndicationErrorR1 = table.Column<double>(type: "float", nullable: false),
                    RelativeIndicationErrorR2 = table.Column<double>(type: "float", nullable: false),
                    RelativeIndicationErrorR3 = table.Column<double>(type: "float", nullable: false),
                    RelativeIndicationErrorR4 = table.Column<double>(type: "float", nullable: false),
                    ZeroReturn = table.Column<double>(type: "float", nullable: false),
                    RelativeResolution = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForceResult", x => new { x.SequenceID, x.CalibrationSubTypeId, x.WorkOrderDetailId });
                });

            migrationBuilder.CreateTable(
                name: "Force",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    Tolerance = table.Column<double>(type: "float", nullable: false),
                    NumberOfSamples = table.Column<int>(type: "int", nullable: false),
                    TestPointID = table.Column<int>(type: "int", nullable: false),
                    Uncertainty = table.Column<double>(type: "float", nullable: false),
                    UnitOfMeasureId = table.Column<int>(type: "int", nullable: true),
                    CalibrationUncertaintyValueUnitOfMeasureId = table.Column<int>(type: "int", nullable: true),
                    MinTolerance = table.Column<double>(type: "float", nullable: false),
                    MaxTolerance = table.Column<double>(type: "float", nullable: false),
                    ISO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Force", x => new { x.SequenceID, x.CalibrationSubTypeId, x.WorkOrderDetailId });
                    table.ForeignKey(
                        name: "FK_Force_ForceResult_SequenceID_CalibrationSubTypeId_WorkOrderDetailId",
                        columns: x => new { x.SequenceID, x.CalibrationSubTypeId, x.WorkOrderDetailId },
                        principalTable: "ForceResult",
                        principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });
                    table.ForeignKey(
                        name: "FK_Force_TestPoint_TestPointID",
                        column: x => x.TestPointID,
                        principalTable: "TestPoint",
                        principalColumn: "TestPointID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightSet_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "WeightSet",
                columns: new[] { "ForceSequenceID", "ForceCalibrationSubTypeId", "ForceWorkOrderDetailId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Certificate_WorkOrderDetailId",
            //    table: "Certificate",
            //    column: "WorkOrderDetailId",
            //    unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Force_TestPointID",
                table: "Force",
                column: "TestPointID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Certificate_WorkOrderDetail_WorkOrderDetailId",
            //    table: "Certificate",
            //    column: "WorkOrderDetailId",
            //    principalTable: "WorkOrderDetail",
            //    principalColumn: "WorkOrderDetailID",
            //    onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightSet_Force_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "WeightSet",
                columns: new[] { "ForceSequenceID", "ForceCalibrationSubTypeId", "ForceWorkOrderDetailId" },
                principalTable: "Force",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_WorkOrderDetail_WorkOrderDetailId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightSet_Force_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropTable(
                name: "Force");

            migrationBuilder.DropTable(
                name: "ForceResult");

            migrationBuilder.DropIndex(
                name: "IX_WeightSet_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_WorkOrderDetailId",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "ForceCalibrationSubTypeId",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "ForceSequenceID",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "ForceWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "DeviceClass",
                table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                name: "PlatformSize",
                table: "EquipmentTemplate");
        }
    }
}

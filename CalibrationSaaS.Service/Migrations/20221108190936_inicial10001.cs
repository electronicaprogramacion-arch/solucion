using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial10001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RockwellSequenceID",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RockwellSequenceID",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RockwellResult",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    FS = table.Column<double>(type: "float", nullable: false),
                    Equipmet = table.Column<int>(type: "int", nullable: false),
                    Nominal = table.Column<double>(type: "float", nullable: false),
                    ScaleRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Standard = table.Column<double>(type: "float", nullable: false),
                    Average = table.Column<double>(type: "float", nullable: false),
                    Test1 = table.Column<double>(type: "float", nullable: false),
                    Test2 = table.Column<double>(type: "float", nullable: false),
                    Test3 = table.Column<double>(type: "float", nullable: false),
                    Test4 = table.Column<double>(type: "float", nullable: false),
                    Test5 = table.Column<double>(type: "float", nullable: false),
                    Error = table.Column<double>(type: "float", nullable: false),
                    Repeateability = table.Column<double>(type: "float", nullable: false),
                    Uncertanty = table.Column<double>(type: "float", nullable: false),
                    Resolution = table.Column<double>(type: "float", nullable: false),
                    DecimalNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockwellResult", x => x.SequenceID);
                });

            migrationBuilder.CreateTable(
                name: "Rockwell",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    BasicCalibrationResultSequenceID = table.Column<int>(type: "int", nullable: false),
                    Tolerance = table.Column<double>(type: "float", nullable: false),
                    NumberOfSamples = table.Column<int>(type: "int", nullable: false),
                    TestPointID = table.Column<int>(type: "int", nullable: false),
                    Uncertainty = table.Column<double>(type: "float", nullable: false),
                    UnitOfMeasureId = table.Column<int>(type: "int", nullable: true),
                    CalibrationUncertaintyValueUnitOfMeasureId = table.Column<int>(type: "int", nullable: true),
                    MinTolerance = table.Column<double>(type: "float", nullable: false),
                    MaxTolerance = table.Column<double>(type: "float", nullable: false),
                    UncertaintyID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rockwell", x => x.SequenceID);
                    table.ForeignKey(
                        name: "FK_Rockwell_RockwellResult_BasicCalibrationResultSequenceID",
                        column: x => x.BasicCalibrationResultSequenceID,
                        principalTable: "RockwellResult",
                        principalColumn: "SequenceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rockwell_TestPoint_TestPointID",
                        column: x => x.TestPointID,
                        principalTable: "TestPoint",
                        principalColumn: "TestPointID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightSet_RockwellSequenceID",
                table: "WeightSet",
                column: "RockwellSequenceID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Weight_RockwellSequenceID",
                table: "CalibrationSubType_Weight",
                column: "RockwellSequenceID");

            migrationBuilder.CreateIndex(
                name: "IX_Rockwell_BasicCalibrationResultSequenceID",
                table: "Rockwell",
                column: "BasicCalibrationResultSequenceID");

            migrationBuilder.CreateIndex(
                name: "IX_Rockwell_TestPointID",
                table: "Rockwell",
                column: "TestPointID");

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Weight_Rockwell_RockwellSequenceID",
                table: "CalibrationSubType_Weight",
                column: "RockwellSequenceID",
                principalTable: "Rockwell",
                principalColumn: "SequenceID");

            migrationBuilder.AddForeignKey(
                name: "FK_WeightSet_Rockwell_RockwellSequenceID",
                table: "WeightSet",
                column: "RockwellSequenceID",
                principalTable: "Rockwell",
                principalColumn: "SequenceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Weight_Rockwell_RockwellSequenceID",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightSet_Rockwell_RockwellSequenceID",
                table: "WeightSet");

            migrationBuilder.DropTable(
                name: "Rockwell");

            migrationBuilder.DropTable(
                name: "RockwellResult");

            migrationBuilder.DropIndex(
                name: "IX_WeightSet_RockwellSequenceID",
                table: "WeightSet");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Weight_RockwellSequenceID",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "RockwellSequenceID",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "RockwellSequenceID",
                table: "CalibrationSubType_Weight");
        }
    }
}

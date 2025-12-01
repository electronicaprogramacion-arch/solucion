using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalibrationSubType_Standard",
                columns: table => new
                {
                    WorkOrderDetailID = table.Column<int>(type: "int", nullable: false),
                    PieceOfEquipmentID = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    CalibrationSubTypeID = table.Column<int>(type: "int", nullable: false),
                    SecuenceID = table.Column<int>(type: "int", nullable: false),
                    RockwellCalibrationSubTypeId = table.Column<int>(type: "int", nullable: true),
                    RockwellSequenceID = table.Column<int>(type: "int", nullable: true),
                    RockwellWorkOrderDetailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationSubType_Standard", x => new { x.WorkOrderDetailID, x.PieceOfEquipmentID, x.CalibrationSubTypeID, x.SecuenceID });
                    table.ForeignKey(
                        name: "FK_CalibrationSubType_Standard_CalibrationSubType_CalibrationSubTypeID",
                        column: x => x.CalibrationSubTypeID,
                        principalTable: "CalibrationSubType",
                        principalColumn: "CalibrationSubTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalibrationSubType_Standard_PieceOfEquipment_PieceOfEquipmentID",
                        column: x => x.PieceOfEquipmentID,
                        principalTable: "PieceOfEquipment",
                        principalColumn: "PieceOfEquipmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalibrationSubType_Standard_Rockwell_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId",
                        columns: x => new { x.RockwellSequenceID, x.RockwellCalibrationSubTypeId, x.RockwellWorkOrderDetailId },
                        principalTable: "Rockwell",
                        principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Standard_CalibrationSubTypeID",
                table: "CalibrationSubType_Standard",
                column: "CalibrationSubTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Standard_PieceOfEquipmentID",
                table: "CalibrationSubType_Standard",
                column: "PieceOfEquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Standard_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId",
                table: "CalibrationSubType_Standard",
                columns: new[] { "RockwellSequenceID", "RockwellCalibrationSubTypeId", "RockwellWorkOrderDetailId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalibrationSubType_Standard");
        }
    }
}

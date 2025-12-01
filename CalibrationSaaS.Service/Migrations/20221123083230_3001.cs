using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class _3001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Weight_Rockwell_RockwellSequenceID",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropForeignKey(
                name: "FK_Rockwell_RockwellResult_BasicCalibrationResultSequenceID",
                table: "Rockwell");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightSet_Rockwell_RockwellSequenceID",
                table: "WeightSet");

            migrationBuilder.DropIndex(
                name: "IX_WeightSet_RockwellSequenceID",
                table: "WeightSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RockwellResult",
                table: "RockwellResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rockwell",
                table: "Rockwell");

            migrationBuilder.DropIndex(
                name: "IX_Rockwell_BasicCalibrationResultSequenceID",
                table: "Rockwell");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Weight_RockwellSequenceID",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "BasicCalibrationResultSequenceID",
                table: "Rockwell");

            migrationBuilder.AddColumn<int>(
                name: "RockwellCalibrationSubTypeId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RockwellWorkOrderDetailId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "SequenceID",
            //    table: "RockwellResult",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AlterColumn<int>(
            //    name: "SequenceID",
            //    table: "Rockwell",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddColumn<bool>(
            //    name: "HasScales",
            //    table: "EquipmentType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RockwellCalibrationSubTypeId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RockwellWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RockwellResult",
                table: "RockwellResult",
                columns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rockwell",
                table: "Rockwell",
                columns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.CreateTable(
                name: "POE_Scale",
                columns: table => new
                {
                    PieceOfEquipmentID = table.Column<string>(type: "varchar(500)", nullable: false),
                    Scale = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POE_Scale", x => new { x.PieceOfEquipmentID, x.Scale });
                    table.ForeignKey(
                        name: "FK_POE_Scale_PieceOfEquipment_PieceOfEquipmentID",
                        column: x => x.PieceOfEquipmentID,
                        principalTable: "PieceOfEquipment",
                        principalColumn: "PieceOfEquipmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightSet_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId",
                table: "WeightSet",
                columns: new[] { "RockwellSequenceID", "RockwellCalibrationSubTypeId", "RockwellWorkOrderDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Weight_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                columns: new[] { "RockwellSequenceID", "RockwellCalibrationSubTypeId", "RockwellWorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Weight_Rockwell_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                columns: new[] { "RockwellSequenceID", "RockwellCalibrationSubTypeId", "RockwellWorkOrderDetailId" },
                principalTable: "Rockwell",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RockwellResult_Rockwell_SequenceID_CalibrationSubTypeId_WorkOrderDetailId",
                table: "RockwellResult",
                columns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" },
                principalTable: "Rockwell",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WeightSet_Rockwell_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId",
                table: "WeightSet",
                columns: new[] { "RockwellSequenceID", "RockwellCalibrationSubTypeId", "RockwellWorkOrderDetailId" },
                principalTable: "Rockwell",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Weight_Rockwell_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropForeignKey(
                name: "FK_RockwellResult_Rockwell_SequenceID_CalibrationSubTypeId_WorkOrderDetailId",
                table: "RockwellResult");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightSet_Rockwell_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropTable(
                name: "POE_Scale");

            migrationBuilder.DropIndex(
                name: "IX_WeightSet_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RockwellResult",
                table: "RockwellResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rockwell",
                table: "Rockwell");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Weight_RockwellSequenceID_RockwellCalibrationSubTypeId_RockwellWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "RockwellCalibrationSubTypeId",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "RockwellWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "HasScales",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "RockwellCalibrationSubTypeId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "RockwellWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.AlterColumn<int>(
                name: "SequenceID",
                table: "RockwellResult",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "SequenceID",
                table: "Rockwell",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "BasicCalibrationResultSequenceID",
                table: "Rockwell",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RockwellResult",
                table: "RockwellResult",
                column: "SequenceID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rockwell",
                table: "Rockwell",
                column: "SequenceID");

            migrationBuilder.CreateIndex(
                name: "IX_WeightSet_RockwellSequenceID",
                table: "WeightSet",
                column: "RockwellSequenceID");

            migrationBuilder.CreateIndex(
                name: "IX_Rockwell_BasicCalibrationResultSequenceID",
                table: "Rockwell",
                column: "BasicCalibrationResultSequenceID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Weight_RockwellSequenceID",
                table: "CalibrationSubType_Weight",
                column: "RockwellSequenceID");

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Weight_Rockwell_RockwellSequenceID",
                table: "CalibrationSubType_Weight",
                column: "RockwellSequenceID",
                principalTable: "Rockwell",
                principalColumn: "SequenceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rockwell_RockwellResult_BasicCalibrationResultSequenceID",
                table: "Rockwell",
                column: "BasicCalibrationResultSequenceID",
                principalTable: "RockwellResult",
                principalColumn: "SequenceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightSet_Rockwell_RockwellSequenceID",
                table: "WeightSet",
                column: "RockwellSequenceID",
                principalTable: "Rockwell",
                principalColumn: "SequenceID");
        }
    }
}

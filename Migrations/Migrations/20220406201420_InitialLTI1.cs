using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class InitialLTI1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Force_ForceResult_BasicCalibrationResultSequenceID",
                table: "Force");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightSet_Force_ForceSequenceID",
                table: "WeightSet");

            migrationBuilder.DropIndex(
                name: "IX_WeightSet_ForceSequenceID",
                table: "WeightSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForceResult",
                table: "ForceResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Force",
                table: "Force");

            migrationBuilder.DropIndex(
                name: "IX_Force_BasicCalibrationResultSequenceID",
                table: "Force");

            migrationBuilder.DropColumn(
                name: "BasicCalibrationResultSequenceID",
                table: "Force");

            migrationBuilder.AddColumn<int>(
                name: "ForceCalibrationSubTypeId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForceWorkOrderDetailId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SequenceID",
                table: "ForceResult",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "SequenceID",
                table: "Force",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForceResult",
                table: "ForceResult",
                columns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Force",
                table: "Force",
                columns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_WeightSet_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "WeightSet",
                columns: new[] { "ForceSequenceID", "ForceCalibrationSubTypeId", "ForceWorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Force_ForceResult_SequenceID_CalibrationSubTypeId_WorkOrderDetailId",
                table: "Force",
                columns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" },
                principalTable: "ForceResult",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

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
                name: "FK_Force_ForceResult_SequenceID_CalibrationSubTypeId_WorkOrderDetailId",
                table: "Force");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightSet_Force_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropIndex(
                name: "IX_WeightSet_ForceSequenceID_ForceCalibrationSubTypeId_ForceWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForceResult",
                table: "ForceResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Force",
                table: "Force");

            migrationBuilder.DropColumn(
                name: "ForceCalibrationSubTypeId",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "ForceWorkOrderDetailId",
                table: "WeightSet");

            migrationBuilder.AlterColumn<int>(
                name: "SequenceID",
                table: "ForceResult",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "SequenceID",
                table: "Force",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "BasicCalibrationResultSequenceID",
                table: "Force",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForceResult",
                table: "ForceResult",
                column: "SequenceID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Force",
                table: "Force",
                column: "SequenceID");

            migrationBuilder.CreateIndex(
                name: "IX_WeightSet_ForceSequenceID",
                table: "WeightSet",
                column: "ForceSequenceID");

            migrationBuilder.CreateIndex(
                name: "IX_Force_BasicCalibrationResultSequenceID",
                table: "Force",
                column: "BasicCalibrationResultSequenceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Force_ForceResult_BasicCalibrationResultSequenceID",
                table: "Force",
                column: "BasicCalibrationResultSequenceID",
                principalTable: "ForceResult",
                principalColumn: "SequenceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightSet_Force_ForceSequenceID",
                table: "WeightSet",
                column: "ForceSequenceID",
                principalTable: "Force",
                principalColumn: "SequenceID");
        }
    }
}

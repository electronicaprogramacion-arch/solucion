using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tolerance",
                table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                name: "ToleranceType",
                table: "EquipmentTemplate");

            migrationBuilder.AddColumn<int>(
                name: "PieceOfEquipmentID",
                table: "RangeTolerance",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceID",
                table: "RangeTolerance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "AccuracyPercentage",
                table: "PieceOfEquipment",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DecimalNumber",
                table: "PieceOfEquipment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Resolution",
                table: "PieceOfEquipment",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ToleranceTypeID",
                table: "PieceOfEquipment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RangeTolerance_PieceOfEquipmentID",
                table: "RangeTolerance",
                column: "PieceOfEquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_RangeTolerance_PieceOfEquipment_PieceOfEquipmentID",
                table: "RangeTolerance",
                column: "PieceOfEquipmentID",
                principalTable: "PieceOfEquipment",
                principalColumn: "PieceOfEquipmentID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RangeTolerance_PieceOfEquipment_PieceOfEquipmentID",
                table: "RangeTolerance");

            migrationBuilder.DropIndex(
                name: "IX_RangeTolerance_PieceOfEquipmentID",
                table: "RangeTolerance");

            migrationBuilder.DropColumn(
                name: "PieceOfEquipmentID",
                table: "RangeTolerance");

            migrationBuilder.DropColumn(
                name: "SourceID",
                table: "RangeTolerance");

            migrationBuilder.DropColumn(
                name: "AccuracyPercentage",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "DecimalNumber",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "ToleranceTypeID",
                table: "PieceOfEquipment");

            migrationBuilder.AddColumn<double>(
                name: "Tolerance",
                table: "EquipmentTemplate",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ToleranceType",
                table: "EquipmentTemplate",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

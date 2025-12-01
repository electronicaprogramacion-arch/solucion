using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class HB1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PieceOfEquipmentID",
                table: "TestPointGroup",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassHB44",
                table: "PieceOfEquipment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTestPointImport",
                table: "PieceOfEquipment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TestPointGroup_PieceOfEquipmentID",
                table: "TestPointGroup",
                column: "PieceOfEquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_TestPointGroup_PieceOfEquipment_PieceOfEquipmentID",
                table: "TestPointGroup",
                column: "PieceOfEquipmentID",
                principalTable: "PieceOfEquipment",
                principalColumn: "PieceOfEquipmentID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestPointGroup_PieceOfEquipment_PieceOfEquipmentID",
                table: "TestPointGroup");

            migrationBuilder.DropIndex(
                name: "IX_TestPointGroup_PieceOfEquipmentID",
                table: "TestPointGroup");

            migrationBuilder.DropColumn(
                name: "PieceOfEquipmentID",
                table: "TestPointGroup");

            migrationBuilder.DropColumn(
                name: "ClassHB44",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "IsTestPointImport",
                table: "PieceOfEquipment");
        }
    }
}

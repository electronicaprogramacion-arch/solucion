using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial105 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_PieceOfEquipment_PieceOfEquipmentId",
                table: "Certificate");

            migrationBuilder.RenameColumn(
                name: "PieceOfEquipmentId",
                table: "Certificate",
                newName: "PieceOfEquipmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Certificate_PieceOfEquipmentId",
                table: "Certificate",
                newName: "IX_Certificate_PieceOfEquipmentID");

            migrationBuilder.AlterColumn<int>(
                name: "PieceOfEquipmentID",
                table: "Certificate",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_PieceOfEquipment_PieceOfEquipmentID",
                table: "Certificate",
                column: "PieceOfEquipmentID",
                principalTable: "PieceOfEquipment",
                principalColumn: "PieceOfEquipmentID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_PieceOfEquipment_PieceOfEquipmentID",
                table: "Certificate");

            migrationBuilder.RenameColumn(
                name: "PieceOfEquipmentID",
                table: "Certificate",
                newName: "PieceOfEquipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificate_PieceOfEquipmentID",
                table: "Certificate",
                newName: "IX_Certificate_PieceOfEquipmentId");

            migrationBuilder.AlterColumn<int>(
                name: "PieceOfEquipmentId",
                table: "Certificate",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_PieceOfEquipment_PieceOfEquipmentId",
                table: "Certificate",
                column: "PieceOfEquipmentId",
                principalTable: "PieceOfEquipment",
                principalColumn: "PieceOfEquipmentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

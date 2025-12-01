using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial106 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_PieceOfEquipment_PieceOfEquipmentID",
                table: "Certificate");

            //migrationBuilder.DropIndex(
            //    name: "IX_Certificate_PieceOfEquipmentID",
            //    table: "Certificate");

            migrationBuilder.DropColumn(
                name: "PieceOfEquipmentID",
                table: "Certificate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PieceOfEquipmentID",
                table: "Certificate",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_PieceOfEquipmentID",
                table: "Certificate",
                column: "PieceOfEquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_PieceOfEquipment_PieceOfEquipmentID",
                table: "Certificate",
                column: "PieceOfEquipmentID",
                principalTable: "PieceOfEquipment",
                principalColumn: "PieceOfEquipmentID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

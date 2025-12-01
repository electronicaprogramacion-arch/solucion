using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial802 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CertificatePoE_PieceOfEquipmentID",
                table: "CertificatePoE",
                column: "PieceOfEquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificatePoE_PieceOfEquipment_PieceOfEquipmentID",
                table: "CertificatePoE",
                column: "PieceOfEquipmentID",
                principalTable: "PieceOfEquipment",
                principalColumn: "PieceOfEquipmentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificatePoE_PieceOfEquipment_PieceOfEquipmentID",
                table: "CertificatePoE");

            migrationBuilder.DropIndex(
                name: "IX_CertificatePoE_PieceOfEquipmentID",
                table: "CertificatePoE");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial804 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificatePoE_PieceOfEquipment_PieceOfEquipmentID",
                table: "CertificatePoE");

            migrationBuilder.AlterColumn<string>(
                name: "PieceOfEquipmentID",
                table: "CertificatePoE",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CertificatePoE_PieceOfEquipment_PieceOfEquipmentID",
                table: "CertificatePoE",
                column: "PieceOfEquipmentID",
                principalTable: "PieceOfEquipment",
                principalColumn: "PieceOfEquipmentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificatePoE_PieceOfEquipment_PieceOfEquipmentID",
                table: "CertificatePoE");

            migrationBuilder.AlterColumn<string>(
                name: "PieceOfEquipmentID",
                table: "CertificatePoE",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500);

            migrationBuilder.AddForeignKey(
                name: "FK_CertificatePoE_PieceOfEquipment_PieceOfEquipmentID",
                table: "CertificatePoE",
                column: "PieceOfEquipmentID",
                principalTable: "PieceOfEquipment",
                principalColumn: "PieceOfEquipmentID");
        }
    }
}

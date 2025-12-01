using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial800 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CertificatePoE",
                columns: table => new
                {
                    CertificateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CalibrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectDueDate = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    PieceOfEquipmentID = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificatePoE", x => x.CertificateNumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificatePoE");
        }
    }
}

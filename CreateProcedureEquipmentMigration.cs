using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    /// <summary>
    /// Migration to create ProcedureEquipment table
    /// This migration creates the missing table that was causing DbSet errors
    /// </summary>
    public partial class CreateProcedureEquipmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create ProcedureEquipment table
            migrationBuilder.CreateTable(
                name: "ProcedureEquipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureID = table.Column<int>(type: "int", nullable: false),
                    PieceOfEquipmentID = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureEquipment_Procedure_ProcedureID",
                        column: x => x.ProcedureID,
                        principalTable: "Procedure",
                        principalColumn: "ProcedureID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcedureEquipment_PieceOfEquipment_PieceOfEquipmentID",
                        column: x => x.PieceOfEquipmentID,
                        principalTable: "PieceOfEquipment",
                        principalColumn: "PieceOfEquipmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create unique index to prevent duplicate associations
            migrationBuilder.CreateIndex(
                name: "IX_ProcedureEquipment_Unique",
                table: "ProcedureEquipment",
                columns: new[] { "ProcedureID", "PieceOfEquipmentID" },
                unique: true);

            // Create additional indexes for performance
            migrationBuilder.CreateIndex(
                name: "IX_ProcedureEquipment_ProcedureID",
                table: "ProcedureEquipment",
                column: "ProcedureID");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureEquipment_PieceOfEquipmentID",
                table: "ProcedureEquipment",
                column: "PieceOfEquipmentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the ProcedureEquipment table
            migrationBuilder.DropTable(
                name: "ProcedureEquipment");
        }
    }
}

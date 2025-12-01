using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial500 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitOfMeasure_UnitOfMeasure_UncertaintyUnitOfMeasureID",
                table: "UnitOfMeasure");

            migrationBuilder.DropIndex(
                name: "IX_UnitOfMeasure_UncertaintyUnitOfMeasureID",
                table: "UnitOfMeasure");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasure_UncertaintyUnitOfMeasureID",
                table: "UnitOfMeasure",
                column: "UncertaintyUnitOfMeasureID",
                unique: true,
                filter: "[UncertaintyUnitOfMeasureID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitOfMeasure_UnitOfMeasure_UncertaintyUnitOfMeasureID",
                table: "UnitOfMeasure",
                column: "UncertaintyUnitOfMeasureID",
                principalTable: "UnitOfMeasure",
                principalColumn: "UnitOfMeasureID");
        }
    }
}

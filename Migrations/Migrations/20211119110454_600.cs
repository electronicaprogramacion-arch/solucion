using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class _600 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "WeightNominalValue",
                table: "WeightSet",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasure_UncertaintyUnitOfMeasureID",
                table: "UnitOfMeasure",
                column: "UncertaintyUnitOfMeasureID");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitOfMeasure_UnitOfMeasure_UncertaintyUnitOfMeasureID",
                table: "UnitOfMeasure",
                column: "UncertaintyUnitOfMeasureID",
                principalTable: "UnitOfMeasure",
                principalColumn: "UnitOfMeasureID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitOfMeasure_UnitOfMeasure_UncertaintyUnitOfMeasureID",
                table: "UnitOfMeasure");

            migrationBuilder.DropIndex(
                name: "IX_UnitOfMeasure_UncertaintyUnitOfMeasureID",
                table: "UnitOfMeasure");

            migrationBuilder.AlterColumn<int>(
                name: "WeightNominalValue",
                table: "WeightSet",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}

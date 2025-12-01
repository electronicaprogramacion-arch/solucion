using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class initiallti21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Force_Uncertainty_UncertaintyID",
            //    table: "Force");

            migrationBuilder.AlterColumn<int>(
                name: "UncertaintyID",
                table: "Force",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Force_Uncertainty_UncertaintyID",
            //    table: "Force",
            //    column: "UncertaintyID",
            //    principalTable: "Uncertainty",
            //    principalColumn: "UncertaintyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Force_Uncertainty_UncertaintyID",
                table: "Force");

            migrationBuilder.AlterColumn<int>(
                name: "UncertaintyID",
                table: "Force",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Force_Uncertainty_UncertaintyID",
                table: "Force",
                column: "UncertaintyID",
                principalTable: "Uncertainty",
                principalColumn: "UncertaintyID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

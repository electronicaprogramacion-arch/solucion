using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class initiallti22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uncertainty_EquipmentTemplate_EquipmentTemplateID",
                table: "Uncertainty");

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentTemplateID",
                table: "Uncertainty",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Uncertainty_EquipmentTemplate_EquipmentTemplateID",
                table: "Uncertainty",
                column: "EquipmentTemplateID",
                principalTable: "EquipmentTemplate",
                principalColumn: "EquipmentTemplateID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uncertainty_EquipmentTemplate_EquipmentTemplateID",
                table: "Uncertainty");

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentTemplateID",
                table: "Uncertainty",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Uncertainty_EquipmentTemplate_EquipmentTemplateID",
                table: "Uncertainty",
                column: "EquipmentTemplateID",
                principalTable: "EquipmentTemplate",
                principalColumn: "EquipmentTemplateID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

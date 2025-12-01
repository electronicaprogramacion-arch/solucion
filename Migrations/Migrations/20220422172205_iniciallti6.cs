using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class iniciallti6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsZeroReturn",
                table: "ForceResult");

            migrationBuilder.AddColumn<double>(
                name: "WeightNominalValue2",
                table: "WeightSet",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "HasAccredited",
                table: "EquipmentType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasClass",
                table: "EquipmentType",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeightNominalValue2",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "HasAccredited",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "HasClass",
                table: "EquipmentType");

            migrationBuilder.AddColumn<bool>(
                name: "IsZeroReturn",
                table: "ForceResult",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

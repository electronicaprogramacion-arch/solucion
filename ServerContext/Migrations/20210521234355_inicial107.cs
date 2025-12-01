using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial107 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delete",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "TechnicianCode");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "Manufacturer");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "Address");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "WeightSet",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "TechnicianCode",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "PieceOfEquipment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Manufacturer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "EquipmentTemplate",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Customer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Contact",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Address",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "TechnicianCode");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Manufacturer");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Address");

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "WeightSet",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "TechnicianCode",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "PieceOfEquipment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "Manufacturer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "EquipmentTemplate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "Contact",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "Address",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

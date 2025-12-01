using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class DeleteTechnician : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "TechnicianCode",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "EquipmentTemplate",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "Manufacturer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "Customer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
               name: "Delete",
               table: "Address",
               nullable: false,
               defaultValue: false);

            migrationBuilder.AddColumn<bool>(
               name: "Delete",
               table: "Contact",
               nullable: false,
               defaultValue: false);

            migrationBuilder.AddColumn<bool>(
              name: "Delete",
              table: "WeightSet",
              nullable: false,
              defaultValue: false);

            migrationBuilder.AddColumn<bool>(
             name: "Delete",
             table: "PieceOfEquipment",
             nullable: false,
             defaultValue: false);
        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delete",
                table: "TechnicianCode");

            migrationBuilder.DropColumn(
               name: "Delete",
               table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                           name: "Delete",
                           table: "Manufacturer");

            migrationBuilder.DropColumn(
               name: "Delete",
               table: "Customer");

            migrationBuilder.DropColumn(
               name: "Delete",
               table: "Address");

            migrationBuilder.DropColumn(
               name: "Delete",
               table: "Contact");
            
            migrationBuilder.DropColumn(
               name: "Delete",
               table: "WeightSet");

            migrationBuilder.DropColumn(
               name: "Delete",
               table: "PieceOfEquipment");





        }
    }
}

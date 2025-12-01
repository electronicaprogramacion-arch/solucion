using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Scale",
                table: "PieceOfEquipment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ToleranceValue",
                table: "PieceOfEquipment",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UncertaintyValue",
                table: "PieceOfEquipment",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "HardnessTestBlockInformation",
                table: "EquipmentType",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scale",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "ToleranceValue",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "UncertaintyValue",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "HardnessTestBlockInformation",
                table: "EquipmentType");
        }
    }
}

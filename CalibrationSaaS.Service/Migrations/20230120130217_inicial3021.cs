using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Hardness",
                table: "PieceOfEquipment",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MicronValue",
                table: "PieceOfEquipment",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TypeMicro",
                table: "PieceOfEquipment",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Note",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hardness",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "MicronValue",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "TypeMicro",
                table: "PieceOfEquipment");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Note",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}

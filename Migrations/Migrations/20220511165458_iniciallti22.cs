using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class iniciallti22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControlNumber",
                table: "Uncertainty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ControlNumber",
                table: "Uncertainty",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

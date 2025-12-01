using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial406 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "Component",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disabled",
                table: "Component");
        }
    }
}

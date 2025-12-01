using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial55 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDescendant",
                table: "TestPoint",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDescendant",
                table: "TestPoint");
        }
    }
}

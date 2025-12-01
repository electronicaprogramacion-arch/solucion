using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial400 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "User_Rol",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Permissions",
                table: "Rol",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "User_Rol");

            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "Rol");
        }
    }
}

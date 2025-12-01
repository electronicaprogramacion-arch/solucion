using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class iniciallti11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InToleranceFound",
                table: "ForceResult");

            migrationBuilder.DropColumn(
                name: "InToleranceLeft",
                table: "ForceResult");

            migrationBuilder.AlterColumn<string>(
                name: "Class",
                table: "ForceResult",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Class",
                table: "ForceResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InToleranceFound",
                table: "ForceResult",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InToleranceLeft",
                table: "ForceResult",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);
        }
    }
}

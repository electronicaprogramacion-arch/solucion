using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class _10003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<double>(
            //    name: "Resolution",
            //    table: "WeightSet",
            //    type: "float",
            //    nullable: false,
            //    defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "RockwellResult",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "ForceResult",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "RockwellResult");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "ForceResult");
        }
    }
}

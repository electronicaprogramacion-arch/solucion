using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    /// <inheritdoc />
    public partial class inicial4100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RuleSet",
                table: "ViewPropertyBase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Formula",
                table: "DynamicProperty",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MethodToCalculate",
                table: "CalibrationSubType",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RuleSet",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "Formula",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "MethodToCalculate",
                table: "CalibrationSubType");
        }
    }
}

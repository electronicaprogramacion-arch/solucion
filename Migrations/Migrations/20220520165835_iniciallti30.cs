using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class iniciallti30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncludeASTM",
                table: "WorkOrderDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUniversal",
                table: "WorkOrderDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeASTM",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "IsUniversal",
                table: "WorkOrderDetail");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial112 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfflineID",
                table: "WorkOrderDetail",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfflineID",
                table: "PieceOfEquipment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfflineID",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "OfflineID",
                table: "PieceOfEquipment");
        }
    }
}

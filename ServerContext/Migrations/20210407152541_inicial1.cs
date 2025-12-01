using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tolerance",
                table: "WorkOrderDetail");

            migrationBuilder.CreateTable(
                name: "POE_POE",
                columns: table => new
                {
                    PieceOfEquipmentID = table.Column<int>(nullable: false),
                    PieceOfEquipmentID2 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POE_POE", x => new { x.PieceOfEquipmentID, x.PieceOfEquipmentID2 });
                    table.ForeignKey(
                        name: "FK_POE_POE_PieceOfEquipment_PieceOfEquipmentID2",
                        column: x => x.PieceOfEquipmentID2,
                        principalTable: "PieceOfEquipment",
                        principalColumn: "PieceOfEquipmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_POE_POE_PieceOfEquipmentID2",
                table: "POE_POE",
                column: "PieceOfEquipmentID2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POE_POE");

            migrationBuilder.AddColumn<double>(
                name: "Tolerance",
                table: "WorkOrderDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

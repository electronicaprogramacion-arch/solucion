using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3125 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalibrationSubType_DynamicProperty",
                columns: table => new
                {
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    DynamicPropertyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationSubType_DynamicProperty", x => new { x.CalibrationSubTypeId, x.DynamicPropertyID });
                    table.ForeignKey(
                        name: "FK_CalibrationSubType_DynamicProperty_CalibrationSubType_DynamicPropertyID",
                        column: x => x.DynamicPropertyID,
                        principalTable: "CalibrationSubType",
                        principalColumn: "CalibrationSubTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_DynamicProperty_DynamicPropertyID",
                table: "CalibrationSubType_DynamicProperty",
                column: "DynamicPropertyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalibrationSubType_DynamicProperty");
        }
    }
}

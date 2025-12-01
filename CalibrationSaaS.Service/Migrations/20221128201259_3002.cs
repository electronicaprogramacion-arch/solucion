using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class _3002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalCondition",
                columns: table => new
                {
                    ExternalConditionConditionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Humidity = table.Column<double>(type: "float", nullable: false),
                    HRC = table.Column<double>(type: "float", nullable: false),
                    HRA = table.Column<double>(type: "float", nullable: false),
                    HRN = table.Column<double>(type: "float", nullable: false),
                    Pass1 = table.Column<bool>(type: "bit", nullable: false),
                    Pass2 = table.Column<bool>(type: "bit", nullable: false),
                    Pass3 = table.Column<bool>(type: "bit", nullable: false),
                    Onesixteenth = table.Column<double>(type: "float", nullable: false),
                    AnEighth = table.Column<double>(type: "float", nullable: false),
                    Quarter = table.Column<double>(type: "float", nullable: false),
                    Medium = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalCondition", x => x.ExternalConditionConditionId);
                    table.ForeignKey(
                        name: "FK_ExternalCondition_WorkOrderDetail_WorkOrderDetailId",
                        column: x => x.WorkOrderDetailId,
                        principalTable: "WorkOrderDetail",
                        principalColumn: "WorkOrderDetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCondition_WorkOrderDetailId",
                table: "ExternalCondition",
                column: "WorkOrderDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalCondition");
        }
    }
}

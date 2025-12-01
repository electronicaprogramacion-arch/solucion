using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial2000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemperatureStandardId",
                table: "WorkOrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TestCodeID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: true,
                defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "Description",
            //    table: "CalibrationResultContributor",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "TestCode",
                columns: table => new
                {
                    TestCodeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangeMIn = table.Column<double>(type: "float", nullable: false),
                    RangeMax = table.Column<double>(type: "float", nullable: false),
                    CalibrationTypeID = table.Column<int>(type: "int", nullable: true),
                    CalibrationSubtTypeID = table.Column<int>(type: "int", nullable: true),
                    Accredited = table.Column<bool>(type: "bit", nullable: false),
                    UnitOfMeasureID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCode", x => x.TestCodeID);
                    table.ForeignKey(
                        name: "FK_TestCode_UnitOfMeasure_UnitOfMeasureID",
                        column: x => x.UnitOfMeasureID,
                        principalTable: "UnitOfMeasure",
                        principalColumn: "UnitOfMeasureID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderDetail_TestCodeID",
                table: "WorkOrderDetail",
                column: "TestCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_TestCode_UnitOfMeasureID",
                table: "TestCode",
                column: "UnitOfMeasureID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrderDetail_TestCode_TestCodeID",
                table: "WorkOrderDetail",
                column: "TestCodeID",
                principalTable: "TestCode",
                principalColumn: "TestCodeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrderDetail_TestCode_TestCodeID",
                table: "WorkOrderDetail");

            migrationBuilder.DropTable(
                name: "TestCode");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrderDetail_TestCodeID",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "TemperatureStandardId",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "TestCodeID",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CalibrationResultContributor");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class iniciallti800 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasWorkOrderDetail",
                table: "EquipmentType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CalibrationResultContributor",
                columns: table => new
                {
                    CalibrationResultContributorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EquipmentTemplateID = table.Column<int>(type: "int", nullable: true),
                    PieceOfEquipmentID = table.Column<string>(type: "varchar(500)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UnitOfMeasureID = table.Column<int>(type: "int", nullable: false),
                    Divisor = table.Column<double>(type: "float", nullable: false),
                    SensitiveCoefficient = table.Column<double>(type: "float", nullable: false),
                    ConfidenceLevel = table.Column<double>(type: "float", nullable: false),
                    Quotient = table.Column<double>(type: "float", nullable: false),
                    Square = table.Column<double>(type: "float", nullable: false),
                    Distribution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeContributor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Magnitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationResultContributor", x => x.CalibrationResultContributorID);
                    table.ForeignKey(
                        name: "FK_CalibrationResultContributor_EquipmentTemplate_EquipmentTemplateID",
                        column: x => x.EquipmentTemplateID,
                        principalTable: "EquipmentTemplate",
                        principalColumn: "EquipmentTemplateID");
                    table.ForeignKey(
                        name: "FK_CalibrationResultContributor_PieceOfEquipment_PieceOfEquipmentID",
                        column: x => x.PieceOfEquipmentID,
                        principalTable: "PieceOfEquipment",
                        principalColumn: "PieceOfEquipmentID");
                    table.ForeignKey(
                        name: "FK_CalibrationResultContributor_UnitOfMeasure_UnitOfMeasureID",
                        column: x => x.UnitOfMeasureID,
                        principalTable: "UnitOfMeasure",
                        principalColumn: "UnitOfMeasureID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationResultContributor_EquipmentTemplateID",
                table: "CalibrationResultContributor",
                column: "EquipmentTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationResultContributor_PieceOfEquipmentID",
                table: "CalibrationResultContributor",
                column: "PieceOfEquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationResultContributor_UnitOfMeasureID",
                table: "CalibrationResultContributor",
                column: "UnitOfMeasureID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalibrationResultContributor");

            migrationBuilder.DropColumn(
                name: "HasWorkOrderDetail",
                table: "EquipmentType");
        }
    }
}

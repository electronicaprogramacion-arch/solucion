using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class initiallti20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UncertaintyID",
                table: "Force",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Uncertainty",
                columns: table => new
                {
                    UncertaintyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentTemplateID = table.Column<int>(type: "int", nullable: false),
                    PieceOfEquipmentID = table.Column<string>(type: "varchar(500)", nullable: true),
                    ControlNumber = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RangeMin = table.Column<double>(type: "float", nullable: false),
                    RangeMax = table.Column<double>(type: "float", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitOfMeasureID = table.Column<int>(type: "int", nullable: false),
                    Divisor = table.Column<double>(type: "float", nullable: false),
                    SensitiveCoefficient = table.Column<double>(type: "float", nullable: false),
                    ConfidenceLevel = table.Column<double>(type: "float", nullable: false),
                    Quotient = table.Column<double>(type: "float", nullable: false),
                    Square = table.Column<double>(type: "float", nullable: false),
                    Distribution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uncertainty", x => x.UncertaintyID);
                    table.ForeignKey(
                        name: "FK_Uncertainty_EquipmentTemplate_EquipmentTemplateID",
                        column: x => x.EquipmentTemplateID,
                        principalTable: "EquipmentTemplate",
                        principalColumn: "EquipmentTemplateID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Uncertainty_PieceOfEquipment_PieceOfEquipmentID",
                        column: x => x.PieceOfEquipmentID,
                        principalTable: "PieceOfEquipment",
                        principalColumn: "PieceOfEquipmentID");
                    table.ForeignKey(
                        name: "FK_Uncertainty_UnitOfMeasure_UnitOfMeasureID",
                        column: x => x.UnitOfMeasureID,
                        principalTable: "UnitOfMeasure",
                        principalColumn: "UnitOfMeasureID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Force_UncertaintyID",
                table: "Force",
                column: "UncertaintyID");

            migrationBuilder.CreateIndex(
                name: "IX_Uncertainty_EquipmentTemplateID",
                table: "Uncertainty",
                column: "EquipmentTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_Uncertainty_PieceOfEquipmentID",
                table: "Uncertainty",
                column: "PieceOfEquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Uncertainty_UnitOfMeasureID",
                table: "Uncertainty",
                column: "UnitOfMeasureID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Force_Uncertainty_UncertaintyID",
            //    table: "Force",
            //    column: "UncertaintyID",
            //    principalTable: "Uncertainty",
            //    principalColumn: "UncertaintyID",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Force_Uncertainty_UncertaintyID",
                table: "Force");

            migrationBuilder.DropTable(
                name: "Uncertainty");

            migrationBuilder.DropIndex(
                name: "IX_Force_UncertaintyID",
                table: "Force");

            migrationBuilder.DropColumn(
                name: "UncertaintyID",
                table: "Force");
        }
    }
}

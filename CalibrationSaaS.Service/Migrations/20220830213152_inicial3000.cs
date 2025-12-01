using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcedureID",
                table: "TestCode",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Procedure",
                columns: table => new
                {
                    ProcedureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedure", x => x.ProcedureID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCode_ProcedureID",
                table: "TestCode",
                column: "ProcedureID");

            migrationBuilder.AddForeignKey(
                name: "FK_TestCode_Procedure_ProcedureID",
                table: "TestCode",
                column: "ProcedureID",
                principalTable: "Procedure",
                principalColumn: "ProcedureID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestCode_Procedure_ProcedureID",
                table: "TestCode");

            migrationBuilder.DropTable(
                name: "Procedure");

            migrationBuilder.DropIndex(
                name: "IX_TestCode_ProcedureID",
                table: "TestCode");

            migrationBuilder.DropColumn(
                name: "ProcedureID",
                table: "TestCode");
        }
    }
}

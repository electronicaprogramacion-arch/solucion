using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3015 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EquipmnetTypeId",
                table: "Note",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TestCodeID",
                table: "Note",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_TestCodeID",
                table: "Note",
                column: "TestCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_TestCode_TestCodeID",
                table: "Note",
                column: "TestCodeID",
                principalTable: "TestCode",
                principalColumn: "TestCodeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_TestCode_TestCodeID",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_TestCodeID",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "TestCodeID",
                table: "Note");

            migrationBuilder.AlterColumn<int>(
                name: "EquipmnetTypeId",
                table: "Note",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

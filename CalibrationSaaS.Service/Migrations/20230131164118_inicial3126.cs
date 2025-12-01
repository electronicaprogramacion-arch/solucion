using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3126 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.DropIndex(
                name: "IX_DynamicProperty_ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DynamicProperty",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                table: "DynamicProperty",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "DynamicProperty",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CalibrationSubType_ViewProperty",
                columns: table => new
                {
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    ViewPropertyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationSubType_ViewProperty", x => new { x.CalibrationSubTypeId, x.ViewPropertyID });
                    table.ForeignKey(
                        name: "FK_CalibrationSubType_ViewProperty_CalibrationSubType_ViewPropertyID",
                        column: x => x.ViewPropertyID,
                        principalTable: "CalibrationSubType",
                        principalColumn: "CalibrationSubTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_ViewProperty_ViewPropertyID",
                table: "CalibrationSubType_ViewProperty",
                column: "ViewPropertyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalibrationSubType_ViewProperty");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DynamicProperty",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                table: "DynamicProperty",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "DynamicProperty",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicProperty_ViewPropertyBaseID",
                table: "DynamicProperty",
                column: "ViewPropertyBaseID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseID",
                table: "DynamicProperty",
                column: "ViewPropertyBaseID",
                principalTable: "ViewPropertyBase",
                principalColumn: "ViewPropertyID");
        }
    }
}

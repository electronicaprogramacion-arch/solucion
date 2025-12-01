using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    /// <inheritdoc />
    public partial class inicial5000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "MethodToCalculate",
            //    table: "CalibrationSubType");

            //migrationBuilder.RenameColumn(
            //    name: "Row",
            //    table: "DynamicProperty",
            //    newName: "ColPosition");

            //migrationBuilder.AddColumn<string>(
            //    name: "ExtendedObject",
            //    table: "GenericCalibrationResult",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "DefaultValueFormula",
            //    table: "DynamicProperty",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "Enable",
            //    table: "DynamicProperty",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "ValidationFormula",
            //    table: "DynamicProperty",
            //    type: "varchar(max)",
            //    unicode: false,
            //    maxLength: 20000,
            //    nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CalibrationSubTypeViewID",
                table: "CalibrationSubType",
                type: "int",
                nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CreateClass",
            //    table: "CalibrationSubType",
            //    type: "varchar(500)",
            //    unicode: false,
            //    maxLength: 500,
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "GetClass",
            //    table: "CalibrationSubType",
            //    type: "varchar(500)",
            //    unicode: false,
            //    maxLength: 500,
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "CalibrationSubTypeView",
                columns: table => new
                {
                    CalibrationSubTypeViewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    EnabledDrag = table.Column<bool>(type: "bit", nullable: false),
                    EnabledDelete = table.Column<bool>(type: "bit", nullable: false),
                    EnabledDuplicate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationSubTypeView", x => x.CalibrationSubTypeViewID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_CalibrationSubTypeViewID",
                table: "CalibrationSubType",
                column: "CalibrationSubTypeViewID",
                unique: true,
                filter: "[CalibrationSubTypeViewID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_CalibrationSubTypeView_CalibrationSubTypeViewID",
                table: "CalibrationSubType",
                column: "CalibrationSubTypeViewID",
                principalTable: "CalibrationSubTypeView",
                principalColumn: "CalibrationSubTypeViewID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_CalibrationSubTypeView_CalibrationSubTypeViewID",
                table: "CalibrationSubType");

            migrationBuilder.DropTable(
                name: "CalibrationSubTypeView");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_CalibrationSubTypeViewID",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "ExtendedObject",
                table: "GenericCalibrationResult");

            migrationBuilder.DropColumn(
                name: "DefaultValueFormula",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "Enable",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "ValidationFormula",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "CalibrationSubTypeViewID",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "CreateClass",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "GetClass",
                table: "CalibrationSubType");

            migrationBuilder.RenameColumn(
                name: "ColPosition",
                table: "DynamicProperty",
                newName: "Row");

            migrationBuilder.AddColumn<string>(
                name: "MethodToCalculate",
                table: "CalibrationSubType",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

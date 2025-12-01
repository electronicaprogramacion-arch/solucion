using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    /// <inheritdoc />
    public partial class inicial60002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<double>(
            //    name: "LoadKGF",
            //    table: "PieceOfEquipment",
            //    type: "float",
            //    nullable: false,
            //    defaultValue: 0.0);

            //migrationBuilder.AddColumn<double>(
            //    name: "ToleranceHV",
            //    table: "PieceOfEquipment",
            //    type: "float",
            //    nullable: false,
            //    defaultValue: 0.0);

            //migrationBuilder.AddColumn<int>(
            //    name: "CalibrationSubtypeId",
            //    table: "Note",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "CSSForm",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CSSRowHeader",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "ShowHeader",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "Enabled",
            //    table: "CalibrationSubType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<int>(
            //    name: "Position",
            //    table: "CalibrationSubType",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "SelectStandarClass",
            //    table: "CalibrationSubType",
            //    type: "nvarchar(max)",
            //    nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoadKGF",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "ToleranceHV",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "CalibrationSubtypeId",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "CSSForm",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "CSSRowHeader",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "ShowHeader",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "SelectStandarClass",
                table: "CalibrationSubType");
        }
    }
}

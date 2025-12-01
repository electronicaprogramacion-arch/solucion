using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    /// <inheritdoc />
    public partial class inicial6001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "Min",
            //    table: "ViewPropertyBase",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Max",
            //    table: "ViewPropertyBase",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<bool>(
            //    name: "IsVisible",
            //    table: "ViewPropertyBase",
            //    type: "bit",
            //    nullable: true,
            //    oldClrType: typeof(bool),
            //    oldType: "bit");

            //migrationBuilder.AlterColumn<bool>(
            //    name: "IsValid",
            //    table: "ViewPropertyBase",
            //    type: "bit",
            //    nullable: true,
            //    oldClrType: typeof(bool),
            //    oldType: "bit");

            //migrationBuilder.AlterColumn<bool>(
            //    name: "IsDisabled",
            //    table: "ViewPropertyBase",
            //    type: "bit",
            //    nullable: true,
            //    oldClrType: typeof(bool),
            //    oldType: "bit");

            //migrationBuilder.AlterColumn<int>(
            //    name: "DecimalRoundType",
            //    table: "ViewPropertyBase",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AlterColumn<int>(
            //    name: "DecimalNumbers",
            //    table: "ViewPropertyBase",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AddColumn<bool>(
            //    name: "ChangeBackground",
            //    table: "ViewPropertyBase",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "HasHeader",
            //    table: "ViewPropertyBase",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "SelectOptions",
            //    table: "ViewPropertyBase",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Key",
            //    table: "ToleranceType_EquipmentType",
            //    type: "varchar(100)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "ToleranceType",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "ToleranceType",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            //migrationBuilder.AlterColumn<double>(
            //    name: "Resolution",
            //    table: "GenericCalibrationResult",
            //    type: "float",
            //    nullable: true,
            //    oldClrType: typeof(double),
            //    oldType: "float");

            //migrationBuilder.AlterColumn<int>(
            //    name: "DecimalNumber",
            //    table: "GenericCalibrationResult",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AddColumn<bool>(
            //    name: "HasStandardConfiguration",
            //    table: "EquipmentType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "HasWorkOrdeDetailStandard",
            //    table: "EquipmentType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "UseWorkOrderDetailStandard",
            //    table: "EquipmentType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "Map",
            //    table: "DynamicProperty",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "ActionBeginRow",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "AlingActionCSS",
            //    table: "CalibrationSubTypeView",
            //    type: "varchar(500)",
            //    unicode: false,
            //    maxLength: 500,
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "BlockIfInvalid",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CSSGrid",
            //    table: "CalibrationSubTypeView",
            //    type: "varchar(500)",
            //    unicode: false,
            //    maxLength: 500,
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "CSSRow",
            //    table: "CalibrationSubTypeView",
            //    type: "varchar(500)",
            //    unicode: false,
            //    maxLength: 500,
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CSSRowSeparator",
            //    table: "CalibrationSubTypeView",
            //    type: "varchar(500)",
            //    unicode: false,
            //    maxLength: 500,
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ColActionCSS",
            //    table: "CalibrationSubTypeView",
            //    type: "varchar(500)",
            //    unicode: false,
            //    maxLength: 500,
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ColButtonActionCSS",
            //    table: "CalibrationSubTypeView",
            //    type: "varchar(500)",
            //    unicode: false,
            //    maxLength: 500,
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "EnableButtonBar",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "EnabledHeader",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "EnabledNew",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "EnabledSelect",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "FilterField",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "FilterValue",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Key",
            //    table: "CalibrationSubTypeView",
            //    type: "varchar(500)",
            //    unicode: false,
            //    maxLength: 500,
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "NoDataMessage",
            //    table: "CalibrationSubTypeView",
            //    type: "varchar(500)",
            //    unicode: false,
            //    maxLength: 500,
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "PageSize",
            //    table: "CalibrationSubTypeView",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "SortField",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToleranceType_CalibrationTypeId",
                table: "ToleranceType",
                column: "CalibrationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToleranceType_CalibrationType_CalibrationTypeId",
                table: "ToleranceType",
                column: "CalibrationTypeId",
                principalTable: "CalibrationType",
                principalColumn: "CalibrationTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToleranceType_CalibrationType_CalibrationTypeId",
                table: "ToleranceType");

            migrationBuilder.DropIndex(
                name: "IX_ToleranceType_CalibrationTypeId",
                table: "ToleranceType");

            migrationBuilder.DropColumn(
                name: "ChangeBackground",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "HasHeader",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "SelectOptions",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "HasStandardConfiguration",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "HasWorkOrdeDetailStandard",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "UseWorkOrderDetailStandard",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "Map",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "ActionBeginRow",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "AlingActionCSS",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "BlockIfInvalid",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "CSSGrid",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "CSSRow",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "CSSRowSeparator",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "ColActionCSS",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "ColButtonActionCSS",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "EnableButtonBar",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "EnabledHeader",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "EnabledNew",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "EnabledSelect",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "FilterField",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "FilterValue",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "NoDataMessage",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "PageSize",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "SortField",
                table: "CalibrationSubTypeView");

            migrationBuilder.AlterColumn<int>(
                name: "Min",
                table: "ViewPropertyBase",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Max",
                table: "ViewPropertyBase",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVisible",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsValid",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDisabled",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DecimalRoundType",
                table: "ViewPropertyBase",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DecimalNumbers",
                table: "ViewPropertyBase",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "ToleranceType_EquipmentType",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "ToleranceType",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "ToleranceType",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<double>(
                name: "Resolution",
                table: "GenericCalibrationResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DecimalNumber",
                table: "GenericCalibrationResult",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

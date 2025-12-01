using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    /// <inheritdoc />
    public partial class inicial3027 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ToolTipMessage",
                table: "ViewPropertyBase",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMesage",
                table: "ViewPropertyBase",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Display",
                table: "ViewPropertyBase",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ControlType",
                table: "ViewPropertyBase",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "ViewPropertyBase",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CSSClass",
                table: "ViewPropertyBase",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DecimalNumbers",
                table: "ViewPropertyBase",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DecimalRoundType",
                table: "ViewPropertyBase",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EnableToastMessage",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ID",
                table: "ViewPropertyBase",
                type: "varchar(200)",
                unicode: false,
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelCSS",
                table: "ViewPropertyBase",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Max",
                table: "ViewPropertyBase",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Min",
                table: "ViewPropertyBase",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "OnChange",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowControl",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLabel",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StepResol",
                table: "ViewPropertyBase",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToastMessage",
                table: "ViewPropertyBase",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecimalNumbers",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "DecimalRoundType",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "EnableToastMessage",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "LabelCSS",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "Max",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "Min",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "OnChange",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "ShowControl",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "ShowLabel",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "StepResol",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "ToastMessage",
                table: "ViewPropertyBase");

            migrationBuilder.AlterColumn<string>(
                name: "ToolTipMessage",
                table: "ViewPropertyBase",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMesage",
                table: "ViewPropertyBase",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Display",
                table: "ViewPropertyBase",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ControlType",
                table: "ViewPropertyBase",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "ViewPropertyBase",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CSSClass",
                table: "ViewPropertyBase",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}

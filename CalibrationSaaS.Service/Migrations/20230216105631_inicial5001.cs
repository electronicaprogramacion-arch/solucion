using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    /// <inheritdoc />
    public partial class inicial5001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseViewPropertyID",
                table: "DynamicProperty");

            migrationBuilder.DropIndex(
                name: "IX_DynamicProperty_ViewPropertyBaseViewPropertyID",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "ViewPropertyBaseViewPropertyID",
                table: "DynamicProperty");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowLabel",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowControl",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.DropIndex(
                name: "IX_DynamicProperty_ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowLabel",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowControl",
                table: "ViewPropertyBase",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ViewPropertyBaseViewPropertyID",
                table: "DynamicProperty",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicProperty_ViewPropertyBaseViewPropertyID",
                table: "DynamicProperty",
                column: "ViewPropertyBaseViewPropertyID");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseViewPropertyID",
                table: "DynamicProperty",
                column: "ViewPropertyBaseViewPropertyID",
                principalTable: "ViewPropertyBase",
                principalColumn: "ViewPropertyID");
        }
    }
}

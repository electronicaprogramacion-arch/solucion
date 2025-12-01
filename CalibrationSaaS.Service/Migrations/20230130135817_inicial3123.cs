using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_DynamicProperty_ViewPropertyBase_DynamicPropertyID",
            //    table: "DynamicProperty");

            //migrationBuilder.AlterColumn<int>(
            //    name: "DynamicPropertyID",
            //    table: "DynamicProperty",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ViewPropertyBaseID",
                table: "DynamicProperty",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.DropIndex(
                name: "IX_DynamicProperty_ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.AlterColumn<int>(
                name: "DynamicPropertyID",
                table: "DynamicProperty",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_DynamicPropertyID",
                table: "DynamicProperty",
                column: "DynamicPropertyID",
                principalTable: "ViewPropertyBase",
                principalColumn: "ViewPropertyID");
        }
    }
}

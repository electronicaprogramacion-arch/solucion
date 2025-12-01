using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3127 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}

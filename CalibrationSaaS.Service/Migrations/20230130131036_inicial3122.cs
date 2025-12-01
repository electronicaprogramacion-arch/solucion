using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_DynamicProperty_ViewPropertyBase_DynamicPropertyID",
            //    table: "DynamicProperty");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_DynamicProperty_ViewPropertyBase_DynamicPropertyID",
            //    table: "DynamicProperty",
            //    column: "DynamicPropertyID",
            //    principalTable: "ViewPropertyBase",
            //    principalColumn: "ViewPropertyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_DynamicPropertyID",
                table: "DynamicProperty");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_DynamicPropertyID",
                table: "DynamicProperty",
                column: "DynamicPropertyID",
                principalTable: "ViewPropertyBase",
                principalColumn: "ViewPropertyID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

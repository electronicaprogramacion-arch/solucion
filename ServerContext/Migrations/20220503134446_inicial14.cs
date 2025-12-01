using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "DeviceClass",
            //    table: "EquipmentTemplate",
            //    type: "varchar(100)",
            //    unicode: false,
            //    maxLength: 100,
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "PlatformSize",
            //    table: "EquipmentTemplate",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomID",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Certificate_WorkOrderDetailId",
            //    table: "Certificate",
            //    column: "WorkOrderDetailId",
            //    unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_WorkOrderDetail_WorkOrderDetailId",
                table: "Certificate",
                column: "WorkOrderDetailId",
                principalTable: "WorkOrderDetail",
                principalColumn: "WorkOrderDetailID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_WorkOrderDetail_WorkOrderDetailId",
                table: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_WorkOrderDetailId",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "DeviceClass",
                table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                name: "PlatformSize",
                table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                name: "CustomID",
                table: "Customer");
        }
    }
}

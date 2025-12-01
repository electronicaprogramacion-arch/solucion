using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkOrderDetailID",
                table: "RangeTolerance",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RangeTolerance_WorkOrderDetailID",
                table: "RangeTolerance",
                column: "WorkOrderDetailID");

            migrationBuilder.AddForeignKey(
                name: "FK_RangeTolerance_WorkOrderDetail_WorkOrderDetailID",
                table: "RangeTolerance",
                column: "WorkOrderDetailID",
                principalTable: "WorkOrderDetail",
                principalColumn: "WorkOrderDetailID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RangeTolerance_WorkOrderDetail_WorkOrderDetailID",
                table: "RangeTolerance");

            migrationBuilder.DropIndex(
                name: "IX_RangeTolerance_WorkOrderDetailID",
                table: "RangeTolerance");

            migrationBuilder.DropColumn(
                name: "WorkOrderDetailID",
                table: "RangeTolerance");
        }
    }
}

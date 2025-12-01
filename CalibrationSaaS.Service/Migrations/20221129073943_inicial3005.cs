using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_WorkOrderDetail_ExternalCondition_WorkOrderDetailID",
            //    table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "ExternalConditionId",
                table: "WorkOrderDetail");

            //migrationBuilder.AlterColumn<int>(
            //    name: "WorkOrderDetailID",
            //    table: "WorkOrderDetail",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCondition_WorkOrderDetailId",
                table: "ExternalCondition",
                column: "WorkOrderDetailId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ExternalCondition_WorkOrderDetail_WorkOrderDetailId",
            //    table: "ExternalCondition",
            //    column: "WorkOrderDetailId",
            //    principalTable: "WorkOrderDetail",
            //    principalColumn: "WorkOrderDetailID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCondition_WorkOrderDetail_WorkOrderDetailId",
                table: "ExternalCondition");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCondition_WorkOrderDetailId",
                table: "ExternalCondition");

            migrationBuilder.AlterColumn<int>(
                name: "WorkOrderDetailID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ExternalConditionId",
                table: "WorkOrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrderDetail_ExternalCondition_WorkOrderDetailID",
                table: "WorkOrderDetail",
                column: "WorkOrderDetailID",
                principalTable: "ExternalCondition",
                principalColumn: "ExternalConditionId");
        }
    }
}

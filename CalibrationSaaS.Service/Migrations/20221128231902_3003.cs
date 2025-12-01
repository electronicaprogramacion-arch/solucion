using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class _3003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExternalCondition_WorkOrderDetailId",
                table: "ExternalCondition");

            migrationBuilder.RenameColumn(
                name: "ExternalConditionConditionId",
                table: "ExternalCondition",
                newName: "ExternalConditionId");

            migrationBuilder.AddColumn<int>(
                name: "ExternalConditionId",
                table: "WorkOrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCondition_WorkOrderDetailId",
                table: "ExternalCondition",
                column: "WorkOrderDetailId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExternalCondition_WorkOrderDetailId",
                table: "ExternalCondition");

            migrationBuilder.DropColumn(
                name: "ExternalConditionId",
                table: "WorkOrderDetail");

            migrationBuilder.RenameColumn(
                name: "ExternalConditionId",
                table: "ExternalCondition",
                newName: "ExternalConditionConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCondition_WorkOrderDetailId",
                table: "ExternalCondition",
                column: "WorkOrderDetailId");
        }
    }
}

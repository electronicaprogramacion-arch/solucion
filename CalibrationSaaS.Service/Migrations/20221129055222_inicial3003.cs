using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCondition_WorkOrderDetail_WorkOrderDetailId",
                table: "ExternalCondition");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCondition_WorkOrderDetailId",
                table: "ExternalCondition");

            //migrationBuilder.AlterColumn<int>(
            //    name: "WorkOrderDetailID",
            //    table: "WorkOrderDetail",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsAsFound",
                table: "ExternalCondition",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PassAnEighth",
                table: "ExternalCondition",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PassHRA",
                table: "ExternalCondition",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PassHRN",
                table: "ExternalCondition",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PassQuarter",
                table: "ExternalCondition",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Serial",
                table: "ExternalCondition",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_WorkOrderDetail_ExternalCondition_WorkOrderDetailID",
            //    table: "WorkOrderDetail",
            //    column: "WorkOrderDetailID",
            //    principalTable: "ExternalCondition",
            //    principalColumn: "ExternalConditionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrderDetail_ExternalCondition_WorkOrderDetailID",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "IsAsFound",
                table: "ExternalCondition");

            migrationBuilder.DropColumn(
                name: "PassAnEighth",
                table: "ExternalCondition");

            migrationBuilder.DropColumn(
                name: "PassHRA",
                table: "ExternalCondition");

            migrationBuilder.DropColumn(
                name: "PassHRN",
                table: "ExternalCondition");

            migrationBuilder.DropColumn(
                name: "PassQuarter",
                table: "ExternalCondition");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "ExternalCondition");

            migrationBuilder.AlterColumn<int>(
                name: "WorkOrderDetailID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCondition_WorkOrderDetailId",
                table: "ExternalCondition",
                column: "WorkOrderDetailId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCondition_WorkOrderDetail_WorkOrderDetailId",
                table: "ExternalCondition",
                column: "WorkOrderDetailId",
                principalTable: "WorkOrderDetail",
                principalColumn: "WorkOrderDetailID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

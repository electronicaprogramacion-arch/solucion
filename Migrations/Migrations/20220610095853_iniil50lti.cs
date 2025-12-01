using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class iniil50lti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CertificationID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderDetail_CertificationID",
                table: "WorkOrderDetail",
                column: "CertificationID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_WorkOrderDetail_Certification_CertificationID",
            //    table: "WorkOrderDetail",
            //    column: "CertificationID",
            //    principalTable: "Certification",
            //    principalColumn: "CertificationID",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrderDetail_Certification_CertificationID",
                table: "WorkOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrderDetail_CertificationID",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "CertificationID",
                table: "WorkOrderDetail");
        }
    }
}

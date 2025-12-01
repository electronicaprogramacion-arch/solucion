using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class iniil51lti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_WorkOrderDetail_Certification_CertificationID",
            //    table: "WorkOrderDetail");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CertificationID",
            //    table: "WorkOrderDetail",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrderDetail_Certification_CertificationID",
                table: "WorkOrderDetail",
                column: "CertificationID",
                principalTable: "Certification",
                principalColumn: "CertificationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrderDetail_Certification_CertificationID",
                table: "WorkOrderDetail");

            migrationBuilder.AlterColumn<int>(
                name: "CertificationID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrderDetail_Certification_CertificationID",
                table: "WorkOrderDetail",
                column: "CertificationID",
                principalTable: "Certification",
                principalColumn: "CertificationID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

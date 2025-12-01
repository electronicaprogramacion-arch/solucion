using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial222 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "WorkOrderDetailByStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "WorkOrderDetailByStatus",
            //    columns: table => new
            //    {
            //        WorkOrderDetailID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        EquipmentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        EquipmentTypeID = table.Column<int>(type: "int", nullable: false),
            //        Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        StatusDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        StatusId = table.Column<int>(type: "int", nullable: false),
            //        WorkOrderId = table.Column<int>(type: "int", nullable: false),
            //        WorkOrderReceiveDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_WorkOrderDetailByStatus", x => x.WorkOrderDetailID);
            //    });
        }
    }
}

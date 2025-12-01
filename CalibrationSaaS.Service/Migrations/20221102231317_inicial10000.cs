using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial10000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Force_Uncertainty_UncertaintyID",
            //    table: "Force");

            //migrationBuilder.DropIndex(
            //    name: "IX_Force_UncertaintyID",
            //    table: "Force");

            //migrationBuilder.AddColumn<bool>(
            //    name: "EndOfMonth",
            //    table: "WorkOrderDetail",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "CalibrationTypeID",
            //    table: "UnitOfMeasureType",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Code",
            //    table: "TechnicianCode",
            //    type: "nvarchar(20)",
            //    maxLength: 20,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Contact",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CellPhoneNumber",
                table: "Contact",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ToleranceType",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalibrationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToleranceType", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "ToleranceType_EquipmentType",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EquipmentTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToleranceType_EquipmentType", x => new { x.Key, x.EquipmentTypeID });
                    table.ForeignKey(
                        name: "FK_ToleranceType_EquipmentType_EquipmentType_EquipmentTypeID",
                        column: x => x.EquipmentTypeID,
                        principalTable: "EquipmentType",
                        principalColumn: "EquipmentTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToleranceType_EquipmentType_ToleranceType_Key",
                        column: x => x.Key,
                        principalTable: "ToleranceType",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToleranceType_EquipmentType_EquipmentTypeID",
                table: "ToleranceType_EquipmentType",
                column: "EquipmentTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToleranceType_EquipmentType");

            migrationBuilder.DropTable(
                name: "ToleranceType");

            migrationBuilder.DropColumn(
                name: "EndOfMonth",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "CalibrationTypeID",
                table: "UnitOfMeasureType");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "TechnicianCode",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CellPhoneNumber",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Force_UncertaintyID",
                table: "Force",
                column: "UncertaintyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Force_Uncertainty_UncertaintyID",
                table: "Force",
                column: "UncertaintyID",
                principalTable: "Uncertainty",
                principalColumn: "UncertaintyID");
        }
    }
}

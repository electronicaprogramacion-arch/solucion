using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerContext.Migrations
{
    public partial class inicial4000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentTemplate_EquipmentType_EquipmentType1ID",
                table: "EquipmentTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_PieceOfEquipment_EquipmentType_EquipmentTypeID",
                table: "PieceOfEquipment");

            migrationBuilder.DropIndex(
                name: "IX_PieceOfEquipment_EquipmentTypeID",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "EquipmentTypeID",
                table: "PieceOfEquipment");

            migrationBuilder.AlterColumn<int>(
                name: "WorkOrderDetailID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentTemplate_EquipmentType_EquipmentTypeID",
                table: "EquipmentTemplate",
                column: "EquipmentTypeID",
                principalTable: "EquipmentType",
                principalColumn: "EquipmentTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentTemplate_EquipmentType_EquipmentTypeID",
                table: "EquipmentTemplate");

            migrationBuilder.AlterColumn<int>(
                name: "WorkOrderDetailID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "EquipmentTypeID",
                table: "PieceOfEquipment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PieceOfEquipment_EquipmentTypeID",
                table: "PieceOfEquipment",
                column: "EquipmentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentTemplate_EquipmentType_EquipmentType1ID",
                table: "EquipmentTemplate",
                column: "EquipmentTypeID",
                principalTable: "EquipmentType",
                principalColumn: "EquipmentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_PieceOfEquipment_EquipmentType_EquipmentTypeID",
                table: "PieceOfEquipment",
                column: "EquipmentTypeID",
                principalTable: "EquipmentType",
                principalColumn: "EquipmentTypeID");
        }
    }
}

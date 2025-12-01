using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rockwell_TestPoint_TestPointID",
                table: "Rockwell");

            migrationBuilder.AddColumn<double>(
                name: "Max",
                table: "RockwellResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Min",
                table: "RockwellResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            //migrationBuilder.AlterColumn<int>(
            //    name: "TestPointID",
            //    table: "Rockwell",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            ////migrationBuilder.AlterColumn<int>(
            //    name: "Commnet",
            //    table: "Note",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.CreateTable(
            //    name: "NoteWOD",
            //    columns: table => new
            //    {
            //        NoteWODId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        WorkOrderDetailId = table.Column<int>(type: "int", nullable: true),
            //        Position = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_NoteWOD", x => x.NoteWODId);
            //        table.ForeignKey(
            //            name: "FK_NoteWOD_WorkOrderDetail_WorkOrderDetailId",
            //            column: x => x.WorkOrderDetailId,
            //            principalTable: "WorkOrderDetail",
            //            principalColumn: "WorkOrderDetailID");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_NoteWOD_WorkOrderDetailId",
            //    table: "NoteWOD",
            //    column: "WorkOrderDetailId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Rockwell_TestPoint_TestPointID",
            //    table: "Rockwell",
            //    column: "TestPointID",
            //    principalTable: "TestPoint",
            //    principalColumn: "TestPointID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rockwell_TestPoint_TestPointID",
                table: "Rockwell");

            migrationBuilder.DropTable(
                name: "NoteWOD");

            migrationBuilder.DropColumn(
                name: "Max",
                table: "RockwellResult");

            migrationBuilder.DropColumn(
                name: "Min",
                table: "RockwellResult");

            migrationBuilder.AlterColumn<int>(
                name: "TestPointID",
                table: "Rockwell",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Commnet",
                table: "Note",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rockwell_TestPoint_TestPointID",
                table: "Rockwell",
                column: "TestPointID",
                principalTable: "TestPoint",
                principalColumn: "TestPointID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

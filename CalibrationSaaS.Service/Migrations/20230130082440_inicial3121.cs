using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    public partial class inicial3121 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "DynamicPropertyID",
            //    table: "DynamicProperty",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "ViewPropertyBase",
                columns: table => new
                {
                    ViewPropertyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsHide = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMesage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Display = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToolTipMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReGenerate = table.Column<bool>(type: "bit", nullable: false),
                    SelectShowDefaultOption = table.Column<bool>(type: "bit", nullable: false),
                    CSSClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DynamicPropertyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewPropertyBase", x => x.ViewPropertyID);
                });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_DynamicProperty_ViewPropertyBase_DynamicPropertyID",
            //    table: "DynamicProperty",
            //    column: "DynamicPropertyID",
            //    principalTable: "ViewPropertyBase",
            //    principalColumn: "ViewPropertyID",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_DynamicPropertyID",
                table: "DynamicProperty");

            migrationBuilder.DropTable(
                name: "ViewPropertyBase");

            migrationBuilder.AlterColumn<int>(
                name: "DynamicPropertyID",
                table: "DynamicProperty",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}

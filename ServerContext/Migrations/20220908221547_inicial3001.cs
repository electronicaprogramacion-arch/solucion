using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerContext.Migrations
{
    public partial class inicial3001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_UnitOfMeasure_UnitOfMeasureType_TypeID",
            //    table: "UnitOfMeasure");

            //migrationBuilder.DropIndex(
            //    name: "IX_User_Email",
            //    table: "User");

            //migrationBuilder.DropIndex(
            //    name: "IX_UnitOfMeasure_TypeID",
            //    table: "UnitOfMeasure");

            //migrationBuilder.DeleteData(
            //    table: "UnitOfMeasure",
            //    keyColumn: "UnitOfMeasureID",
            //    keyValue: 1);

            //migrationBuilder.DeleteData(
            //    table: "UnitOfMeasure",
            //    keyColumn: "UnitOfMeasureID",
            //    keyValue: 2);

            //migrationBuilder.DeleteData(
            //    table: "UnitOfMeasureType",
            //    keyColumn: "Value",
            //    keyValue: 1);

            //migrationBuilder.DeleteData(
            //    table: "UnitOfMeasureType",
            //    keyColumn: "Value",
            //    keyValue: 2);

            //migrationBuilder.DeleteData(
            //    table: "UnitOfMeasureType",
            //    keyColumn: "Value",
            //    keyValue: 3);

            migrationBuilder.DropColumn(
                name: "PassWord",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassWord",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "UnitOfMeasureType",
                columns: new[] { "Value", "Description", "Name" },
                values: new object[] { 1, null, "Temperature" });

            migrationBuilder.InsertData(
                table: "UnitOfMeasureType",
                columns: new[] { "Value", "Description", "Name" },
                values: new object[] { 2, null, "Humidity" });

            migrationBuilder.InsertData(
                table: "UnitOfMeasureType",
                columns: new[] { "Value", "Description", "Name" },
                values: new object[] { 3, null, "Weight" });

            migrationBuilder.InsertData(
                table: "UnitOfMeasure",
                columns: new[] { "UnitOfMeasureID", "Abbreviation", "ConversionValue", "Description", "IsEnabled", "Name", "TypeID", "UncertaintyUnitOfMeasureID", "UnitOfMeasureBaseID" },
                values: new object[] { 1, "Pd", 0.0, null, true, "Pounds", 3, 1, null });

            migrationBuilder.InsertData(
                table: "UnitOfMeasure",
                columns: new[] { "UnitOfMeasureID", "Abbreviation", "ConversionValue", "Description", "IsEnabled", "Name", "TypeID", "UncertaintyUnitOfMeasureID", "UnitOfMeasureBaseID" },
                values: new object[] { 2, "Kg", 0.0, null, true, "Kilogramo", 3, 2, null });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasure_TypeID",
                table: "UnitOfMeasure",
                column: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitOfMeasure_UnitOfMeasureType_TypeID",
                table: "UnitOfMeasure",
                column: "TypeID",
                principalTable: "UnitOfMeasureType",
                principalColumn: "Value",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Migrations
{
    public partial class inicial300 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Contact_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumber_CustomerAggregates_CustomerAggregateAggregateID",
                table: "PhoneNumber");

            migrationBuilder.DropForeignKey(
                name: "FK_Social_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Social");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAggregateAggregateID",
                table: "Social",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAggregateAggregateID",
                table: "PhoneNumber",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAggregateAggregateID",
                table: "Contact",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAggregateAggregateID",
                table: "Address",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Address",
                column: "CustomerAggregateAggregateID",
                principalTable: "CustomerAggregates",
                principalColumn: "AggregateID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Contact",
                column: "CustomerAggregateAggregateID",
                principalTable: "CustomerAggregates",
                principalColumn: "AggregateID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumber_CustomerAggregates_CustomerAggregateAggregateID",
                table: "PhoneNumber",
                column: "CustomerAggregateAggregateID",
                principalTable: "CustomerAggregates",
                principalColumn: "AggregateID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Social_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Social",
                column: "CustomerAggregateAggregateID",
                principalTable: "CustomerAggregates",
                principalColumn: "AggregateID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Contact_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumber_CustomerAggregates_CustomerAggregateAggregateID",
                table: "PhoneNumber");

            migrationBuilder.DropForeignKey(
                name: "FK_Social_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Social");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAggregateAggregateID",
                table: "Social",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAggregateAggregateID",
                table: "PhoneNumber",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAggregateAggregateID",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAggregateAggregateID",
                table: "Address",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Address",
                column: "CustomerAggregateAggregateID",
                principalTable: "CustomerAggregates",
                principalColumn: "AggregateID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Contact",
                column: "CustomerAggregateAggregateID",
                principalTable: "CustomerAggregates",
                principalColumn: "AggregateID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumber_CustomerAggregates_CustomerAggregateAggregateID",
                table: "PhoneNumber",
                column: "CustomerAggregateAggregateID",
                principalTable: "CustomerAggregates",
                principalColumn: "AggregateID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Social_CustomerAggregates_CustomerAggregateAggregateID",
                table: "Social",
                column: "CustomerAggregateAggregateID",
                principalTable: "CustomerAggregates",
                principalColumn: "AggregateID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

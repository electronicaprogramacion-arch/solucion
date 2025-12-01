using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    /// <inheritdoc />
    public partial class inicial8002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CalibrationSubType_Weight_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCali~",
            //    table: "CalibrationSubType_Weight");

            //migrationBuilder.DropIndex(
            //    name: "IX_CalibrationSubType_Weight_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderDet~",
            //    table: "CalibrationSubType_Weight");

            //migrationBuilder.DropColumn(
            //    name: "GenericCalibrationCalibrationSubTypeId",
            //    table: "CalibrationSubType_Weight");

            //migrationBuilder.DropColumn(
            //    name: "GenericCalibrationSequenceID",
            //    table: "CalibrationSubType_Weight");

            //migrationBuilder.DropColumn(
            //    name: "GenericCalibrationWorkOrderDetailId",
            //    table: "CalibrationSubType_Weight");

            //migrationBuilder.AddColumn<string>(
            //    name: "Configuration",
            //    table: "WorkOrderDetail",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Option",
            //    table: "WOD_Weight",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Option",
            //    table: "WOD_Standard",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Option",
            //    table: "WeightSet",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "ViewPropertyID",
            //    table: "ViewPropertyBase",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddColumn<long>(
            //    name: "Updated",
            //    table: "RockwellResult",
            //    type: "bigint",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Configuration",
            //    table: "PieceOfEquipment",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Validation",
            //    table: "Note",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<long>(
            //    name: "Updated",
            //    table: "GenericCalibrationResult2",
            //    type: "bigint",
            //    nullable: true);

            //migrationBuilder.AddColumn<long>(
            //    name: "Updated",
            //    table: "GenericCalibrationResult",
            //    type: "bigint",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "DefaultCustomer",
            //    table: "EquipmentType",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "GridLocation",
            //    table: "DynamicProperty",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "HasNew",
            //    table: "CalibrationType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "EnableSelect2",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "HasDelete",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "HasNewButton",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "NewButtonCSS",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "NewButtonTitle",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Select2Title",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "SelectTitle",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "CreateCopy",
            //    table: "CalibrationSubType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "Select2StandarClass",
            //    table: "CalibrationSubType",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "StandardAssignComponent",
            //    table: "CalibrationSubType",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "StandardAssignComponent2",
            //    table: "CalibrationSubType",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "Lenght",
                columns: table => new
                {
                    LenghtID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<int>(type: "int", nullable: false),
                    To = table.Column<int>(type: "int", nullable: false),
                    UnitOfMeasure = table.Column<int>(type: "int", nullable: false),
                    Tolerance = table.Column<double>(type: "float", nullable: false),
                    CertificationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lenght", x => x.LenghtID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lenght");

            migrationBuilder.DropColumn(
                name: "Configuration",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "Option",
                table: "WOD_Weight");

            migrationBuilder.DropColumn(
                name: "Option",
                table: "WOD_Standard");

            migrationBuilder.DropColumn(
                name: "Option",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "RockwellResult");

            migrationBuilder.DropColumn(
                name: "Configuration",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "Validation",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "GenericCalibrationResult2");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "GenericCalibrationResult");

            migrationBuilder.DropColumn(
                name: "DefaultCustomer",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "GridLocation",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "HasNew",
                table: "CalibrationType");

            migrationBuilder.DropColumn(
                name: "EnableSelect2",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "HasDelete",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "HasNewButton",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "NewButtonCSS",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "NewButtonTitle",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "Select2Title",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "SelectTitle",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "CreateCopy",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "Select2StandarClass",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "StandardAssignComponent",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "StandardAssignComponent2",
                table: "CalibrationSubType");

            migrationBuilder.AlterColumn<int>(
                name: "ViewPropertyID",
                table: "ViewPropertyBase",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationCalibrationSubTypeId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationSequenceID",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Weight_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderDet~",
                table: "CalibrationSubType_Weight",
                columns: new[] { "GenericCalibrationSequenceID", "GenericCalibrationCalibrationSubTypeId", "GenericCalibrationWorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Weight_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCali~",
                table: "CalibrationSubType_Weight",
                columns: new[] { "GenericCalibrationSequenceID", "GenericCalibrationCalibrationSubTypeId", "GenericCalibrationWorkOrderDetailId" },
                principalTable: "GenericCalibration",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });
        }
    }
}

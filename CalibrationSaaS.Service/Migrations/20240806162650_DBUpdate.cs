using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    /// <inheritdoc />
    public partial class DBUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.DropIndex(
                name: "IX_DynamicProperty_ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "AccuracyPercentage",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "AccuracyPercentage",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "ToleranceTypeID",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "ToleranceValue",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "HasWorkOrdeDetailStandard",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "AccuracyPercentage",
                table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                name: "ActionFormula",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "ActionBeginRow",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "EnabledHeader",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CalibrationSubType");

            migrationBuilder.RenameColumn(
                name: "ToleranceTypeID",
                table: "EquipmentTemplate",
                newName: "EquipmentTemplateParent");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "DynamicProperty",
                newName: "Pattern");

            migrationBuilder.RenameColumn(
                name: "DefaultValueFormula",
                table: "DynamicProperty",
                newName: "JSONConfiguration");

            migrationBuilder.RenameColumn(
                name: "HasDelete",
                table: "CalibrationSubTypeView",
                newName: "HasDeleteSubType");

            migrationBuilder.RenameColumn(
                name: "CreateCopy",
                table: "CalibrationSubType",
                newName: "Mandatory");

            migrationBuilder.AddColumn<string>(
                name: "JsonTolerance",
                table: "WorkOrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerInvoice",
                table: "WorkOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Invoice",
                table: "WorkOrder",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JSONConfiguration",
                table: "ViewPropertyBase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassOrFailJSON",
                table: "ViewPropertyBase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Uncertainty",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "RangeMin",
                table: "Uncertainty",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "RangeMax",
                table: "Uncertainty",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "Contributors",
                table: "Uncertainty",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UncertantyOperation",
                table: "Uncertainty",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Enable",
                table: "ToleranceType",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EquipmentTypeID",
                table: "TestCode",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Rol",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JsonTolerance",
                table: "PieceOfEquipment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "To",
                table: "Lenght",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "From",
                table: "Lenght",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "KeyObject",
                table: "GenericCalibrationResult2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExtendedObject",
                table: "GenericCalibrationResult",
                type: "varchar(5000)",
                unicode: false,
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ETCalculatedAlgorithm",
                table: "EquipmentType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EquipmentTypeGroupID",
                table: "EquipmentType",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JSONConfiguration",
                table: "EquipmentType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentTypeID",
                table: "EquipmentTemplate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EquipmentTypeGroupID",
                table: "EquipmentTemplate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "FullScale",
                table: "EquipmentTemplate",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonTolerance",
                table: "EquipmentTemplate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMaxField",
                table: "DynamicProperty",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isRequired",
                table: "DynamicProperty",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "unique",
                table: "DynamicProperty",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomID",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EnableSelect2",
                table: "CalibrationSubTypeView",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "CSSGrid",
                table: "CalibrationSubTypeView",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "CSSWidth",
                table: "CalibrationSubTypeView",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CSVValidator",
                table: "CalibrationSubTypeView",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultDecimalNumber",
                table: "CalibrationSubTypeView",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "JSONConfiguration",
                table: "CalibrationSubTypeView",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UncertaintyDescription",
                table: "CalibrationSubTypeView",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseContext",
                table: "CalibrationSubTypeView",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupportCSV",
                table: "CalibrationSubType",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "BasicCalibrationResult",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BasicCalibrationResult",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentTypeGroup",
                columns: table => new
                {
                    EquipmentTypeGroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypeGroup", x => x.EquipmentTypeGroupID);
                });

            migrationBuilder.CreateTable(
                name: "GenericCalibrationResult2Aggregate",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    ComponentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Component = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JSON = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericCalibrationResult2Aggregate", x => new { x.SequenceID, x.CalibrationSubTypeId, x.ComponentID });
                });

            migrationBuilder.CreateTable(
                name: "Tolerance",
                columns: table => new
                {
                    ToleranceTypeID = table.Column<int>(type: "int", nullable: true),
                    FullScale = table.Column<bool>(type: "bit", nullable: true),
                    TolerancePercentage = table.Column<double>(type: "float", nullable: false),
                    ToleranceValue = table.Column<double>(type: "float", nullable: false),
                    ToleranceFixedValue = table.Column<double>(type: "float", nullable: false),
                    ResolutionValue = table.Column<double>(type: "float", nullable: false),
                    AccuracyPercentage = table.Column<double>(type: "float", nullable: false),
                    MaxValue = table.Column<double>(type: "float", nullable: false),
                    Resolution = table.Column<double>(type: "float", nullable: false),
                    DecimalNumber = table.Column<int>(type: "int", nullable: false),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Changed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "WOD_ParametersTable",
                columns: table => new
                {
                    WorkOrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    urlWebOneDrive = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOD_ParametersTable", x => x.WorkOrderDetailID);
                });

            migrationBuilder.CreateTable(
                name: "WOD_Procedure",
                columns: table => new
                {
                    WorkOrderDetailID = table.Column<int>(type: "int", nullable: false),
                    ProcedureID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOD_Procedure", x => new { x.WorkOrderDetailID, x.ProcedureID });
                    table.ForeignKey(
                        name: "FK_WOD_Procedure_Procedure_ProcedureID",
                        column: x => x.ProcedureID,
                        principalTable: "Procedure",
                        principalColumn: "ProcedureID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WOD_Procedure_WorkOrderDetail_WorkOrderDetailID",
                        column: x => x.WorkOrderDetailID,
                        principalTable: "WorkOrderDetail",
                        principalColumn: "WorkOrderDetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCode_EquipmentTypeID",
                table: "TestCode",
                column: "EquipmentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTemplate_EquipmentTypeGroupID",
                table: "EquipmentTemplate",
                column: "EquipmentTypeGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicProperty_ViewPropertyBaseID",
                table: "DynamicProperty",
                column: "ViewPropertyBaseID");

            migrationBuilder.CreateIndex(
                name: "IX_WOD_Procedure_ProcedureID",
                table: "WOD_Procedure",
                column: "ProcedureID");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseID",
                table: "DynamicProperty",
                column: "ViewPropertyBaseID",
                principalTable: "ViewPropertyBase",
                principalColumn: "ViewPropertyID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentTemplate_EquipmentTypeGroup_EquipmentTypeGroupID",
                table: "EquipmentTemplate",
                column: "EquipmentTypeGroupID",
                principalTable: "EquipmentTypeGroup",
                principalColumn: "EquipmentTypeGroupID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestCode_EquipmentType_EquipmentTypeID",
                table: "TestCode",
                column: "EquipmentTypeID",
                principalTable: "EquipmentType",
                principalColumn: "EquipmentTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentTemplate_EquipmentTypeGroup_EquipmentTypeGroupID",
                table: "EquipmentTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_TestCode_EquipmentType_EquipmentTypeID",
                table: "TestCode");

            migrationBuilder.DropTable(
                name: "EquipmentTypeGroup");

            migrationBuilder.DropTable(
                name: "GenericCalibrationResult2Aggregate");

            migrationBuilder.DropTable(
                name: "Tolerance");

            migrationBuilder.DropTable(
                name: "WOD_ParametersTable");

            migrationBuilder.DropTable(
                name: "WOD_Procedure");

            migrationBuilder.DropIndex(
                name: "IX_TestCode_EquipmentTypeID",
                table: "TestCode");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentTemplate_EquipmentTypeGroupID",
                table: "EquipmentTemplate");

            migrationBuilder.DropIndex(
                name: "IX_DynamicProperty_ViewPropertyBaseID",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "JsonTolerance",
                table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "CustomerInvoice",
                table: "WorkOrder");

            migrationBuilder.DropColumn(
                name: "Invoice",
                table: "WorkOrder");

            migrationBuilder.DropColumn(
                name: "JSONConfiguration",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "PassOrFailJSON",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Contributors",
                table: "Uncertainty");

            migrationBuilder.DropColumn(
                name: "UncertantyOperation",
                table: "Uncertainty");

            migrationBuilder.DropColumn(
                name: "Enable",
                table: "ToleranceType");

            migrationBuilder.DropColumn(
                name: "EquipmentTypeID",
                table: "TestCode");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "JsonTolerance",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "KeyObject",
                table: "GenericCalibrationResult2");

            migrationBuilder.DropColumn(
                name: "ETCalculatedAlgorithm",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "EquipmentTypeGroupID",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "JSONConfiguration",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "EquipmentTypeGroupID",
                table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                name: "FullScale",
                table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                name: "JsonTolerance",
                table: "EquipmentTemplate");

            migrationBuilder.DropColumn(
                name: "IsMaxField",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "isRequired",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "unique",
                table: "DynamicProperty");

            migrationBuilder.DropColumn(
                name: "CustomID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CSSWidth",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "CSVValidator",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "DefaultDecimalNumber",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "JSONConfiguration",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "UncertaintyDescription",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "UseContext",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "SupportCSV",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BasicCalibrationResult");

            migrationBuilder.RenameColumn(
                name: "EquipmentTemplateParent",
                table: "EquipmentTemplate",
                newName: "ToleranceTypeID");

            migrationBuilder.RenameColumn(
                name: "Pattern",
                table: "DynamicProperty",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "JSONConfiguration",
                table: "DynamicProperty",
                newName: "DefaultValueFormula");

            migrationBuilder.RenameColumn(
                name: "HasDeleteSubType",
                table: "CalibrationSubTypeView",
                newName: "HasDelete");

            migrationBuilder.RenameColumn(
                name: "Mandatory",
                table: "CalibrationSubType",
                newName: "CreateCopy");

            migrationBuilder.AddColumn<double>(
                name: "AccuracyPercentage",
                table: "WorkOrderDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Uncertainty",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RangeMin",
                table: "Uncertainty",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RangeMax",
                table: "Uncertainty",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AccuracyPercentage",
                table: "PieceOfEquipment",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ToleranceTypeID",
                table: "PieceOfEquipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ToleranceValue",
                table: "PieceOfEquipment",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "To",
                table: "Lenght",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "From",
                table: "Lenght",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "ExtendedObject",
                table: "GenericCalibrationResult",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5000)",
                oldUnicode: false,
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasWorkOrdeDetailStandard",
                table: "EquipmentType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentTypeID",
                table: "EquipmentTemplate",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AccuracyPercentage",
                table: "EquipmentTemplate",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ActionFormula",
                table: "DynamicProperty",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EnableSelect2",
                table: "CalibrationSubTypeView",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CSSGrid",
                table: "CalibrationSubTypeView",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ActionBeginRow",
                table: "CalibrationSubTypeView",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnabledHeader",
                table: "CalibrationSubTypeView",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CalibrationSubType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "BasicCalibrationResult",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(34)",
                oldMaxLength: 34);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicProperty_ViewPropertyBaseID",
                table: "DynamicProperty",
                column: "ViewPropertyBaseID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicProperty_ViewPropertyBase_ViewPropertyBaseID",
                table: "DynamicProperty",
                column: "ViewPropertyBaseID",
                principalTable: "ViewPropertyBase",
                principalColumn: "ViewPropertyID");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    /// <inheritdoc />
    public partial class inicial7001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CalibrationSubType_Weight_Eccentricity_CalibrationSubTypeID_WorkOrderDetailID",
            //    table: "CalibrationSubType_Weight");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentType_CalibrationType_CalibrationTypeID",
                table: "EquipmentType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalibrationSubType_Weight",
                table: "CalibrationSubType_Weight");

            //migrationBuilder.DropIndex(
            //    name: "IX_CalibrationSubType_Weight_CalibrationSubTypeID_WorkOrderDetailID",
            //    table: "CalibrationSubType_Weight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalibrationSubType_Standard",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "CalibrationSubTypeID",
                table: "WorkOrderDetail");

            //migrationBuilder.AddColumn<string>(
            //    name: "WorkOrderDetailUserID",
            //    table: "WorkOrderDetail",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibration2CalibrationSubTypeId",
                table: "WeightSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GenericCalibration2ComponentID",
                table: "WeightSet",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibration2SequenceID",
                table: "WeightSet",
                type: "int",
                nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "ExtendedObject",
            //    table: "ViewPropertyBase",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "FormulaProperty",
            //    table: "ViewPropertyBase",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Resolution",
                table: "GenericCalibrationResult",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DecimalNumber",
                table: "GenericCalibrationResult",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ComponentID",
            //    table: "GenericCalibrationResult",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ComponentID",
            //    table: "GenericCalibration",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CalibrationTypeID",
                table: "EquipmentType",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "DynamicConfiguration",
            //    table: "EquipmentType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "ActionFormula",
            //    table: "DynamicProperty",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "FormulaClass",
            //    table: "DynamicProperty",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CalibrationProvider",
            //    table: "CertificatePoE",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "ShowType",
            //    table: "CalibrationType",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "CloseButton",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ComponentID",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCollection",
                table: "CalibrationSubTypeView",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsVisible",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "SaveButton",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Style",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseResult",
                table: "CalibrationSubTypeView",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkOrderDetailID",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.AddColumn<string>(
            //    name: "ComponentID",
            //    table: "CalibrationSubType_Weight",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EccentricityCalibrationSubTypeId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EccentricityWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibration2CalibrationSubTypeId",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GenericCalibration2ComponentID",
                table: "CalibrationSubType_Weight",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibration2SequenceID",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkOrderDetailID",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.AddColumn<string>(
            //    name: "ComponentID",
            //    table: "CalibrationSubType_Standard",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibration2CalibrationSubTypeId",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GenericCalibration2ComponentID",
                table: "CalibrationSubType_Standard",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibration2SequenceID",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComponentID",
                table: "BasicCalibrationResult",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalibrationSubType_Weight",
                table: "CalibrationSubType_Weight",
                columns: new[] { "ComponentID", "WeightSetID", "CalibrationSubTypeID", "SecuenceID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalibrationSubType_Standard",
                table: "CalibrationSubType_Standard",
                columns: new[] { "ComponentID", "PieceOfEquipmentID", "CalibrationSubTypeID", "SecuenceID" });

            migrationBuilder.CreateTable(
                name: "GenericCalibration2",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    ComponentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    NumberOfSamples = table.Column<int>(type: "int", nullable: false),
                    TestPointID = table.Column<int>(type: "int", nullable: true),
                    UnitOfMeasureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericCalibration2", x => new { x.SequenceID, x.CalibrationSubTypeId, x.ComponentID });
                    table.ForeignKey(
                        name: "FK_GenericCalibration2_TestPoint_TestPointID",
                        column: x => x.TestPointID,
                        principalTable: "TestPoint",
                        principalColumn: "TestPointID");
                });

            migrationBuilder.CreateTable(
                name: "GenericCalibrationResult2",
                columns: table => new
                {
                    SequenceID = table.Column<int>(type: "int", nullable: false),
                    CalibrationSubTypeId = table.Column<int>(type: "int", nullable: false),
                    ComponentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkOrderDetailId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Resolution = table.Column<double>(type: "float", nullable: false),
                    DecimalNumber = table.Column<int>(type: "int", nullable: false),
                    Object = table.Column<string>(type: "varchar(max)", unicode: false, maxLength: 10000, nullable: true),
                    ExtendedObject = table.Column<string>(type: "varchar(max)", unicode: false, maxLength: 10000, nullable: true),
                    PieceOfEquipmentID = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericCalibrationResult2", x => new { x.SequenceID, x.CalibrationSubTypeId, x.ComponentID });
                    table.ForeignKey(
                        name: "FK_GenericCalibrationResult2_GenericCalibration2_SequenceID_CalibrationSubTypeId_ComponentID",
                        columns: x => new { x.SequenceID, x.CalibrationSubTypeId, x.ComponentID },
                        principalTable: "GenericCalibration2",
                        principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "ComponentID" });
                    table.ForeignKey(
                        name: "FK_GenericCalibrationResult2_PieceOfEquipment_PieceOfEquipmentID",
                        column: x => x.PieceOfEquipmentID,
                        principalTable: "PieceOfEquipment",
                        principalColumn: "PieceOfEquipmentID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightSet_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2ComponentID",
                table: "WeightSet",
                columns: new[] { "GenericCalibration2SequenceID", "GenericCalibration2CalibrationSubTypeId", "GenericCalibration2ComponentID" });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Weight_CalibrationSubTypeID",
                table: "CalibrationSubType_Weight",
                column: "CalibrationSubTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Weight_EccentricityCalibrationSubTypeId_EccentricityWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                columns: new[] { "EccentricityCalibrationSubTypeId", "EccentricityWorkOrderDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Weight_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2Component~",
                table: "CalibrationSubType_Weight",
                columns: new[] { "GenericCalibration2SequenceID", "GenericCalibration2CalibrationSubTypeId", "GenericCalibration2ComponentID" });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Standard_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2Compone~",
                table: "CalibrationSubType_Standard",
                columns: new[] { "GenericCalibration2SequenceID", "GenericCalibration2CalibrationSubTypeId", "GenericCalibration2ComponentID" });

            migrationBuilder.CreateIndex(
                name: "IX_GenericCalibration2_TestPointID",
                table: "GenericCalibration2",
                column: "TestPointID");

            migrationBuilder.CreateIndex(
                name: "IX_GenericCalibrationResult2_PieceOfEquipmentID",
                table: "GenericCalibrationResult2",
                column: "PieceOfEquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Standard_GenericCalibration2_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_Generi~",
                table: "CalibrationSubType_Standard",
                columns: new[] { "GenericCalibration2SequenceID", "GenericCalibration2CalibrationSubTypeId", "GenericCalibration2ComponentID" },
                principalTable: "GenericCalibration2",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "ComponentID" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Weight_Eccentricity_EccentricityCalibrationSubTypeId_EccentricityWorkOrderDetailId",
                table: "CalibrationSubType_Weight",
                columns: new[] { "EccentricityCalibrationSubTypeId", "EccentricityWorkOrderDetailId" },
                principalTable: "Eccentricity",
                principalColumns: new[] { "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Weight_GenericCalibration2_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericC~",
                table: "CalibrationSubType_Weight",
                columns: new[] { "GenericCalibration2SequenceID", "GenericCalibration2CalibrationSubTypeId", "GenericCalibration2ComponentID" },
                principalTable: "GenericCalibration2",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "ComponentID" });

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentType_CalibrationType_CalibrationTypeID",
                table: "EquipmentType",
                column: "CalibrationTypeID",
                principalTable: "CalibrationType",
                principalColumn: "CalibrationTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightSet_GenericCalibration2_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2Compo~",
                table: "WeightSet",
                columns: new[] { "GenericCalibration2SequenceID", "GenericCalibration2CalibrationSubTypeId", "GenericCalibration2ComponentID" },
                principalTable: "GenericCalibration2",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "ComponentID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Standard_GenericCalibration2_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_Generi~",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Weight_Eccentricity_EccentricityCalibrationSubTypeId_EccentricityWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationSubType_Weight_GenericCalibration2_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericC~",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentType_CalibrationType_CalibrationTypeID",
                table: "EquipmentType");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightSet_GenericCalibration2_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2Compo~",
                table: "WeightSet");

            migrationBuilder.DropTable(
                name: "GenericCalibrationResult2");

            migrationBuilder.DropTable(
                name: "GenericCalibration2");

            migrationBuilder.DropIndex(
                name: "IX_WeightSet_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2ComponentID",
                table: "WeightSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalibrationSubType_Weight",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Weight_CalibrationSubTypeID",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Weight_EccentricityCalibrationSubTypeId_EccentricityWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Weight_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2Component~",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalibrationSubType_Standard",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationSubType_Standard_GenericCalibration2SequenceID_GenericCalibration2CalibrationSubTypeId_GenericCalibration2Compone~",
                table: "CalibrationSubType_Standard");

            //migrationBuilder.DropColumn(
            //    name: "WorkOrderDetailUserID",
            //    table: "WorkOrderDetail");

            migrationBuilder.DropColumn(
                name: "GenericCalibration2CalibrationSubTypeId",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "GenericCalibration2ComponentID",
                table: "WeightSet");

            migrationBuilder.DropColumn(
                name: "GenericCalibration2SequenceID",
                table: "WeightSet");

            //migrationBuilder.DropColumn(
            //    name: "ExtendedObject",
            //    table: "ViewPropertyBase");

            //migrationBuilder.DropColumn(
            //    name: "FormulaProperty",
            //    table: "ViewPropertyBase");

            //migrationBuilder.DropColumn(
            //    name: "ComponentID",
            //    table: "GenericCalibrationResult");

            //migrationBuilder.DropColumn(
            //    name: "ComponentID",
            //    table: "GenericCalibration");

            //migrationBuilder.DropColumn(
            //    name: "DynamicConfiguration",
            //    table: "EquipmentType");

            //migrationBuilder.DropColumn(
            //    name: "ActionFormula",
            //    table: "DynamicProperty");

            //migrationBuilder.DropColumn(
            //    name: "FormulaClass",
            //    table: "DynamicProperty");

            //migrationBuilder.DropColumn(
            //    name: "CalibrationProvider",
            //    table: "CertificatePoE");

            //migrationBuilder.DropColumn(
            //    name: "ShowType",
            //    table: "CalibrationType");

            //migrationBuilder.DropColumn(
            //    name: "CloseButton",
            //    table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "ComponentID",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "IsCollection",
                table: "CalibrationSubTypeView");

            //migrationBuilder.DropColumn(
            //    name: "IsVisible",
            //    table: "CalibrationSubTypeView");

            //migrationBuilder.DropColumn(
            //    name: "SaveButton",
            //    table: "CalibrationSubTypeView");

            //migrationBuilder.DropColumn(
            //    name: "Style",
            //    table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "UseResult",
                table: "CalibrationSubTypeView");

            //migrationBuilder.DropColumn(
            //    name: "ComponentID",
            //    table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "EccentricityCalibrationSubTypeId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "EccentricityWorkOrderDetailId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "GenericCalibration2CalibrationSubTypeId",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "GenericCalibration2ComponentID",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "GenericCalibration2SequenceID",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "ComponentID",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "GenericCalibration2CalibrationSubTypeId",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "GenericCalibration2ComponentID",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "GenericCalibration2SequenceID",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "ComponentID",
                table: "BasicCalibrationResult");

            migrationBuilder.AddColumn<int>(
                name: "CalibrationSubTypeID",
                table: "WorkOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "Resolution",
                table: "GenericCalibrationResult",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "DecimalNumber",
                table: "GenericCalibrationResult",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CalibrationTypeID",
                table: "EquipmentType",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WorkOrderDetailID",
                table: "CalibrationSubType_Weight",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkOrderDetailID",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalibrationSubType_Weight",
                table: "CalibrationSubType_Weight",
                columns: new[] { "WorkOrderDetailID", "WeightSetID", "CalibrationSubTypeID", "SecuenceID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalibrationSubType_Standard",
                table: "CalibrationSubType_Standard",
                columns: new[] { "WorkOrderDetailID", "PieceOfEquipmentID", "CalibrationSubTypeID", "SecuenceID" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CalibrationSubType_Weight_CalibrationSubTypeID_WorkOrderDetailID",
            //    table: "CalibrationSubType_Weight",
            //    columns: new[] { "CalibrationSubTypeID", "WorkOrderDetailID" });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_CalibrationSubType_Weight_Eccentricity_CalibrationSubTypeID_WorkOrderDetailID",
            //    table: "CalibrationSubType_Weight",
            //    columns: new[] { "CalibrationSubTypeID", "WorkOrderDetailID" },
            //    principalTable: "Eccentricity",
            //    principalColumns: new[] { "CalibrationSubTypeId", "WorkOrderDetailId" },
            //    onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentType_CalibrationType_CalibrationTypeID",
                table: "EquipmentType",
                column: "CalibrationTypeID",
                principalTable: "CalibrationType",
                principalColumn: "CalibrationTypeId");
        }
    }
}

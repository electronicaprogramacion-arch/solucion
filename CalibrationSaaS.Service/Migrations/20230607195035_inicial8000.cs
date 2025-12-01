using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalibrationSaaS.Infraestructure.GrpcServices.Migrations
{
    /// <inheritdoc />
    public partial class inicial8000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_CalibrationSubType_Standard_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCa~",
            //    table: "CalibrationSubType_Standard");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_GenericCalibrationResult2_PieceOfEquipment_PieceOfEquipmentID",
            //    table: "GenericCalibrationResult2");

            //migrationBuilder.DropIndex(
            //    name: "IX_GenericCalibrationResult2_PieceOfEquipmentID",
            //    table: "GenericCalibrationResult2");

            //migrationBuilder.DropIndex(
            //    name: "IX_CalibrationSubType_Standard_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderD~",
            //    table: "CalibrationSubType_Standard");

            //migrationBuilder.DropColumn(
            //    name: "IsEnabled",
            //    table: "User");

            //migrationBuilder.DropColumn(
            //    name: "PasswordReset",
            //    table: "User");

            //migrationBuilder.DropColumn(
            //    name: "PieceOfEquipmentID",
            //    table: "GenericCalibrationResult2");

            //migrationBuilder.DropColumn(
            //    name: "GenericCalibrationCalibrationSubTypeId",
            //    table: "CalibrationSubType_Standard");

            //migrationBuilder.DropColumn(
            //    name: "GenericCalibrationSequenceID",
            //    table: "CalibrationSubType_Standard");

            //migrationBuilder.DropColumn(
            //    name: "GenericCalibrationWorkOrderDetailId",
            //    table: "CalibrationSubType_Standard");

            //migrationBuilder.DropColumn(
            //    name: "ComponentID",
            //    table: "BasicCalibrationResult");

            //migrationBuilder.RenameColumn(
            //    name: "ComponentID",
            //    table: "CalibrationSubTypeView",
            //    newName: "Component");

            //migrationBuilder.AddColumn<string>(
            //    name: "ColGroup",
            //    table: "ViewPropertyBase",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ColGroupCSS",
            //    table: "ViewPropertyBase",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ColGroupTitle",
            //    table: "ViewPropertyBase",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "RowCSSCol",
            //    table: "ViewPropertyBase",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "Rol",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "Procedure",
            //    type: "nvarchar(50)",
            //    maxLength: 50,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(50)",
            //    oldMaxLength: 50);

            //migrationBuilder.AddColumn<string>(
            //    name: "DocumentUrl",
            //    table: "Procedure",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AlterColumn<double>(
            //    name: "ToleranceHV",
            //    table: "PieceOfEquipment",
            //    type: "float",
            //    nullable: true,
            //    oldClrType: typeof(double),
            //    oldType: "float");

            //migrationBuilder.AlterColumn<double>(
            //    name: "LoadKGF",
            //    table: "PieceOfEquipment",
            //    type: "float",
            //    nullable: true,
            //    oldClrType: typeof(double),
            //    oldType: "float");

            //migrationBuilder.AddColumn<int>(
            //    name: "TestCodeID",
            //    table: "PieceOfEquipment",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "CalibrationSubtypeId",
            //    table: "NoteWOD",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Component",
            //    table: "GenericCalibrationResult2",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Component",
            //    table: "GenericCalibration2",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CalibrationInstrucctions",
            //    table: "EquipmentType",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "DynamicConfiguration2",
            //    table: "EquipmentType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "HasRepeateabilityAndEccentricity",
            //    table: "EquipmentType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "HasReturnToZero",
            //    table: "EquipmentType",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "StandardComponent",
            //    table: "EquipmentType",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "UrlReport",
            //    table: "CalibrationType",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ColFormActionCSS",
            //    table: "CalibrationSubTypeView",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "EnableEdit",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsRowView",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "ShowCardButton",
            //    table: "CalibrationSubTypeView",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "Component",
            //    table: "CalibrationSubType_Weight",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Component",
            //    table: "CalibrationSubType_Standard",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "NewClass",
            //    table: "CalibrationSubType",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "OnChangeClass",
            //    table: "CalibrationSubType",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "Mass",
                columns: table => new
                {
                    MassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Metric = table.Column<int>(type: "int", nullable: false),
                    UnitOfMeasure = table.Column<int>(type: "int", nullable: false),
                    Class1 = table.Column<double>(type: "float", nullable: false),
                    Class1UoM = table.Column<int>(type: "int", nullable: false),
                    Class2 = table.Column<double>(type: "float", nullable: false),
                    Class2UoM = table.Column<int>(type: "int", nullable: false),
                    Class3 = table.Column<double>(type: "float", nullable: false),
                    Class3UoM = table.Column<int>(type: "int", nullable: false),
                    Class4 = table.Column<double>(type: "float", nullable: false),
                    Class4UoM = table.Column<int>(type: "int", nullable: false),
                    Class5 = table.Column<double>(type: "float", nullable: false),
                    Class5UoM = table.Column<int>(type: "int", nullable: false),
                    Class6 = table.Column<double>(type: "float", nullable: false),
                    Class6UoM = table.Column<int>(type: "int", nullable: false),
                    Class7 = table.Column<double>(type: "float", nullable: false),
                    Class7UoM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mass", x => x.MassID);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Rol_Name",
            //    table: "Rol",
            //    column: "Name",
            //    unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mass");

            migrationBuilder.DropIndex(
                name: "IX_Rol_Name",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "ColGroup",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "ColGroupCSS",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "ColGroupTitle",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "RowCSSCol",
                table: "ViewPropertyBase");

            migrationBuilder.DropColumn(
                name: "DocumentUrl",
                table: "Procedure");

            migrationBuilder.DropColumn(
                name: "TestCodeID",
                table: "PieceOfEquipment");

            migrationBuilder.DropColumn(
                name: "CalibrationSubtypeId",
                table: "NoteWOD");

            migrationBuilder.DropColumn(
                name: "Component",
                table: "GenericCalibrationResult2");

            migrationBuilder.DropColumn(
                name: "Component",
                table: "GenericCalibration2");

            migrationBuilder.DropColumn(
                name: "CalibrationInstrucctions",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "DynamicConfiguration2",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "HasRepeateabilityAndEccentricity",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "HasReturnToZero",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "StandardComponent",
                table: "EquipmentType");

            migrationBuilder.DropColumn(
                name: "UrlReport",
                table: "CalibrationType");

            migrationBuilder.DropColumn(
                name: "ColFormActionCSS",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "EnableEdit",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "IsRowView",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "ShowCardButton",
                table: "CalibrationSubTypeView");

            migrationBuilder.DropColumn(
                name: "Component",
                table: "CalibrationSubType_Weight");

            migrationBuilder.DropColumn(
                name: "Component",
                table: "CalibrationSubType_Standard");

            migrationBuilder.DropColumn(
                name: "NewClass",
                table: "CalibrationSubType");

            migrationBuilder.DropColumn(
                name: "OnChangeClass",
                table: "CalibrationSubType");

            migrationBuilder.RenameColumn(
                name: "Component",
                table: "CalibrationSubTypeView",
                newName: "ComponentID");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PasswordReset",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Rol",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Procedure",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ToleranceHV",
                table: "PieceOfEquipment",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LoadKGF",
                table: "PieceOfEquipment",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PieceOfEquipmentID",
                table: "GenericCalibrationResult2",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationCalibrationSubTypeId",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationSequenceID",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenericCalibrationWorkOrderDetailId",
                table: "CalibrationSubType_Standard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComponentID",
                table: "BasicCalibrationResult",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GenericCalibrationResult2_PieceOfEquipmentID",
                table: "GenericCalibrationResult2",
                column: "PieceOfEquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationSubType_Standard_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCalibrationWorkOrderD~",
                table: "CalibrationSubType_Standard",
                columns: new[] { "GenericCalibrationSequenceID", "GenericCalibrationCalibrationSubTypeId", "GenericCalibrationWorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationSubType_Standard_GenericCalibration_GenericCalibrationSequenceID_GenericCalibrationCalibrationSubTypeId_GenericCa~",
                table: "CalibrationSubType_Standard",
                columns: new[] { "GenericCalibrationSequenceID", "GenericCalibrationCalibrationSubTypeId", "GenericCalibrationWorkOrderDetailId" },
                principalTable: "GenericCalibration",
                principalColumns: new[] { "SequenceID", "CalibrationSubTypeId", "WorkOrderDetailId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GenericCalibrationResult2_PieceOfEquipment_PieceOfEquipmentID",
                table: "GenericCalibrationResult2",
                column: "PieceOfEquipmentID",
                principalTable: "PieceOfEquipment",
                principalColumn: "PieceOfEquipmentID");
        }
    }
}

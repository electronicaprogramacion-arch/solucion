using Blazor.IndexedDB.Framework;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics.TestPoints;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazed.Controls.Toast;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using Helpers;

namespace CalibrationSaaS.Infraestructure.Blazor.Shared
{
    public class CalibrationInstructionsBase_ : CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<WorkOrderDetail>
    {
        public ResolutionComponent ResolutionComponent { get; set; }

        public bool IsPercentage = false;

      

        public WorkOrderDetail eq { get
            {

               
                return WorkOrderItemCreate.eq;


            } 
            
            set {

                WorkOrderItemCreate.eq = value;
            } 
        }

        public void LoadConditionData()
        {
            LoadConditionData(WorkOrderItemCreate.eq);
        }

        public Dictionary<int, string> Modes { get; set; } = new Dictionary<int, string>();

        [CascadingParameter(Name = "CascadeParam1")]
        public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }
        
        public EditContext editContext { get; set; }
        protected override async Task OnInitializedAsync()
        {

            Modes.Add(1, "Method1");

            Modes.Add(2, "Method1");


            if (WorkOrderItemCreate == null || WorkOrderItemCreate.eq == null)
            {
                editContext = new EditContext(eq);
            }
            else
            {
                
                editContext = new EditContext(WorkOrderItemCreate.eq);
            }

            //LoadConditionData();


            await base.OnInitializedAsync();

        }


            public void LoadConditionData(WorkOrderDetail eq2)
        {
            //if (eq2 != null && (eq2?.CalibrationTypeID == null || eq2?.CalibrationTypeID == 0) && eq2?.WorkOder?.CalibrationType > 0)
            //{
            //    eq2.CalibrationTypeID = eq2.WorkOder.CalibrationType;
            //}



            var equipmentTemplate = new EquipmentTemplate();


            if (eq2.PieceOfEquipment != null && eq2.PieceOfEquipment.IsToleranceImport 
                && (eq2.Tolerance == null || !eq2.Tolerance.ToleranceTypeID.HasValue))
            {
                var q = eq2.PieceOfEquipment;
                //equipmentTemplate.Tolerance.ToleranceTypeID = q.Tolerance.ToleranceTypeID;

                //equipmentTemplate.Tolerance.AccuracyPercentage = q.Tolerance.AccuracyPercentage;


                equipmentTemplate.Tolerance = q.Tolerance;

                equipmentTemplate.Resolution = q.Resolution;
                equipmentTemplate.DecimalNumber = q.DecimalNumber;
                equipmentTemplate.ClassHB44 = q.ClassHB44;
                equipmentTemplate.Ranges = q.Ranges;

            }
            else if (!eq2.PieceOfEquipment.IsToleranceImport && eq2?.PieceOfEquipment?.EquipmentTemplate != null 
                && (eq2.Tolerance == null || !eq2.Tolerance.ToleranceTypeID.HasValue))
            {
                 equipmentTemplate = eq2.PieceOfEquipment.EquipmentTemplate;

                equipmentTemplate.Resolution= eq2.PieceOfEquipment.EquipmentTemplate.Resolution;

              
                equipmentTemplate.DecimalNumber = eq2.PieceOfEquipment.EquipmentTemplate.DecimalNumber;
                equipmentTemplate.ClassHB44 = eq2.PieceOfEquipment.EquipmentTemplate.ClassHB44;
                equipmentTemplate.Ranges = eq2.PieceOfEquipment.EquipmentTemplate.Ranges;

                equipmentTemplate.Tolerance = eq2.PieceOfEquipment.EquipmentTemplate.Tolerance;
            }


            if (eq2 != null && eq2.Resolution == 0 && equipmentTemplate?.Resolution > 0)
            {
                eq2.Resolution = equipmentTemplate.Resolution;
            }
            if (eq2 != null && eq2.ClassHB44 == 0 && equipmentTemplate?.ClassHB44 > 0)
            {
                eq2.ClassHB44 = equipmentTemplate.ClassHB44;
            }
            if (eq2 != null && eq2.DecimalNumber == 0 && equipmentTemplate?.DecimalNumber > 0)
            {
                eq2.DecimalNumber = equipmentTemplate.DecimalNumber;
            }

            //TODO falata codigo para ranges
            if (equipmentTemplate != null && equipmentTemplate?.Ranges?.Count > 0)
            {
                eq2.Ranges = equipmentTemplate?.Ranges;
            }
            //eq.IsComercial = eq.PieceOfEquipment.EquipmentTemplate.IsComercial;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (eq2?.Tolerance?.ToleranceTypeID == 2)
            {
                IsPercentage = true;
            }
            if (eq2 == null || !eq2.CalibrationDate.HasValue || eq2.CalibrationDate.Value.Year == 1)
            {
                eq2.CalibrationDate = DateTime.Now;
            }
            if (eq2 == null || !eq2.CalibrationDate.HasValue || eq2.CalibrationDate.Value.Year == 1)
            {
                eq2.CalibrationNextDueDate = DateTime.Now;
            }

            if((eq2.Tolerance == null || !eq2.Tolerance.ToleranceTypeID.HasValue))
            {
                eq2.Tolerance = equipmentTemplate.Tolerance;
            }
            

            //ypp
            if (ResolutionComponent != null)
            {
                EquipmentTemplate et = new EquipmentTemplate();
                //et.Tolerance.ToleranceTypeID = eq.Tolerance.ToleranceTypeID;
                //et.Tolerance.AccuracyPercentage = eq.Tolerance.AccuracyPercentage;

                et.Tolerance=(Tolerance) eq2.Tolerance.CloneObject();
                //et.Tolerance.Resolution= eq.Resolution;   
                //et.Tolerance.ResolutionValue = eq.ResolutionValue;
                
                et.Resolution = eq2.Resolution;
                et.DecimalNumber = eq2.DecimalNumber;
                //ResolutionComponent.Tolerance = et.Tolerance;
                et.Ranges = eq2.Ranges;
                et.ClassHB44 = eq2.ClassHB44;
#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                ResolutionComponent.Show(et);
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
            }

            
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (!firstRender && ResolutionComponent != null )
            {
                LoadConditionData();
                //LoadConditionData();
            }

            //LoadConditionData();
            await base.OnAfterRenderAsync(firstRender);
        }

        public bool changeTolerance { get; set; }
        public async Task CloseWindow()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            Console.WriteLine("CloseWindow CALINStruc");

            Console.WriteLine(WorkOrderItemCreate.eq.Resolution);

            Console.WriteLine(ResolutionComponent.eq.Resolution);

            Console.WriteLine(ResolutionComponent.ResolutionChanged);




            if (WorkOrderItemCreate.eq.IsComercial != ResolutionComponent.eq.IsComercial)
            {
                changeTolerance = true;
                WorkOrderItemCreate.eq.IsComercial = ResolutionComponent.eq.IsComercial;
            }
            if (WorkOrderItemCreate.eq.Tolerance.ToleranceTypeID != ResolutionComponent.Tolerance.ToleranceTypeID.Value)
            {
                changeTolerance = true;
                WorkOrderItemCreate.eq.Tolerance.ToleranceTypeID = ResolutionComponent.Tolerance.ToleranceTypeID.Value;
            }

            if (WorkOrderItemCreate.eq.Resolution != ResolutionComponent.eq.Resolution)
            {
                changeTolerance = true;
                //WorkOrderItemCreate.eq.Resolution = ResolutionComponent.eq.Resolution;
            }

            if (WorkOrderItemCreate.eq.DecimalNumber != ResolutionComponent.eq.DecimalNumber)
            {
                changeTolerance = true;
                //WorkOrderItemCreate.eq.DecimalNumber = ResolutionComponent.eq.DecimalNumber;
            }

            if (WorkOrderItemCreate.eq.ClassHB44 != ResolutionComponent.eq.ClassHB44)
            {
                changeTolerance = true;
                WorkOrderItemCreate.eq.ClassHB44 = ResolutionComponent.eq.ClassHB44;
            }

            //eq.IsComercial = ResolutionComponent.eq.IsComercial;

            //eq.Resolution= ResolutionComponent.eq.Resolution;

            WorkOrderItemCreate.ChangeResolution = changeTolerance | ResolutionComponent.ResolutionChanged | WorkOrderItemCreate.eq.Tolerance.Changed;

            if (WorkOrderItemCreate.ChangeResolution)
            {
                WorkOrderItemCreate.eq.Tolerance = ResolutionComponent.Tolerance;

                //WorkOrderItemCreate.eq.Tolerance.Changed = true;

                //WorkOrderItemCreate.eq.Resolution = ResolutionComponent.eq.Resolution;

                WorkOrderItemCreate.eq.JsonTolerance = ResolutionComponent.Tolerance.Json;

            }

            Console.WriteLine("closewindow" + WorkOrderItemCreate.ChangeResolution);

            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("Collapse", "ScaleCalibration");
            }
            Console.WriteLine(WorkOrderItemCreate.eq.CalibrationTypeID);

            StateHasChanged();

        }

    }
}

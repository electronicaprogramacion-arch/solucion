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
using Radzen.Blazor;
using System.Linq;
using System.Text.Json;
using Helpers.Controls;


namespace CalibrationSaaS.Infraestructure.Blazor.Shared
{
    public class CalibrationInstructionsBase_ : CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<WorkOrderDetail>
    {
        public ResolutionComponent ResolutionComponent { get; set; }

        public bool IsPercentage = false;

        public List<GenericCalibrationResult2> genericCalibrationResult2s = new List<GenericCalibrationResult2>();
        public List<KeyValueOption> keys = new List<KeyValueOption>();

        private IList<string> _values = new List<string>();
        public IList<string> values
        {
            get => _values;
            set
            {
                _values = value;
                OnValuesChangedInternal(_values);
            }
        }

       
        public IList<int> value { get; set; }

        [Parameter]
        public List<KeyValueOption> compositeWods { get; set; }

        [Parameter]
        public IList<int> compositesIds { get; set; }

        public KeyValueOption key = new KeyValueOption();


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

            if (WorkOrderItemCreate.eq.PieceOfEquipment.TestPointResult != null && WorkOrderItemCreate.eq.PieceOfEquipment.TestPointResult.Count() > 0)
            {
                genericCalibrationResult2s = WorkOrderItemCreate.eq.PieceOfEquipment.TestPointResult;
            }
            else if (WorkOrderItemCreate.eq.PieceOfEquipment.EquipmentTemplate.TestPointResult != null && WorkOrderItemCreate.eq.PieceOfEquipment.EquipmentTemplate.TestPointResult.Count() > 0)
            {
                genericCalibrationResult2s = WorkOrderItemCreate.eq.PieceOfEquipment.EquipmentTemplate.TestPointResult;
            }

            
           
            
            
            if (genericCalibrationResult2s != null && genericCalibrationResult2s.Count() > 0)
            {

               var list  = genericCalibrationResult2s.DistinctBy(x =>x.GroupName).Where(x=>x.GroupName != "TestPointResult").ToList();
                foreach (var item in list)
                {
                    KeyValueOption key = new KeyValueOption()
                    {
                        Key = NumericExtensions.GetUniqueID().ToString(),
                        Value = item.GroupName.ToString()
                    };
                         keys.Add(key);
                }
            }

            ///List for compositeWods DrowpDown
            ///


            // Load previously selected values from JsonTestPointsGroups
            LoadSelectedTestPointGroups();

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

            //Console.WriteLine("CloseWindow CALINStruc");

            //Console.WriteLine(WorkOrderItemCreate.eq.Resolution);

            //Console.WriteLine(ResolutionComponent.eq.Resolution);

            //Console.WriteLine(ResolutionComponent.ResolutionChanged);




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

            //Console.WriteLine("closewindow" + WorkOrderItemCreate.ChangeResolution);

            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("Collapse", "ScaleCalibration");
            }
            //Console.WriteLine(WorkOrderItemCreate.eq.CalibrationTypeID);

            StateHasChanged();

        }

        /// <summary>
        /// Loads previously selected test point groups from JsonTestPointsGroups
        /// </summary>
        private void LoadSelectedTestPointGroups()
        {
            try
            {
                if (WorkOrderItemCreate?.eq != null && !string.IsNullOrEmpty(WorkOrderItemCreate.eq.JsonTestPointsGroups))
                {
                    // Deserialize the JSON string back to a list of strings
                    var selectedValues = System.Text.Json.JsonSerializer.Deserialize<List<string>>(WorkOrderItemCreate.eq.JsonTestPointsGroups);

                    if (selectedValues != null)
                    {
                        values = selectedValues;
                        //Console.WriteLine($"Test Point Groups loaded: {WorkOrderItemCreate.eq.JsonTestPointsGroups}");
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error loading test point groups: {ex.Message}");
                // If there's an error, initialize with empty list
                values = new List<string>();
            }
        }

        /// <summary>
        /// Internal method called when values property changes (via setter)
        /// </summary>
        private void OnValuesChangedInternal(IList<string> selectedValues)
        {
            try
            {
                //Console.WriteLine($"OnValuesChangedInternal called with {selectedValues?.Count} values");

                if (selectedValues != null && WorkOrderItemCreate?.eq != null)
                {
                    // Convert the selected values to a JSON string
                    var jsonString = System.Text.Json.JsonSerializer.Serialize(selectedValues);

                    // Save to the JsonTestPointsGroups property
                    WorkOrderItemCreate.eq.JsonTestPointsGroups = jsonString;

                    //Console.WriteLine($"Test Point Groups saved: {jsonString}");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error saving test point groups: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the change event when test point groups are selected/deselected
        /// Saves the selected values as JSON in WorkOrderItemCreate.eq.JsonTestPointsGroups
        /// </summary>
        public void OnValuesChanged(object value)
        {
            try
            {
                //Console.WriteLine($"OnValuesChanged called - value type: {value?.GetType()}, value: {value}");

                // Handle different possible types that Radzen might pass
                IList<string> selectedValues = null;

                if (value is IList<string> stringList)
                {
                    selectedValues = stringList;
                    //Console.WriteLine("Value is IList<string>");
                }
                else if (value is string[] stringArray)
                {
                    selectedValues = stringArray.ToList();
                    //Console.WriteLine("Value is string[]");
                }
                else if (value is List<string> list)
                {
                    selectedValues = list;
                    //Console.WriteLine("Value is List<string>");
                }
                else if (value is System.Collections.IEnumerable enumerable && !(value is string))
                {
                    //Console.WriteLine("Value is IEnumerable, converting to List<string>");
                    var convertedList = new List<string>();
                    foreach (var item in enumerable)
                    {
                        if (item != null)
                        {
                            convertedList.Add(item.ToString());
                        }
                    }
                    selectedValues = convertedList;
                    //Console.WriteLine($"Converted IEnumerable to List<string> with {convertedList.Count} items: [{string.Join(", ", convertedList)}]");
                }
                else if (value != null)
                {
                    //Console.WriteLine($"Unexpected value type: {value.GetType()}");
                    // Try to convert to string and split if it's a comma-separated string
                    var stringValue = value.ToString();
                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        selectedValues = stringValue.Split(',').Select(s => s.Trim()).ToList();
                        //Console.WriteLine($"Converted string to list: {string.Join(", ", selectedValues)}");
                    }
                }

                if (selectedValues != null)
                {
                    // This will trigger the setter and call OnValuesChangedInternal
                    values = selectedValues;
                }

                StateHasChanged();

            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error in OnValuesChanged: {ex.Message}");
                //Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

    }
}

using Blazor.IndexedDB.Framework;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared;

using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Configuration;
using Helpers.Controls.ValueObjects;
using Reports.Domain.ReportViewModels;
using System.Security.Cryptography.X509Certificates;
using Helpers;


using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers.Models;
using Blazed.Controls.MultiComponent;
using FormatValidator;
using System.IO;
using CalibrationSaaS.Infraestructure.Blazor.GenericMethods;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using System.Dynamic;
using System.Reflection;

using System.Text.Json;
using static SQLite.SQLite3;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using BlazorApp1.Blazor.Blazor.GenericMethods;
using WeightSetComponent = BlazorApp1.Blazor.Blazor.GenericMethods.WeightSetComponent;
using BlazorApp1.Pages.Order;
using BlazorApp1.Blazor.Blazor.Pages.Basics;
using Helpers.Controls;
using BlazorApp1.Client.Security;

namespace BlazorApp1.Blazor.Shared
{
    public partial  class GenericTestComponent<CalibrationItem, CalibrationItemResult,DomainEntity>  
          : CalibrationSaaS.Infraestructure.Blazor.GridResults2ComponentBase2<DomainEntity, CalibrationItem> 
         where CalibrationItem : class, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>, IResultComp, IResultTesPointGroup, new() 
        where CalibrationItemResult :class, IResult2, IResultComp, IResultGen2,IDynamic, IUpdated,ISelect, IResultTesPointGroup, new() 
        where DomainEntity :class, 
        IGenericCalibrationCollection<GenericCalibrationResult2>, 
        IResolution, ITolerance, IUncetainty, new()
    {

       

        public bool ShowGrid { get; set; }

        [Parameter]
        public Func<int, Task> ChangePage { get; set; }


        //[Parameter]
        public CalibrationSubTypeView CalibrationSubTypeView { get; set; }



        [Parameter]
        public string ID { get; set; }


        [Parameter]
        public string CalibrationSubType { get; set; }

        public Blazed.Controls.MultiComponent.MultipleComponent<CalibrationItemResult> MultipleComponent { get; set; }


        //[Parameter]
        //public ICreateItems _strCreate { get; set; }

        //[Parameter]
        //public IGetItems _strGet { get; set; }


        //[Parameter]
        //public ISelectItems SelectStandards { get; set; }



        [Parameter]
        public bool ShowEnviroment { get; set; }

        public Pages.Order.EnviromentComponent EnviromentComponent { get; set; }

        [Inject] 

        Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>> _poeServices { get; set; }



        //[CascadingParameter(Name = "CascadeParam1")]
        //public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

        [CascadingParameter(Name = "CascadeParam2")]
        public ICalibrationType eq { get; set; }




        /// <summary>
        /// YPPP
        /// </summary>
      
        [CascadingParameter]
        public  IModalService ModalService { get; set; }

        [Inject]
        public IModalHandler ModalHandler { get; set; }

        [Inject]
        public IDynamicModalHandler DynamicModalHandler { get; set; }

        [Inject]
        public Radzen.DialogService DialogService { get; set; }

        private Tolerance tolerance = new Tolerance();

        [Parameter]
        public string jsonTolerancePerTestPoint { get; set; }

        //YP
        [Parameter]
        public string? GroupName { get; set; }


        /// End <summary>


        [Parameter]
        public int IsCompresion { get; set; } = 0;

        //[Inject]
        //public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public bool RefreshResolution { get; set; }

        public ElementReference InputText { get; set; }

        //[Inject]
        //public IIndexedDbFactory DbFactory { get; set; }

        [Parameter]
        public bool Enabled { get; set; } = false;


         [Parameter]
        public string IdGroup { get; set; }


        public string Description { get; set; } = "";

       


        //[Inject]
        //public Func<IIndexedDbFactory, Application.Services.IWorkOrderDetailServices<CallContext>> Service { get; set; }
        [Inject] Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
        public string url { get; set; }


        

        [Parameter]
        public ResponsiveTable<CalibrationItemResult> RT { get; set; } = new ResponsiveTable<CalibrationItemResult>();

        public string SortField { get; set; }

        public bool HasDescendants { get; set; }

        public int MyProperty { get; set; }

        [Parameter]
        public CalibrationType cmcValues { get; set; }

        public CalibrationItemResult RowAfterRender(CalibrationItemResult lin)
        {

            var srt = RT.Items.Where(x => x.SequenceID == lin.SequenceID).ToList();
            if (srt.Count > 1)
            {


                lin.SequenceID = RT.Items.MaxBy(x => x.SequenceID).SequenceID + 1;
                lin.SequenceID = lin.SequenceID;
                int cont1 = 0;
                foreach (var itemn in RT.Items)
                {
                    
                    itemn.Position = cont1;
                    cont1++;
                }


            }

            
            //}

            IniCalculated = 1;
           

            return lin;

        }


        // Copy values from "As Found" field to "As Left" field based on formula
        public async Task<bool> SetCopyAsFoundToAsLeft()
        {
            var testPoints = LIST.ToList();

            var property = CalSubType.DynamicPropertiesSchema.Where(x => x.GridLocation == "new" && GroupName == GroupName && x.ViewPropertyBase.ControlType == "execute").FirstOrDefault();
            var formula = property.Formula;
            string toField = null;
            string fromField = null;

            property.ViewPropertyBase.copyAsFoundToAsleft = true;

            if (!string.IsNullOrEmpty(formula))
            {

                formula = formula.Trim().TrimStart('{').TrimEnd('}').Trim();


                var assignments = formula.Split(',');

                foreach (var assignment in assignments)
                {
                    if (string.IsNullOrWhiteSpace(assignment))
                        continue;

                    var parts = assignment.Split('=');
                    if (parts.Length == 2)
                    {

                        toField = parts[0].Trim();
                        fromField = parts[1].Trim();

                        foreach (var itemtest in testPoints)
                        {

                            var objectJson = itemtest.Object;

                            if (objectJson == null)
                            {
                                continue;
                            }

                            try
                            {

                                var jsonDict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(objectJson);

                                if (jsonDict == null)
                                {
                                    continue;
                                }


                                var keyValueArray = jsonDict.Select(kvp => new { Key = kvp.Key, Value = kvp.Value }).ToList();


                                var toFieldEntry = keyValueArray.FirstOrDefault(x => x.Key == toField);
                                var fromFieldEntry = keyValueArray.FirstOrDefault(x => x.Key == fromField);

                                if (toFieldEntry != null && fromFieldEntry != null)
                                {
                                    jsonDict[toField] = fromFieldEntry.Value;

                                    // Update itemtest.Object with the modified JSON
                                    itemtest.Object = Newtonsoft.Json.JsonConvert.SerializeObject(jsonDict);
                                }
                            }
                            catch (Exception ex)
                            {

                                await ExceptionManager(ex);
                            }
                        }

                    }
                }
            }

            // Trigger component re-render to reflect changes
            // Force MultipleComponent.razor to read the new values from GenericCalibrationResult.Object
            foreach (var item in testPoints)
            {
                // Ensure changes are reflected in the LIST collection
                var existingItem =   LIST.FirstOrDefault(x => x.SequenceID == item.SequenceID);
                if (existingItem != null)
                {
                    existingItem.Object = item.Object;
                }
            }

            // Set refresh = true to force MultipleComponent.razor to read the new values
            refresh = true;
            StateHasChanged();

            return true;

        }

        public CalibrationItemResult RowChange(CalibrationItemResult lin)
        {



            if (RT.IsDragAndDrop)
            {
                int posi = 100;

                for (int i = 0; i < RT.ItemList.Count; i++)
                {
                    RT.ItemList[i].Position = posi;
                    posi++;
                    RT.ReplaceItemKey(RT.ItemList[i]);
                }

                return lin;
            }

            if (lin.Position >= 100)
            {
                return lin;
            }

            return lin;



        }



        public string TextMultiplerFound { get; set; } = "1";

        public string TextMultiplerLeft { get; set; } = "1";


        public string TextNewValue { get; set; }

        public bool EnableWeightSet { get; set; } = true;


        public bool EnableSetPoints { get; set; } = true;

        public CalibrationItem _Linearity { get; set; }


        public bool IsAddLinearity { get; set; }

        public async Task TaskAfterSave(CalibrationItemResult item, string sav)
        {
            if (IsNewItem)
            {
                //await Calculate();

            }

            IsNewItem = false;

        }


        public async Task AddNewItem()
        {
            try
            {

                var standards = WorkOrderItemCreate?.TestPointResult?.ToList();
                //bool hasSelectedStandards = false;

                //foreach (var item in standards)
                //{
                //    if (item.GenericCalibration2?.WeightSets?.Count() > 0)
                //    {
                //        hasSelectedStandards = true;
                //        break;
                //    }

                //}
                //// Check if there are selected standards and show confirmation dialog
                //if (hasSelectedStandards)
                //{
                //    bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "If you add a new test point, the selected standards will be removed. Would you like to continue?");
                //    if (!confirmed)
                //    {
                //        return;
                //    }

                //    if (confirmed != true)
                //    {
                //        return; // Exit the method if user cancels
                //    }
                //}


                IsNewItem = true;
                var defaultitem = await _strNew.DefaultNewItem(WorkOrderItemCreate, IsCompresion);
                if (defaultitem != null)
                {

                    foreach (var item in defaultitem)
                    {
                        item.GroupName = GroupName;
                        foreach (var item2 in item.TestPointResult)
                        {
                            item2.GroupName = GroupName;
                            await RT.SaveNewItem(item2 as CalibrationItemResult);

                            //WorkOrderItemCreate.TestPointResult.Add(item2);
                        }
                    }

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);

            }




        }

        public bool IsNewItem { get; set; }
        public CalibrationItemResult DefaultNew()
        {
            CalibrationItemResult lin = new CalibrationItemResult();

            IsAddLinearity = true;

           
            IsNewItem = true;

            return lin;

        }



        public async Task ChangeMultipleControl(ChangeEventArgs arg, dynamic lin, bool AddWeight = false)
        {



            aTimer.Stop();
            // new timer
            aTimer.Start();
            //IniCalculated = 1;
            StateHasChanged();

        }

        //beacause -1 already in use an 0 cause confuse with 0 position
        public int SelectPosition { get; set; } = -11;

        public string SelectValue { get; set; } = "";

        public CalibrationItemResult SelectRow { get; set; } 

        public void RefreshGrid(string valor,int position)
        {


            foreach(var item in tableListResult)
            {
                item.KeyObject = null; 
            }

            SelectRow = tableListResult.Where(x => x.Position == position).FirstOrDefault();

            SelectValue =valor;
            SelectPosition = position;

            SelectRow.KeyObject = valor;




            StateHasChanged();



        }




        public async Task ChangeControl2(ChangeEventArgs arg, CalibrationItemResult lin, bool AddWeight = false)
        {
            try
            {
                if (arg == null || arg.Value.ToString() == "")
                {
                    return;
                }


                var result = Convert.ToDouble(arg.Value);

                //if (AddWeight)
                //{
                //    AddWeightset(lin, result);
                //}

                // remove previous one
                aTimer.Stop();
                // new timer
                aTimer.Start();
                //IniCalculated = 1;
                //if (IniCalculated == 2)
                //{

                    //await Calculate();


                    //IniCalculated = 1;
                //}





            }
            catch
            {

            }



        }

        public int IniCalculated { get; set; }
        private static System.Timers.Timer aTimer;

        


      

       


      

        [Parameter]
        public bool Accredited { get; set; }

        public bool IsWeightLoaded { get; set; }

       

       

        


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
     

        List<WeightSet> Selected = new List<WeightSet>();



        public CalibrationItem Linearity = new CalibrationItem();

        public CalibrationItemResult LinearityResult = new CalibrationItemResult();

        [Inject]
        public CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState AppState { get; set; }

        [Parameter]
        public CalibrationSaaS.Domain.Aggregates.Entities.Status CurrentStatus { get; set; } = new CalibrationSaaS.Domain.Aggregates.Entities.Status();

        public EditContext editContext { get; set; }

        public List<CalibrationItemResult> LIST { get; set; } = new List<CalibrationItemResult>();


        public CalibrationItemResult NewItem { get; set; } 



        public WeightSetComponent WeightSetComponent { get; set; } //= new WeightSetComponent();

        PieceOfEquipment poq = new PieceOfEquipment();

        [Parameter]
        public List<WeightSet> WeightSetList2 { get; set; } = new List<WeightSet>();


        public bool IsNew { get; set; }


        public bool FirstRender { get; set; }


        //WorkOrderDetail wod = new WorkOrderDetail();

        protected override async Task OnParametersSetAsync()
        {
          

        }


        int WeightSetListCount = 0;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
           

            var a = Enabled;

            try
            {

                if (firstRender)
                {
                    await IniComponent();

                    ShowGrid = true;

                }

                if (firstRender && WorkOrderItemCreate != null )
                {
                    
                }
              

                if (firstRender && RT != null)
                {
                    RT.ShowControl = true;
                    RT.ShowLabel = false;
                }


                if (WeightSetComponent == null || WeightSetComponent?.lstWeightSet == null || WeightSetComponent?.lstWeightSet?.Count == 0)
                {

                    EnableWeightSet = false;

                }
                else
                {
                    EnableWeightSet = true;
                }



                FirstRender = firstRender;

                if (WeightSetList2 == null)
                {
                    WeightSetList2 = new List<WeightSet>();
                }


            }

            catch (Exception ex)

            {
                await ExceptionManager(ex);
            }
            finally
            {
                await base.OnAfterRenderAsync(firstRender);
            }

        }



        public List<CalibrationItem> tableList = null;

        public List<CalibrationItemResult> tableListResult = null;

        public CalibrationSubType CalSubType { get; set; }


        public string PropertyMax { get; set; }

        public double GetMaxValue()
        {

            if(!string.IsNullOrEmpty(PropertyMax) && WorkOrderItemCreate?.TestPointResult != null 
                && WorkOrderItemCreate?.TestPointResult?.Count > 0)
            {
                List<double> values = new List<double>();   

                foreach (var item in WorkOrderItemCreate?.TestPointResult)
                {
                    dynamic Model = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(item.Object);

                    double value = 0;

                    if (Model != null && Model[PropertyMax] != null) {

                         value = (double)Model[PropertyMax];

                    }

                    values.Add(value);
                }

                return values.Max();
            }

            return 0;
        
        
        }

            public Tolerance SetTolerance()
        {
            
            if (WorkOrderItemCreate.Tolerance.ToleranceTypeID.HasValue)
            {

                tolerance = new Tolerance();

                tolerance = WorkOrderItemCreate.Tolerance;
                tolerance.DecimalNumber = WorkOrderItemCreate.DecimalNumber;
                tolerance.Resolution = WorkOrderItemCreate.Resolution;

                tolerance.MaxValue = GetMaxValue();

            }

            return tolerance;   
        }

        public Tolerance SetTolerancePerTestPoint(string jsonTolerancePerTestPoint)
        {

            
            var tol = new Tolerance();

            tol = Newtonsoft.Json.JsonConvert.DeserializeObject<Tolerance>(jsonTolerancePerTestPoint);
            return tol;
        }
        public CreateModel param { get; set; }

        bool HasNewItem = false;
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();

            if (ModalHandler != null)
            {
                ModalHandler.OnShowModal = ShowModal;
            }
            if (DynamicModalHandler != null)
            {
                DynamicModalHandler.OnShowModal = ShowModal;
            }

        }

        [Parameter]
        public Func<string,Task> DataLoad { get; set; }


        public async Task IniComponent()
        {
            try
           {
               


                if (string.IsNullOrEmpty(CalibrationSubType))
                {
                    return;
                }

                //await ShowProgress();

                HideElement = CalibrationSubType + "-modal";

                if (!string.IsNullOrEmpty(CalibrationSubType))
                {
                    IsCompresion = Convert.ToInt32(CalibrationSubType);
                }

                url = Configuration.GetSection("Reports")["URL"];

                Linearity.TestPoint = new TestPoint();

                editContext = new EditContext(Linearity);

                aTimer = new System.Timers.Timer(500);

                aTimer.AutoReset = false;

                aTimer.Elapsed += OnUserFinish;

                //await base.OnInitializedAsync();

                //ValidateGrid = LTILogic.ValidateGenericlList;
                var CalSubType2 = eq.CalibrationSubTypes.Where(x => x.CalibrationSubTypeId == Convert.ToInt32(CalibrationSubType)).FirstOrDefault();

                if (CalSubType2 == null)
                {
                    return;
                }

                CalSubType = CalSubType2.GetStyle(CalSubType2,eq.CalibrationSubTypes.Count);

                CalibrationSubTypeView = CalSubType.CalibrationSubTypeView;
         

                 if (!string.IsNullOrEmpty(CalSubType.CreateClass))//(iso == 14)
                {
                    if(param == null)
                    {
                        param = CalSubType.CreateModel();
                    }

                    
                    if (CalSubType.DynamicPropertiesSchema != null)
                    {
                        var dyn = CalSubType.DynamicPropertiesSchema.Where(x => x.Enable == true && x.GridLocation=="row").OrderBy(x => x.ColPosition).ToList();

                        //ICollection<Dictionary<string, object>> colecc = new List<Dictionary<string, object>>();
                       

                        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                        for (int yi = 0; yi < dyn.Count; yi++)
                        {

                            keyValuePairs.Add(dyn[yi].Name, (object)dyn[yi].DefaultValue);

                            if (!string.IsNullOrEmpty(dyn[yi]?.ViewPropertyBase?.SelectOptions) && dyn[yi]?.ViewPropertyBase?.SelectOptions?.ToLower() == "GetUoMByCalibrationType".ToLower())
                            {
                                //var adf = typeof(AppState).GetMethod("add").Invoke(null, new[] { arg1, arg2 });
                                //var adf = typeof(AppState).GetMethod("add").Invoke(null, new[] );

                                MethodInfo addMethod = typeof(AppState).GetType().GetMethod(dyn[yi]?.ViewPropertyBase?.SelectOptions);

                                if (CalSubType.UnitOfMeasureTypeID.HasValue)
                                {
                                    object result = AppState.GetUoMByTypeDIC(CalSubType.UnitOfMeasureTypeID.Value);//addMethod.Invoke(this, new object[] { CalSubType.CalibrationTypeId });

                                    var optio = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                                    dyn[yi].ViewPropertyBase.SelectOptions = optio;
                                }
                                else
                                {
                                    var optio = Newtonsoft.Json.JsonConvert.SerializeObject(AppState.UnitofMeasureList);

                                    dyn[yi].ViewPropertyBase.SelectOptions = optio;
                                }
                                


                            }
                            else if (!string.IsNullOrEmpty(dyn[yi]?.ViewPropertyBase?.SelectOptions) && dyn[yi]?.ViewPropertyBase?.SelectOptions?.ToLower() == "GetAllUoMDic".ToLower())
                            {
                                //MethodInfo addMethod = typeof(AppState).GetType().GetMethod(dyn[yi]?.ViewPropertyBase?.SelectOptions);
                                object result = AppState.GetAllUoMDic();// addMethod.Invoke(this, new object[] {  });

                                var optio = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                                dyn[yi].ViewPropertyBase.SelectOptions = optio;

                            }
                            else if (!string.IsNullOrEmpty(dyn[yi]?.ViewPropertyBase?.SelectOptions)
                                &&( dyn[yi]?.ViewPropertyBase?.SelectOptions?.ToLower().Contains("filter") ==true
                                || dyn[yi]?.ViewPropertyBase?.SelectOptions?.ToLower().Contains("method") == true)
                                //&& !string.IsNullOrEmpty(dyn[yi]?.ViewPropertyBase?.OptionsFilter)
                                )
                            {

                                //var result =  AppState.GetAllUoMHierarchicalDic();// addMethod.Invoke(this, new object[] {  });
                                var sf=Newtonsoft.Json.JsonConvert.DeserializeObject<SelectFilter>(dyn[yi]?.ViewPropertyBase?.SelectOptions);

                                //var result = CreateGroupedDictionaryFromAppState(
                                //    appStateMethodName: "GetAllUnitOfMeasure",
                                //    keyPropertyName: "UnitOfMeasureID",
                                //    valuePropertyName: "Name",
                                //    groupingPropertyName: "TypeID"
                                //);
                                var result = CreateGroupedDictionaryFromAppState(
                                   appStateMethodName: sf.Method,
                                   keyPropertyName: sf?.Key, //"UnitOfMeasureID",
                                   valuePropertyName:sf?.Value,// "Name",
                                   groupingPropertyName: sf?.Group //"TypeID"
                               );

                                if(sf.Filter != null && sf.Filter.Count() > 0)
                                {
                                    dyn[yi].ViewPropertyBase.OptionsFilter = sf.Filter[0];
                                }


                                var optio = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                                dyn[yi].ViewPropertyBase.SelectOptions = optio;

                            }
                            else if(!string.IsNullOrEmpty(dyn[yi]?.ViewPropertyBase?.SelectOptions) && dyn[yi]?.ViewPropertyBase?.SelectOptions?.ToLower()?.Contains("appstate")==true)
                            {

                                MethodInfo addMethod = typeof(AppState).GetType().GetMethod(dyn[yi]?.ViewPropertyBase?.SelectOptions);
                                
                                object result = addMethod.Invoke(this, new object[] { });

                                var optio = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                                dyn[yi].ViewPropertyBase.SelectOptions = optio;
                            }


                        }

                        param.DefaultData = keyValuePairs;

                         dyn = CalSubType.DynamicPropertiesSchema.Where(x => x.Enable == true && x.GridLocation == "new").OrderBy(x => x.ColPosition).ToList();

                        //ICollection<Dictionary<string, object>> colecc = new List<Dictionary<string, object>>();


                        Dictionary<string, object> keyValuePairs2 = new Dictionary<string, object>();
                        for (int yi = 0; yi < dyn.Count; yi++)
                        {

                            keyValuePairs2.Add(dyn[yi].Name, (object)dyn[yi].DefaultValue);

                            if (!string.IsNullOrEmpty(dyn[yi]?.ViewPropertyBase?.SelectOptions) && dyn[yi]?.ViewPropertyBase?.SelectOptions?.ToLower() == "GetUoMByCalibrationType".ToLower())
                            {
                                //var adf = typeof(AppState).GetMethod("add").Invoke(null, new[] { arg1, arg2 });
                                //var adf = typeof(AppState).GetMethod("add").Invoke(null, new[] );

                                MethodInfo addMethod = typeof(AppState).GetType().GetMethod(dyn[yi]?.ViewPropertyBase?.SelectOptions);
                                object result = addMethod.Invoke(this, new object[] { CalSubType.CalibrationTypeId });

                                var optio = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                                dyn[yi].ViewPropertyBase.SelectOptions = optio;


                            }
                            else if (!string.IsNullOrEmpty(dyn[yi]?.ViewPropertyBase?.SelectOptions) && dyn[yi]?.ViewPropertyBase?.SelectOptions?.ToLower() == "GetAllUoMDic".ToLower())
                            {
                                MethodInfo addMethod = typeof(AppState).GetType().GetMethod(dyn[yi]?.ViewPropertyBase?.SelectOptions);
                                object result = addMethod.Invoke(this, new object[] { });

                                var optio = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                                dyn[yi].ViewPropertyBase.SelectOptions = optio;

                            }
                            else if(!string.IsNullOrEmpty(dyn[yi]?.ViewPropertyBase?.SelectOptions) && dyn[yi]?.ViewPropertyBase?.SelectOptions?.ToLower()?.Contains("appstate") == true)
                            {

                                MethodInfo addMethod = typeof(AppState).GetType().GetMethod(dyn[yi]?.ViewPropertyBase?.SelectOptions);

                                object result = addMethod.Invoke(this, new object[] { });

                                var optio = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                                dyn[yi].ViewPropertyBase.SelectOptions = optio;
                            }


                        }
                        if(param?.Header != null)
                        {
                            param.Header.DefaultData = keyValuePairs2;
                        }
                        


                    }
                    else
                    {
                        ConfigMessage += "Fields Not Configured" + Environment.NewLine;
                    }

                    

                   


                    ErrorMessage += param.ErrorMessage;
                    string pathclass = param.ClassName;
                    var inst = Helpers.DynamicHelper.GetInstance(pathclass); //Helpers.DynamicHelper.GetInstance(itemcs.CreateClass);

                    if(inst == null)
                    {
                        throw new Exception("Create Class bad configured");
                    }

                    _strCreate = inst; // (CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.ICreateItems<DomainEntity, CalibrationItem>)inst; // new CalibrationSaaS.Infraestructure.Blazor.LTI.Micro.CreateItemsMicro();

                    if(_strCreate != null)
                    {
                        _strCreate.Parameter = param;
                        _strCreate.AppState = AppState;
                        
                    }
                    
                }
                else
                {
                    ErrorMessage += "Create path Not Defined";
                }

                if (!string.IsNullOrEmpty(CalSubType.GetClass))//(iso == 14)
                {

                    CreateModel paramGet = CalSubType.GetModel();
                    ErrorMessage += paramGet.ErrorMessage;
                    string pathclass = paramGet.ClassName;
                    
                    var ins = Helpers.DynamicHelper.GetInstance(pathclass);


                    if(ins == null)
                    {
                        throw new Exception("Get Class bad configured");
                    }

                    _strGet = (CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.IGetItems<DomainEntity, CalibrationItem>)ins;//new CalibrationSaaS.Infraestructure.Blazor.LTI.Micro.GetItemsMicro();

                    

                }
                else
                {
                    ErrorMessage += "Get path Not Defined";
                }


                if (!string.IsNullOrEmpty(CalSubType.SelectStandarClass))//(iso == 14)
                {

                    CreateModel paramSelect = CalSubType.SelectModel();

                    ErrorMessage += paramSelect.ErrorMessage;
                    string pathclass = paramSelect.ClassName;

                    var ins = Helpers.DynamicHelper.GetInstance(pathclass);
                    SelectStandards = (CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.ISelectItems<DomainEntity>)ins;//new CalibrationSaaS.Infraestructure.Blazor.LTI.Micro.GetItemsMicro();

                    SelectStandards.Parameter = param;
                }

              
                if (!string.IsNullOrEmpty(CalSubType.Select2StandarClass))//(iso == 14)
                {
                    CreateModel paramSelect2 = CalSubType.Select2Model();

                    ErrorMessage += paramSelect2.ErrorMessage;
                    string pathclass = paramSelect2.ClassName;


                    var ins = Helpers.DynamicHelper.GetInstance(pathclass);
                    Select2Standards = (CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.ISelectItems<DomainEntity>)ins;//new CalibrationSaaS.Infraestructure.Blazor.LTI.Micro.GetItemsMicro();


                }


                if (!string.IsNullOrEmpty(CalSubType.NewClass))//(iso == 14)
                {  CreateModel paramNew = CalSubType.NewModel();
                     paramNew.DefaultData = param.DefaultData;
                    paramNew.Data = param.Data;

                    ErrorMessage += paramNew.ErrorMessage;
                    string pathclass = paramNew.ClassName;

                    param.ClassName = pathclass;

                    var ins = Helpers.DynamicHelper.GetInstance(pathclass);

                  

                    _strNew = (CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.INewItem<DomainEntity, CalibrationItem>)ins;//new CalibrationSaaS.Infraestructure.Blazor.LTI.Micro.GetItemsMicro();


                    if (_strNew != null)
                    {
                        _strNew.Parameter = param;
                       // _strNew.AppState = AppState;

                    }
                    
                }






                if (CalSubType?.DynamicPropertiesSchema?.Where(x => x.GridLocation == "new").FirstOrDefault() != null)
                {
                    //NewItem = new CalibrationItemResult();

                    //NewItem.Component = Component.Name;                    

                    //NewItem.Position = -1;

                    HasNewItem = true;
                }


                if (WorkOrderItemCreate.Tolerance.FullScale.HasValue && WorkOrderItemCreate.Tolerance.FullScale.Value)
                {
                    var field = CalSubType?.DynamicPropertiesSchema?.Where(x => x.IsMaxField.HasValue && x.IsMaxField == true).FirstOrDefault();

                    if (field != null)
                    {
                        PropertyMax = field.Name;

                    }
                    else
                    {
                        await ShowError("IsMaxField Not Configured Error in Calibration Subtype: " + IsCompresion);
                    }

                    //var a = WorkOrderItemCreate.TestPointResult[0].FirstOrDefault();

                    //var b = a.Object;

                    //var des= 


                }


                List<CalibrationItem> getreturn = null;

                
                var HasData = false;

                if (_strGet != null)
                {
                    var inheritCalSubType = CalSubType.inheritCalibrationSubTypeId;
                    var temgetreturn = await _strGet.GetItems(WorkOrderItemCreate, IsCompresion, inheritCalSubType);
                    
                   

                    if (temgetreturn != null)
                    {
                        getreturn = temgetreturn.Where(x => x.TestPointResult.Any(x => x.GroupName == GroupName && x.CalibrationSubTypeId== IsCompresion)).ToList(); //TODO add filter by GroupName
                    }
                    if (getreturn != null && getreturn.Count > 0)
                    {
                        HasData = true;
                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(CalSubType.GetClass))
                    {
                        NoDataMessage = "Get Class  Bad Configured " + CalSubType.GetClass;
                    }
                    else
                    {
                        NoDataMessage = "Get Class Not Configured";
                    }

                    

                    return;

                }
                if (HasData)
                {

                    tableList = getreturn;//_strGet.GetItems(WorkOrderItemCreate.eq, IsCompresion);


                }
                else

                if (!HasData)
                {
                    //!ValidateGrid(WorkOrderItemCreate.eq, IsCompresion)
                    if (_strCreate != null)
                    {

                        if (_poeServices != null)
                        {
                            _strCreate.PoeServices = _poeServices;                           
                            
                        }
                        _strCreate.AppState = AppState;

                        _strCreate.Parameter = param;

                        tableList = await _strCreate.CreateItems(WorkOrderItemCreate, IsCompresion, GroupName);

                        //if(tableListtmp != null)
                        //{                           

                        //    tableList = tableListtmp.Where(x=>x.GroupName== GroupName).ToList();
                        //}


                         

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(CalSubType.CreateClass))
                        {
                            NoDataMessage = "Create Class Bad Configured " + CalSubType.CreateClass;
                        }
                        else
                        {
                            NoDataMessage = "Create Class Not  Configured";
                        }

                            

                        return;
                    }

                }

                

                if (tableList != null && tableList.Count > 0 && tableList[0]?.TestPointResult?.Count == 1 && !CalibrationSubTypeView.IsCollection)
                {
                    RT.Clear();

                    SortField = RT.GetPropertyName(() => LinearityResult.Position);
                    var list34 = new List<CalibrationItemResult>();
                    for (int iu = 0; iu < tableList.Count; iu++)
                    {
                        if (CalSubType.CalibrationSubTypeView.UseResult.HasValue && CalSubType.CalibrationSubTypeView.UseResult==true)
                        {
                            tableList[0].TestPointResult[0].GenericCalibration2 = tableList[0] as GenericCalibration2;
                        }

                        if (tableList[0].TestPointResult[0].Resolution == 0)
                        {
                            tableList[0].TestPointResult[0].Resolution = WorkOrderItemCreate.Resolution;
                            tableList[0].TestPointResult[0].DecimalNumber = WorkOrderItemCreate.DecimalNumber;
                           
                        }
                        tableList[iu].GroupName = GroupName;
                        tableList[iu].Component = Component.Name;
                        tableList[iu].TestPointResult[0].Component = Component.Name;
                        var iu2 = tableList[iu].TestPointResult[0] as CalibrationItemResult;
                        iu2.GroupName = GroupName;
                        if (iu2.Position != -1 )
                        {
                            list34.Add(iu2);
                        }
                        else if(iu2.GroupName == GroupName)
                        {


                            NewItem = iu2;
                        }
                        //RT.SaveNewItem(iu2, "asc", SortField);


                    }

                    if (list34.Any())
                    {
                        foreach(var item in list34)
                        {
                            if (!string.IsNullOrEmpty(item.KeyObject))
                            {
                                SelectValue = item.KeyObject;
                            }
                        }
                    }   




                    list34 = list34.OrderBy(x => x.Position).ToList();
                    LIST = list34;
                    RT.ItemList = list34;
                    RT.ItemsDataSource = list34;

                    tableListResult = list34;

                    if (!IsWeightLoaded)
                    {
                        //aTimer.Stop();
                        //aTimer.Start();
                        //IniCalculated = 2;
                        //await Calculate();
                    }
                }
                else
                if (CalibrationSubTypeView.IsCollection && tableList != null && tableList.Count > 0 && tableList[0]?.TestPointResult?.Count > 0)
                {
                    RT.Clear();

                    SortField = RT.GetPropertyName(() => LinearityResult.Position);

                    var list34 = new List<CalibrationItemResult>();
                    foreach (var itemm2 in tableList)
                    {
                        itemm2.Component = Component.Name;

                        // Set GroupName on the calibration item if it implements IResultComp

                        itemm2.GroupName = GroupName;
                        
                        foreach (var itemm in itemm2.TestPointResult)
                        {
                            if (itemm.Resolution == 0)
                            {
                                itemm.Resolution = WorkOrderItemCreate.Resolution;
                                itemm.DecimalNumber = WorkOrderItemCreate.DecimalNumber;
                            }

                            if (CalSubType.CalibrationSubTypeView.UseResult.HasValue && CalSubType.CalibrationSubTypeView.UseResult==true)
                            {
                                itemm.GenericCalibration2 = itemm2 as GenericCalibration2;
                            }

                            itemm.Component = Component.Name;

                            // Set GroupName on the result item if it implements IResultComp

                            itemm.GroupName = GroupName;
                            
                            var iu = itemm as CalibrationItemResult;
                            if (iu.Position != -1)
                            {
                                
                                list34.Add(iu);
                            }
                            else if( iu.GroupName == GroupName)
                            {
                                NewItem = iu;
                            }
                            //RT.SaveNewItem(iu, "asc", SortField);
                            //list34.Add(iu);
                        }

                    }

                    if (list34.Any())
                    {
                        foreach (var item in list34)
                        {
                            if (!string.IsNullOrEmpty(item.KeyObject))
                            {
                                SelectValue = item.KeyObject;
                            }
                        }
                    }

                    list34 = list34.OrderBy(x => x.Position).ToList();
                    LIST = list34;
                    RT.ItemList = list34;
                    RT.ItemsDataSource = list34;
                    tableListResult = list34;


                    if (!IsWeightLoaded)
                    {
                        //aTimer.Stop();
                        //aTimer.Start();
                        //IniCalculated = 2;
                        //await Calculate();
                    }
                }

                else if (CalibrationSubTypeView != null && WorkOrderItemCreate?.TestPointResult?.Count==0)
                {
                    NoDataMessage = CalibrationSubTypeView.NoDataMessage;

                }

                if ((tableList == null || tableList?.Count == 0) && (!CalSubType.SupportCSV.HasValue || !CalSubType.SupportCSV.Value) && !IsNewItem)
                {
                    

                    if (!string.IsNullOrEmpty(CalSubType.SelectStandarClass) && WorkOrderItemCreate?.TestPointResult?.Count == 0)
                    {
                        ConfigMessage += CalSubType.CreateClass + " " + Environment.NewLine + CalSubType.GetClass + " " + Environment.NewLine;
                        ConfigMessage = ConfigMessage + CalSubType.SelectStandarClass;
                    }
                }
                else if (tableList != null && tableList.Count >= 0)
                {
                    
                }

                if (CalSubType?.CalibrationSubTypeView?.EnabledSelect == true && SelectStandards == null && !IsNewItem)
                {
                    ConfigMessage += "Select Class not configured";
                }
                if (CalSubType?.CalibrationSubTypeView?.EnableSelect2 == true && Select2Standards == null && !IsNewItem)
                {
                    ConfigMessage += "Select2 Class not configured";
                }
                if (CalSubType?.CalibrationSubTypeView?.EnabledNew == true && _strNew == null && CalSubType?.CalibrationSubTypeView?.HasNewButton == true  && !IsNewItem)
                {
                    ConfigMessage += "New Class not configured";
                }



                
                if (!HasData && tableList != null && tableList.Count > 0 && DataLoad != null)
                {
                  //DataLoad(Id).ConfigureAwait(true).GetAwaiter().GetResult();
                }
                


                //UoM
                //
                //LIST = RT.Items;

            }
            catch (Exception ex)
            {

                await ExceptionManager(ex, "Error in calibrationSubtype: " + IdGroup);

            }
            finally
            {
                IsNewItem = false;
                
                StateHasChanged();
                //await CloseProgress();
            }
        }


        public string NoDataMessage { get; set; }

        public string ConfigMessage { get; set; }


        
        private void OnUserFinish(Object source, ElapsedEventArgs e)
        {

            
                
                aTimer.Stop();

                List<CalibrationItem> a = new List<CalibrationItem>();


                InvokeAsync(async () =>
           {

               await ExecuteFormula();

           });

        }


        //[Parameter]
        //public IExecuteMethod IExecuteMethod  { get; set; }


        public async Task ExecuteFormula()
        {
            //WorkOrderItemCreate.BasicCalibrationResult = (RT.Items as List<GenericCalibrationResult2>);

        }

        



        [Parameter]
        public string HideElement { get; set; } =  "modalLinearity";

        [CascadingParameter] public IModalService Modal { get; set; }
        public string _message { get; set; }
        //Logger.LogDebug(Group.Name);

        public bool refresh = false;
        public async Task SelectWeight(CalibrationItem itemRow, int position)
        {
            try
            {
                refresh = true;
                SelectStandards.PoeServices = _poeServices;

                if(SelectStandards == null)
                {
                    throw new Exception("Select Standard not implemented");
                }

                var selec = await SelectStandards.SelectItems(WorkOrderItemCreate, IsCompresion, itemRow, position);


                var parameters = new ModalParameters();


                //WeightSetList2 = selec.SetList;

                parameters.Add("target", itemRow);

                parameters.Add("DataSource", selec.SetList);

                parameters.Add("TestPoint", itemRow.TestPoint);

                parameters.Add("SelectWeight", selec.SelecList);

                ModalOptions op = new ModalOptions();

                op.Position = ModalPosition.Center;

                op.Class = "blazored-modal " + ModalSize.MediumWindow;  //blazored-modal blazored-modal-scrollable allWindow

                op.DisableBackgroundCancel= true;
                

                if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
                {
                    await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
                }


                IModalReference messageForm=null;

                if (!string.IsNullOrEmpty(CalSubType.StandardAssignComponent))
                {
                    switch (CalSubType.StandardAssignComponent)
                    {
                        case "weightset":

                            messageForm = Modal.Show(typeof(BlazorApp1.Blazor.Blazor.GenericMethods.SelectWeightComponent<GenericCalibration2>), "Select Weight Kit ", parameters, op);
                            break;


                        case "standard":

                            messageForm = Modal.Show(typeof(BlazorApp1.Blazor.Blazor.GenericMethods.SelectStandardComponent<GenericCalibration2>), "Select Weight Kit ", parameters, op);
                            break;


                        case "LTI":
                            messageForm = Modal.Show(typeof(Blazor.LTI.WeightComponent<GenericCalibration2>), "Select ", parameters, op);
                            //CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Target>
                            break;

                        case "rockwell":
                            messageForm = Modal.Show(typeof(Blazor.LTI.Rockwell.WeightComponent<GenericCalibration2>), "Select Standard", parameters, op);
                            //CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Target>
                            break;
                    }
                }
                else
                {
                    messageForm = Modal.Show(typeof(Blazor.LTI.Rockwell.WeightComponent<GenericCalibration2>), "Select ", parameters, op);
                }       


                Console.WriteLine("select 1");
                var result = await messageForm.Result;

                if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
                {
                    await JSRuntime.InvokeVoidAsync("showElement", HideElement);
                }

                if (result.Cancelled)
                {
                    return;
                }

                _message = result.Data?.ToString() ?? string.Empty;

                WeightSetResult r = (WeightSetResult)result.Data;

                List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>> lst1 = new List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>>();

                foreach (var item34 in RT.Items)
                { 
                    if(item34.GenericCalibration2 != null)
                    {
                        lst1.Add(item34.GenericCalibration2);
                    }

                   
                }


                //var lst = RT.Items as List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>>;

                await SelectStandards.StandardTask(WorkOrderItemCreate, r, selec.Result, itemRow, lst1, position);

                if(itemRow?.TestPointResult?.Count > 0)
                {
                    itemRow.TestPointResult[0].Updated = DateTime.Now.Ticks; 
                }
                


                StateHasChanged();

                refresh = false;

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
            }

           

        }


        public async Task SelectWeight2(CalibrationItem itemRow, int position)
        {
            try
            {
                refresh = true;
                Select2Standards.PoeServices = _poeServices;

                if (Select2Standards == null)
                {
                    throw new Exception("Select Standard not implemented");
                }

                var selec = await Select2Standards.SelectItems(WorkOrderItemCreate, IsCompresion, itemRow, position);


                var parameters = new ModalParameters();


                //WeightSetList2 = selec.SetList;

                parameters.Add("target", itemRow);

                parameters.Add("DataSource", selec.SetList);

                parameters.Add("TestPoint", itemRow.TestPoint);

                parameters.Add("SelectWeight", selec.SelecList);

                parameters.Add("AllowZero", true);

                ModalOptions op = new ModalOptions();

                op.Position = ModalPosition.Center;

                op.Class = "blazored-modal " + ModalSize.MediumWindow;  //blazored-modal blazored-modal-scrollable allWindow

                op.DisableBackgroundCancel = true;


                if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
                {
                    await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
                }
                IModalReference messageForm = null;
                if (!string.IsNullOrEmpty(CalSubType.StandardAssignComponent2))
                {
                    switch (CalSubType.StandardAssignComponent2)
                    {
                        case "weightset":

                            messageForm = Modal.Show(typeof(Blazor.LTI.Scale.WeightComponent<GenericCalibration2>), "Select Weight", parameters, op);
                            break;


                        case "LTI":
                            messageForm = Modal.Show(typeof(Blazor.LTI.WeightComponent<GenericCalibration2>), "Select ", parameters, op);
                            //CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Target>
                            break;

                        case "rockwell":
                            messageForm = Modal.Show(typeof(Blazor.LTI.Rockwell.WeightComponent<GenericCalibration2>), "Select Standard", parameters, op);
                            //CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Target>
                            break;
                    }
                }
                else
                {
                    messageForm = Modal.Show(typeof(Blazor.LTI.Rockwell.WeightComponent<GenericCalibration>), "Select ", parameters, op);
                }



                //var messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.WeightComponent<GenericCalibration>), "Select ", parameters, op);

                Console.WriteLine("select 1");
                var result = await messageForm.Result;

                if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
                {
                    await JSRuntime.InvokeVoidAsync("showElement", HideElement);
                }

                if (result.Cancelled)
                {
                    return;
                }

                _message = result.Data?.ToString() ?? string.Empty;

                WeightSetResult r = (WeightSetResult)result.Data;

                //var lst = RT.Items as List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>>;
                List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>> lst1 = new List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>>();

                foreach (var item34 in RT.Items)
                {
                    if (item34.GenericCalibration2 != null)
                    {
                        lst1.Add(item34.GenericCalibration2);
                    }


                }

                await Select2Standards.StandardTask(WorkOrderItemCreate, r, selec.Result, itemRow, lst1, position);

                StateHasChanged();

                refresh = false;

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
            }



        }

        public async Task LoadCSV(CalibrationSubType dynamicProperty)
        {
            try
            {
                var parameters = new ModalParameters();
                parameters.Add("CalSubType", dynamicProperty);
                //parameters.Add("IsModal", true);

                ModalOptions op = new ModalOptions();



                op.ContentScrollable = true;
                op.Class = "blazored-modal " + ModalSize.MediumWindow;

                if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
                {
                    await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
                }

                var messageForm = Modal.Show<LoadCSV>("Config", parameters, op);
                var result = await messageForm.Result;

                if (!result.Cancelled)
                {
                    string pathclass = param.ClassName;//"CalibrationSaaS.Infraestructure.Blazor.GenericMethods.CreateCSV"; // 
                    var inst = Helpers.DynamicHelper.GetInstance(pathclass); //Helpers.DynamicHelper.GetInstance(itemcs.CreateClass);
                    dynamic _strCreate2 = inst; // (CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.ICreateItems<DomainEntity, CalibrationItem>)inst; // new CalibrationSaaS.Infraestructure.Blazor.LTI.Micro.CreateItemsMicro();

                  var data1 = (List<string>)result.Data;
                    
                    //param.Data = data1;

                    var dyn = dynamicProperty.DynamicPropertiesSchema.Where(x => x.Enable == true).OrderBy(x => x.ColPosition).ToList();

                    ICollection<Dictionary<string, object>> colecc= new List<Dictionary<string, object>>();   

                    foreach (var item in data1)
                    {
                        var row = item.Split(',');
                        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                        for (int yi=0; yi<row.Length; yi++)
                        {
                            keyValuePairs.Add(dyn[yi].Name, row[yi]);
                        }

                        colecc.Add(keyValuePairs);

                    }

                    param.Data= colecc; 

                    _strCreate2.Parameter = param;

                    await IniComponent();

                    //var table = await _strCreate2.CreateItems(WorkOrderItemCreate, IsCompresion);


                }
                if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
                {
                    await JSRuntime.InvokeVoidAsync("showElement", HideElement);
                }
            }
            catch(Exception ex) {

                await ExceptionManager(ex, "Error records creation");
            
            }
            
          

        }

        public async Task CalibrationConfig(CalibrationSubType dynamicProperty)
        {
            var parameters = new ModalParameters();
            parameters.Add("NewPerson", dynamicProperty);
            //parameters.Add("IsModal", true);

            ModalOptions op = new ModalOptions();



            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;

            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
            }

            var messageForm = Modal.Show<CalibrationSubTypeForm>("Config", parameters, op);
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {

            }
            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("showElement", HideElement);
            }

        }



        [Parameter]
        public EventCallback<ChangeEventArgs> OnChangeDescription { get; set; }


        public async Task ChangeDescripcion(ChangeEventArgs arg)
        {

            if (WeightSetComponent == null)
            {
                return;
            }

            Description += ((PieceOfEquipment)arg.Value).PieceOfEquipmentID + " | ";

            //var w = ((PieceOfEquipment)arg.Value).WeightSets;
            //WeightSetList2 = w.ToList();

            //if (WeightSetComponent != null && WeightSetList2 != null)
            //{


            //    await WeightSetComponent.Show(w.ToList());

            //}

        }



        public async Task SelectReport(int TestPointID, int WorkOrderDetailID)
        {



            var parameters = new ModalParameters();
            //parameters.Add("SelectOnly", true);
            //parameters.Add("IsModal", true);



            parameters.Add("TestPointID", TestPointID.ToString());
            parameters.Add("WorkOrderDetailID", WorkOrderDetailID.ToString());

            ModalOptions op = new ModalOptions();

            op.Position = ModalPosition.Center;

            op.Class = "blazored-modal " + ModalSize.MediumWindow;  //blazored-modal blazored-modal-scrollable allWindow

            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
            }

            var messageForm = Modal.Show<GetReportByTestPoint>("Report Uncertainty", parameters, op);
            var result = await messageForm.Result;

            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("showElement", HideElement);
            }

            if (result.Cancelled)
            {
                return;
            }
            _message = result.Data?.ToString() ?? string.Empty;

        }

       
        public async Task<string> ShowModal(IModalService _modalService, string jsonTolerancePerTestPoint)
        {
            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
            List<EquipmentTemplate> listEquipment;

            var parameters = new ModalParameters();

            if (jsonTolerancePerTestPoint != null && jsonTolerancePerTestPoint != "")
            {
                tolerance = JsonSerializer.Deserialize<Tolerance>(jsonTolerancePerTestPoint);
            }
            else
            {
                tolerance = SetTolerance();
            }

            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("calibrationTypeId", eq.CalibrationTypeId);
            parameters.Add("ToleranceModal", tolerance);

            ModalOptions op = new ModalOptions();

            op.Class = "blazored-modal " + ModalSize.MediumWindow;

            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
            }
            var messageForm = _modalService.Show<BlazorApp1.Blazor.Blazor.Pages.Basics.ResolutionComponentModal>("Tolerance per Test Point", parameters, op);
            var result = await messageForm.Result;

            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("showElement", HideElement);
            }

            if (!result.Cancelled)
            {
                _message = result.Data?.ToString() ?? string.Empty;
            }
            else
            {
                _message = tolerance?.Json;
            }

            return _message;
        }


        public async Task<string> ShowModal(IModalService _modalService, string dynamicComponentName, string jsonDynamic)
        {
            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
            List<EquipmentTemplate> listEquipment;

            var parameters = new ModalParameters();
            string result_ = null;
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);

            // Extract FilterId fron jsonDynamic if exists
            if (!string.IsNullOrEmpty(jsonDynamic))
            {
                try
                {
                    var dynamicData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonDynamic);

                    // Buscar FilterId o CustomerId en el JSON
                    if (dynamicData.ContainsKey("FilterId"))
                    {
                        var filterId = dynamicData["FilterId"];
                        parameters.Add("CustomerFilterId", filterId);
                        Console.WriteLine($"FilterId add to parameters as CustomerFilterId: {filterId}");
                    }
                    else if (dynamicData.ContainsKey("CustomerId"))
                    {
                        var customerId = dynamicData["CustomerId"];
                        parameters.Add("CustomerFilterId", customerId);
                        Console.WriteLine($"CustomerId Add as CustomerFilterId to parameters: {customerId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al procesar jsonDynamic: {ex.Message}");
                }
            }
           

            ModalOptions op = new ModalOptions();

            op.Class = "blazored-modal " + ModalSize.MediumWindow;

           if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
            }

            Type modalType = Type.GetType($"CalibrationSaaS.Infraestructure.Blazor.Pages.Customer.{dynamicComponentName}");

            if (modalType == null)
            {
                throw new Exception($"Component not Found: {dynamicComponentName}");
            }

            var messageForm = Modal.Show(modalType, "Select", parameters, op);
            
            var result = await messageForm.Result;

            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("showElement", HideElement);
            }

            if (!result.Cancelled)
            {
                result_ = result?.Data != null
                ? System.Text.Json.JsonSerializer.Serialize(result.Data)
                : string.Empty;
            }
            

            return result_;
        }

        /// <summary>
        /// Creates a nested dictionary structure by grouping items and extracting key-value pairs using reflection.
        /// Calls a method from AppState to get the list of items.
        /// </summary>
        /// <param name="appStateMethodName">The name of the method in AppState that returns a list</param>
        /// <param name="keyPropertyName">The name of the property that will serve as the key</param>
        /// <param name="valuePropertyName">The name of the property that will serve as the value</param>
        /// <param name="groupingPropertyName">The name of the property that will be used for grouping</param>
        /// <param name="methodParameters">Optional parameters to pass to the AppState method</param>
        /// <returns>Dictionary where outer key is grouping property value, inner dictionary contains key-value pairs</returns>
        public Dictionary<string, Dictionary<string, string>> CreateGroupedDictionaryFromAppState(
            string appStateMethodName,
            string keyPropertyName,
            string valuePropertyName,
            string groupingPropertyName,
            params object[] methodParameters)
        {
            if (AppState == null)
                throw new InvalidOperationException("AppState is not available");

            if (string.IsNullOrEmpty(appStateMethodName))
                throw new ArgumentException("AppState method name cannot be null or empty", nameof(appStateMethodName));

            if (string.IsNullOrEmpty(keyPropertyName))
                throw new ArgumentException("Key property name cannot be null or empty", nameof(keyPropertyName));

            if (string.IsNullOrEmpty(valuePropertyName))
                throw new ArgumentException("Value property name cannot be null or empty", nameof(valuePropertyName));

            if (string.IsNullOrEmpty(groupingPropertyName))
                throw new ArgumentException("Grouping property name cannot be null or empty", nameof(groupingPropertyName));

            // Get the AppState type to access methods via reflection
            var appStateType = AppState.GetType();

            // Find the method by name
            MethodInfo method = null;

            // First try to find method with exact parameter count
            if (methodParameters != null && methodParameters.Length > 0)
            {
                var parameterTypes = methodParameters.Select(p => p?.GetType()).ToArray();
                method = appStateType.GetMethod(appStateMethodName, parameterTypes);
            }

            // If not found, try to find method by name only (for parameterless methods or overload resolution)
            if (method == null)
            {
                var methods = appStateType.GetMethods().Where(m => m.Name == appStateMethodName).ToArray();

                if (methods.Length == 1)
                {
                    method = methods[0];
                }
                else if (methods.Length > 1)
                {
                    // Try to find the best match based on parameter count
                    var paramCount = methodParameters?.Length ?? 0;
                    method = methods.FirstOrDefault(m => m.GetParameters().Length == paramCount);

                    if (method == null)
                    {
                        method = methods.First(); // Use the first one as fallback
                    }
                }
            }

            if (method == null)
                throw new ArgumentException($"Method '{appStateMethodName}' not found in AppState", nameof(appStateMethodName));

            // Validate that the method returns a list or enumerable
            var returnType = method.ReturnType;
            if (!IsListOrEnumerable(returnType))
                throw new ArgumentException($"Method '{appStateMethodName}' does not return a list or enumerable type", nameof(appStateMethodName));

            // Call the method to get the list
            object listResult;
            try
            {
                listResult = method.Invoke(AppState, methodParameters);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calling method '{appStateMethodName}': {ex.Message}", ex);
            }

            if (listResult == null)
                return new Dictionary<string, Dictionary<string, string>>();

            // Convert to IEnumerable to work with any list type
            var enumerable = (System.Collections.IEnumerable)listResult;
            var items = enumerable.Cast<object>().ToList();

            if (items.Count == 0)
                return new Dictionary<string, Dictionary<string, string>>();

            // Get the type of the items in the list
            var itemType = items.First().GetType();

            // Get property info for the three properties we need
            var keyProperty = itemType.GetProperty(keyPropertyName);
            var valueProperty = itemType.GetProperty(valuePropertyName);
            var groupingProperty = itemType.GetProperty(groupingPropertyName);

            // Validate that all properties exist
            if (keyProperty == null)
                throw new ArgumentException($"Property '{keyPropertyName}' not found on type '{itemType.Name}'", nameof(keyPropertyName));

            if (valueProperty == null)
                throw new ArgumentException($"Property '{valuePropertyName}' not found on type '{itemType.Name}'", nameof(valuePropertyName));

            if (groupingProperty == null)
                throw new ArgumentException($"Property '{groupingPropertyName}' not found on type '{itemType.Name}'", nameof(groupingPropertyName));

            var result = new Dictionary<string, Dictionary<string, string>>();

            // Process each item in the list
            foreach (var item in items)
            {
                if (item == null) continue;

                // Get the values using reflection
                var keyValue = keyProperty.GetValue(item)?.ToString() ?? string.Empty;
                var valueValue = valueProperty.GetValue(item)?.ToString() ?? string.Empty;
                var groupingValue = groupingProperty.GetValue(item)?.ToString() ?? string.Empty;

                // Create the group dictionary if it doesn't exist
                if (!result.ContainsKey(groupingValue))
                {
                    result[groupingValue] = new Dictionary<string, string>();
                }

                // Add the key-value pair to the appropriate group
                // Note: If duplicate keys exist within a group, the last one will overwrite previous ones
                result[groupingValue][keyValue] = valueValue;
            }

            return result;
        }

        /// <summary>
        /// Helper method to check if a type is a list or enumerable
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if the type is a list or enumerable, false otherwise</returns>
        private bool IsListOrEnumerable(Type type)
        {
            if (type == null) return false;

            // Check if it's a generic list
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return true;

            // Check if it implements IEnumerable<T>
            if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                return true;

            // Check if it implements IEnumerable (non-generic)
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type) && type != typeof(string))
                return true;

            return false;
        }

        /// <summary>
        /// Creates a nested dictionary structure by grouping items and extracting key-value pairs using reflection.
        /// This is the original method that accepts a list directly.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list</typeparam>
        /// <param name="keyPropertyName">The name of the property that will serve as the key</param>
        /// <param name="valuePropertyName">The name of the property that will serve as the value</param>
        /// <param name="groupingPropertyName">The name of the property that will be used for grouping</param>
        /// <param name="items">A generic list of objects of any class type</param>
        /// <returns>Dictionary where outer key is grouping property value, inner dictionary contains key-value pairs</returns>
        public Dictionary<string, Dictionary<string, string>> CreateGroupedDictionary<T>(
            string keyPropertyName,
            string valuePropertyName,
            string groupingPropertyName,
            List<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (string.IsNullOrEmpty(keyPropertyName))
                throw new ArgumentException("Key property name cannot be null or empty", nameof(keyPropertyName));

            if (string.IsNullOrEmpty(valuePropertyName))
                throw new ArgumentException("Value property name cannot be null or empty", nameof(valuePropertyName));

            if (string.IsNullOrEmpty(groupingPropertyName))
                throw new ArgumentException("Grouping property name cannot be null or empty", nameof(groupingPropertyName));

            var result = new Dictionary<string, Dictionary<string, string>>();

            // Get the type of T to access properties via reflection
            var type = typeof(T);

            // Get property info for the three properties we need
            var keyProperty = type.GetProperty(keyPropertyName);
            var valueProperty = type.GetProperty(valuePropertyName);
            var groupingProperty = type.GetProperty(groupingPropertyName);

            // Validate that all properties exist
            if (keyProperty == null)
                throw new ArgumentException($"Property '{keyPropertyName}' not found on type '{type.Name}'", nameof(keyPropertyName));

            if (valueProperty == null)
                throw new ArgumentException($"Property '{valuePropertyName}' not found on type '{type.Name}'", nameof(valuePropertyName));

            if (groupingProperty == null)
                throw new ArgumentException($"Property '{groupingPropertyName}' not found on type '{type.Name}'", nameof(groupingPropertyName));

            // Process each item in the list
            foreach (var item in items)
            {
                if (item == null) continue;

                // Get the values using reflection
                var keyValue = keyProperty.GetValue(item)?.ToString() ?? string.Empty;
                var valueValue = valueProperty.GetValue(item)?.ToString() ?? string.Empty;
                var groupingValue = groupingProperty.GetValue(item)?.ToString() ?? string.Empty;

                // Create the group dictionary if it doesn't exist
                if (!result.ContainsKey(groupingValue))
                {
                    result[groupingValue] = new Dictionary<string, string>();
                }

                // Add the key-value pair to the appropriate group
                // Note: If duplicate keys exist within a group, the last one will overwrite previous ones
                result[groupingValue][keyValue] = valueValue;
            }

            return result;
        }

    }


}

using Blazor.IndexedDB.Framework;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Helper;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Tools1;
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
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using Microsoft.Extensions.Configuration;
using Helpers.Controls.ValueObjects;
using Reports.Domain.ReportViewModels;
using System.Security.Cryptography.X509Certificates;
using Helpers;

using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.LTI.Mass;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers.Models;
using Blazed.Controls.MultiComponent;
using FormatValidator;
using System.IO;
using CalibrationSaaS.Infraestructure.Blazor.GenericMethods;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using System.Dynamic;
using System.Reflection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace CalibrationSaaS.Infraestructure.Blazor.Shared.ReportView
{
    public partial  class GenericTestComponent<CalibrationItem, CalibrationItemResult,DomainEntity>  
          : CalibrationSaaS.Infraestructure.Blazor.GridResults2ComponentBase2<DomainEntity, CalibrationItem> 
         where CalibrationItem : class, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>, IResultComp, new() 
        where CalibrationItemResult :class, IResult2, IResultComp, IResultGen2,IDynamic, IUpdated,ISelect, new() 
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

        public Blazed.Controls.MultiComponent.MultipleComponent MultipleComponent { get; set; }


        //[Parameter]
        //public ICreateItems _strCreate { get; set; }

        //[Parameter]
        //public IGetItems _strGet { get; set; }


        //[Parameter]
        //public ISelectItems SelectStandards { get; set; }



        [Parameter]
        public bool ShowEnviroment { get; set; }

        public CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.EnviromentComponent EnviromentComponent { get; set; }

        [Inject] 

        Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>> _poeServices { get; set; }



        //[CascadingParameter(Name = "CascadeParam1")]
        //public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

        [CascadingParameter(Name = "CascadeParam2")]
        public ICalibrationType eq { get; set; }




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
                var defaultitem = await _strNew.DefaultNewItem(WorkOrderItemCreate, IsCompresion);
                if (defaultitem != null)
                {
                    foreach (var item in defaultitem)
                    {
                        foreach (var item2 in item.TestPointResult)
                        {
                            await RT.SaveNewItem(item2 as CalibrationItemResult);
                        }
                    }


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
        public Domain.Aggregates.Shared.Basic.AppState AppState { get; set; }

        //[Inject] public AppStateCompany AppState { get; set; }

        [Parameter]
        public Domain.Aggregates.Entities.Status CurrentStatus { get; set; } = new Domain.Aggregates.Entities.Status();

        public EditContext editContext { get; set; }

        //[Parameter]
        //public WorkOrderDetail eq { get; set; } = new WorkOrderDetail();


        public List<CalibrationItemResult> LIST { get; set; } = new List<CalibrationItemResult>();


        public CalibrationItemResult NewItem { get; set; } 



        public dynamic WeightSetComponent { get; set; } //= new WeightSetComponent();

        PieceOfEquipment poq = new PieceOfEquipment();

        [Parameter]
        public List<WeightSet> WeightSetList2 { get; set; } = new List<WeightSet>();


        public bool IsNew { get; set; }


        public bool FirstRender { get; set; }


        //WorkOrderDetail wod = new WorkOrderDetail();

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnParametersSetAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            //var resultsetw = await _assetsServices.GetWeightSetXPoE(poq);

            //WeightSetList2 = resultsetw.ToList();

            //StateHasChanged();


            //if (!FirstRender)
            //{
            //    return ;
            //}







        }


#pragma warning disable CS0414 // El campo 'LinearityComponentBase.WeightSetListCount' está asignado pero su valor nunca se usa
        int WeightSetListCount = 0;
#pragma warning restore CS0414 // El campo 'LinearityComponentBase.WeightSetListCount' está asignado pero su valor nunca se usa

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            var a = Enabled;

            try
            {
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


                //if (FirstRender &&  WorkOrderItemCreate.eq != null && WorkOrderItemCreate.eq.WOD_Weights != null)
                //{

                //    foreach (var w in WorkOrderItemCreate.eq.WOD_Weights)
                //    {
                //        if (w.WeightSet != null)
                //        {
                //            WeightSetList2.Add(w.WeightSet);
                //        }

                //    }


                //    if (WeightSetComponent != null && WeightSetList2 != null)
                //    {


                //        await WeightSetComponent.Show(WeightSetList2);

                //    }


                //}




            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {

            }
            finally
            {

            }
        }



        public List<CalibrationItem> tableList = null;

        public List<CalibrationItemResult> tableListResult = null;

        public CalibrationSubType CalSubType { get; set; }


        public string PropertyMax { get; set; }

        public double GetMaxValue()
        {

            if(!string.IsNullOrEmpty(PropertyMax))
            {
                foreach (var item in RT.Items)
                {
                    dynamic Model = Newtonsoft.Json.JsonConvert.DeserializeObject<double>(item.Object);

                    var value = Model.GetPropValue(PropertyMax);

                    return value;

                }


            }

            return 0;
        
        
        }

            public Tolerance SetTolerance()
        {
            Tolerance tolerance = new Tolerance();
            //default;
            if (WorkOrderItemCreate.Tolerance.ToleranceTypeID.HasValue)
            {

                tolerance = new Tolerance();

                //tolerance.ToleranceTypeID = WorkOrderItemCreate.Tolerance.ToleranceTypeID.Value;

                //if (WorkOrderItemCreate.Tolerance.FullScale.HasValue)
                //{
                //    tolerance.FullScale = WorkOrderItemCreate.Tolerance.FullScale.Value;
                //}

                //tolerance.ToleranceValue = WorkOrderItemCreate.ToleranceValue;
                //tolerance.ToleranceFixedValue = WorkOrderItemCreate.ToleranceFixedValue;
                //tolerance.AccuracyPercentage = WorkOrderItemCreate.AccuracyPercentage;
               
                tolerance = WorkOrderItemCreate.Tolerance;

                //tolerance.ResolutionValue = WorkOrderItemCreate.Resolution;
                //tolerance.ToleranceFixedValue = WorkOrderItemCreate.ToleranceFixedValue;
                tolerance.DecimalNumber = WorkOrderItemCreate.DecimalNumber;
                tolerance.Resolution = WorkOrderItemCreate.Resolution;

                tolerance.MaxValue = GetMaxValue();

            }

            return tolerance;   
        }

        public CreateModel param { get; set; }

        bool HasNewItem = false;
        protected override async Task OnInitializedAsync()

        {


            await IniComponent();




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
                                object result = addMethod.Invoke(this, new object[] { CalSubType.CalibrationTypeId });

                                var optio = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                                dyn[yi].ViewPropertyBase.SelectOptions = optio;


                            }
                            else if (!string.IsNullOrEmpty(dyn[yi]?.ViewPropertyBase?.SelectOptions) && dyn[yi]?.ViewPropertyBase?.SelectOptions?.ToLower() == "GetAllUoMDic".ToLower())
                            {
                                MethodInfo addMethod = typeof(AppState).GetType().GetMethod(dyn[yi]?.ViewPropertyBase?.SelectOptions);
                                object result = addMethod.Invoke(this, new object[] {  });

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
                {   CreateModel paramNew = CalSubType.NewModel();
                    ErrorMessage += paramNew.ErrorMessage;
                    string pathclass = paramNew.ClassName;
                    var ins = Helpers.DynamicHelper.GetInstance(pathclass);
                    _strNew = (CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces.INewItem<DomainEntity, CalibrationItem>)ins;//new CalibrationSaaS.Infraestructure.Blazor.LTI.Micro.GetItemsMicro();
                    ShowGrid = true;
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

                    PropertyMax = field.Name;

                    //var a = WorkOrderItemCreate.TestPointResult[0].FirstOrDefault();

                    //var b = a.Object;

                    //var des= 


                }


                List<CalibrationItem> getreturn = null;

                
                var HasData = false;

                if (_strGet != null)
                {

                    var ihneritCalSubtypeId1 = CalSubType?.inheritCalibrationSubTypeId;
                    var ihneritCalSubtypeId = 0;
                    if (ihneritCalSubtypeId1 != null)
                        ihneritCalSubtypeId = (int)ihneritCalSubtypeId1;

                    getreturn = await _strGet.GetItems(WorkOrderItemCreate, IsCompresion, ihneritCalSubtypeId);

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

                        tableList = await _strCreate.CreateItems(WorkOrderItemCreate, IsCompresion);



                        


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

                        tableList[0].Component = Component.Name;
                        tableList[0].TestPointResult[0].Component = Component.Name;
                        var iu2 = tableList[0].TestPointResult[0] as CalibrationItemResult;
                        if (iu2.Position != -1)
                        {
                            list34.Add(iu2);
                        }
                        else
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

                            var iu = itemm as CalibrationItemResult;
                            if (iu.Position != -1)
                            {
                                
                                list34.Add(iu);
                            }
                            else
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

                else if (CalibrationSubTypeView != null)
                {
                    NoDataMessage = CalibrationSubTypeView.NoDataMessage;

                }

                if ((tableList == null || tableList?.Count == 0) && (!CalSubType.SupportCSV.HasValue || !CalSubType.SupportCSV.Value))
                {
                    ConfigMessage += CalSubType.CreateClass + " " + Environment.NewLine + CalSubType.GetClass + " " + Environment.NewLine;

                    if (!string.IsNullOrEmpty(CalSubType.SelectStandarClass))
                    {
                        ConfigMessage = ConfigMessage + CalSubType.SelectStandarClass;
                    }
                }
                else if (tableList != null && tableList.Count >= 0)
                {
                    ShowGrid = true;
                }

                if (CalSubType?.CalibrationSubTypeView?.EnabledSelect == true && SelectStandards == null)
                {
                    ConfigMessage += "Select Class not configured";
                }
                if (CalSubType?.CalibrationSubTypeView?.EnableSelect2 == true && Select2Standards == null)
                {
                    ConfigMessage += "Select2 Class not configured";
                }
                if (CalSubType?.CalibrationSubTypeView?.EnabledNew == true && _strNew == null && CalSubType?.CalibrationSubTypeView?.HasNewButton == true)
                {
                    ConfigMessage += "New Class not configured";
                }



                
                if (!HasData && tableList != null && tableList.Count > 0 && DataLoad != null)
                {
                  DataLoad(CalibrationSubType).ConfigureAwait(true).GetAwaiter().GetResult();
                }
                


                //UoM
                //
                //LIST = RT.Items;

            }
            catch (Exception ex)
            {

                await ExceptionManager(ex, "Error in calibrationSubtype: " + CalibrationSubType);

            }
            finally
            {
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

        //public async Task ExecuteFormula()
        //{

        //    try
        //    {


        //        await ShowProgress(false);
        //        //HasChange = false;
        //        var aa = await IExecuteMethod.ExcuteMethod(RT.Items.OrderBy(x => x.BasicCalibrationResult.Position).ToList<GenericCalibration>());
        //        int cont = 0;
        //        Console.WriteLine("OnUserFinish " + RT.Items.Count + " --- " + aa.Count);
        //        foreach (var item in aa.OrderBy(x => x.BasicCalibrationResult.Position))
        //        {

        //            //item.BasicCalibrationResult.MaxErrorNominal = aa.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == item.BasicCalibrationResult.SequenceID).BasicCalibrationResult.MaxErrorNominal;
        //            //item.BasicCalibrationResult.MaxErrorNominalASTM = aa.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == item.BasicCalibrationResult.SequenceID).BasicCalibrationResult.MaxErrorNominalASTM;
        //            RT.Items[cont] = item as CalibrationItem;
        //            cont++;
        //        }
        //        LIST = RT.Items.OrderBy(x => x.BasicCalibrationResult.Position).ToList();
        //        RT.ItemList = RT.Items;
        //        RT.ItemsDataSource = RT.Items;

        //        //HasChange = true;
        //        //aTimer2.Start();
        //        StateHasChanged();

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("error onuserfinish " + ex.Message);
        //    }
        //    finally
        //    {
        //        await CloseProgress();
        //        IniCalculated = 2;
        //    }



        //}









        [Parameter]
        public string HideElement { get; set; } =  "modalLinearity";

        [CascadingParameter] public IModalService Modal { get; set; }
        public string _message { get; set; }
        //Logger.LogDebug(Group.Name);

        bool refresh = false;
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


                //switch ()
                //{

                //}

                IModalReference messageForm=null;
                if (!string.IsNullOrEmpty(CalSubType.StandardAssignComponent))
                {
                    switch (CalSubType.StandardAssignComponent)
                    {
                        case "weightset":

                            messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.GenericMethods.SelectWeightComponent<GenericCalibration2>), "Select Weight Kit ", parameters, op);
                            break;


                        case "standard":

                            messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.GenericMethods.SelectStandardComponent<GenericCalibration2>), "Select Weight Kit ", parameters, op);
                            break;


                        case "LTI":
                            messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<GenericCalibration2>), "Select ", parameters, op);
                            //CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Target>
                            break;

                        case "rockwell":
                            messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.WeightComponent<GenericCalibration2>), "Select Standard", parameters, op);
                            //CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Target>
                            break;
                    }
                }
                else
                {
                    messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.WeightComponent<GenericCalibration2>), "Select ", parameters, op);
                }
               





                //Console.WriteLine("select 1");
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

                            messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.Scale.WeightComponent<GenericCalibration2>), "Select Weight", parameters, op);
                            break;


                        case "LTI":
                            messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<GenericCalibration2>), "Select ", parameters, op);
                            //CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Target>
                            break;

                        case "rockwell":
                            messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.WeightComponent<GenericCalibration2>), "Select Standard", parameters, op);
                            //CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Target>
                            break;
                    }
                }
                else
                {
                    messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.WeightComponent<GenericCalibration>), "Select ", parameters, op);
                }



                //var messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.WeightComponent<GenericCalibration>), "Select ", parameters, op);

                //Console.WriteLine("select 1");
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


        //public async Task<bool> ValidateCSV(Stream json,string config)
        //{
        //    List<RowValidationError> errors = new List<RowValidationError>();

            

        //    DateTime start = DateTime.Now;
        //    //Validator validator = Validator.FromJson(System.IO.File.ReadAllText(config));
        //    //FileSourceReader source = new FileSourceReader(json);

        //    Validator validator = Validator.FromJson(config);
        //    FileSourceReader source = new FileSourceReader(json);

        //    foreach (RowValidationError current in validator.Validate(source))
        //    {
        //        errors.Add(current);
                
        //    }
        //    ErrorMessage = string.Join(", ", errors);
        //    if(errors.Count > 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;    
        //    }

        //}
      
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

    }


}

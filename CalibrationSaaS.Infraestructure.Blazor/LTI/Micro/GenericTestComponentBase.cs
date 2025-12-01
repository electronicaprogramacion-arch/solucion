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

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public partial  class GenericTestComponent<CalibrationItem, CalibrationItemResult>  
          : CalibrationSaaS.Infraestructure.Blazor.GridResultsComponentBase<GenericCalibration> 
         where CalibrationItem : GenericCalibration, new() where CalibrationItemResult : GenericCalibrationResult, new()
    {



        [Parameter]
        public Func<int, Task> ChangePage { get; set; }


        [Parameter]
        public CalibrationSubTypeView CalibrationSubTypeView { get; set; }



        [Parameter]
        public string ID { get; set; }


        [Parameter]
        public string CalibrationSubType { get; set; }

        public Blazed.Controls.MultiComponent.MultipleComponent MultipleComponent { get; set; }


        [Parameter]
        public ICreateItems _strCreate { get; set; }

        [Parameter]
        public IGetItems _strGet { get; set; }


        [Parameter]
        public ISelectItems SelectStandards { get; set; }



        [Parameter]
        public bool ShowEnviroment { get; set; }

        public CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.EnviromentComponent EnviromentComponent { get; set; }

        [Inject] public Application.Services.IPieceOfEquipmentService<CallContext> _poeServices { get; set; }

        
        
        
        
        [CascadingParameter(Name = "CascadeParam1")]
        public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

        [CascadingParameter(Name = "CascadeParam2")]
        public ICalibrationType eq { get; set; }

        [Parameter]
        public int IsCompresion { get; set; } = 0;

        //[Inject]
        //public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public bool RefreshResolution { get; set; }

        public ElementReference InputText { get; set; }

        

        [Parameter]
        public bool Enabled { get; set; } = false;

        public string Description { get; set; } = "";

       


        //[Inject]
        //public Func<IIndexedDbFactory, Application.Services.IWorkOrderDetailServices<CallContext>> Service { get; set; }
        [Inject] Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
        public string url { get; set; }


        

        [Parameter]
        public ResponsiveTable<CalibrationItem> RT { get; set; } = new ResponsiveTable<CalibrationItem>();

        public string SortField { get; set; }

        public bool HasDescendants { get; set; }

        public int MyProperty { get; set; }

        [Parameter]
        public CalibrationType cmcValues { get; set; }

        public CalibrationItem RowAfterRender(CalibrationItem lin)
        {

            var srt = RT.Items.Where(x => x.SequenceID == lin.SequenceID).ToList();
            if (srt.Count > 1)
            {


                lin.SequenceID = RT.Items.MaxBy(x => x.SequenceID).SequenceID + 1;
                lin.BasicCalibrationResult.SequenceID = lin.SequenceID;
                int cont1 = 0;
                foreach (var itemn in RT.Items)
                {
                    
                    itemn.BasicCalibrationResult.Position = cont1;
                    cont1++;
                }


            }

            
            //}

            IniCalculated = 1;
           

            return lin;

        }



        public CalibrationItem RowChange(CalibrationItem lin)
        {



            if (RT.IsDragAndDrop)
            {
                int posi = 100;

                for (int i = 0; i < RT.ItemList.Count; i++)
                {
                    RT.ItemList[i].BasicCalibrationResult.Position = posi;
                    posi++;
                    RT.ReplaceItemKey(RT.ItemList[i]);
                }

                return lin;
            }

            if (lin.BasicCalibrationResult.Position >= 100)
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

        public async Task TaskAfterSave(CalibrationItem item, string sav)
        {
            if (IsNewItem)
            {
                //await Calculate();

            }

            IsNewItem = false;

        }


        public async Task AddNewItem()
        {

            await RT.SaveNewItem(DefaultNew());


        }

        public bool IsNewItem { get; set; }
        public CalibrationItem DefaultNew()
        {
            CalibrationItem lin = new CalibrationItem();

            IsAddLinearity = true;

            double result = 0;
            try
            {
                result = Convert.ToDouble(TextNewValue);
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                return null;
            }

           



            lin.BasicCalibrationResult = new CalibrationItemResult();
            lin.TestPoint = new TestPoint();
            lin.WeightSets = new List<WeightSet>();



            //lin.TestPoint.UnitOfMeasurement = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure;

            //lin.TestPoint.UnitOfMeasurementID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;

            //lin.TestPoint.UnitOfMeasurementOutID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;

            //lin.TestPoint.Resolution = WorkOrderItemCreate.eq.Resolution;

            //lin.TestPoint.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;

            

            //lin.TestPoint.CalibrationType = LTILogic.GetCalibrationSubTypeStr(WorkOrderItemCreate.eq,IsCompresion);

            //lin.TestPoint.TestPointTarget = 1;

           


            //lin.BasicCalibrationResult.CalibrationSubTypeId = LTILogic.GetCalibrationSubType(WorkOrderItemCreate.eq,IsCompresion);

            //lin.BasicCalibrationResult.SequenceID = RT.Items.Count + 1;

            //lin.BasicCalibrationResult.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;

            //lin.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;

            //lin.SequenceID = RT.Items.Count + 1;

            //lin.UnitOfMeasureId = lin.TestPoint.UnitOfMeasurementID;

            //lin.TestPoint.NominalTestPoit = result;

         
            //lin.CalibrationSubTypeId = lin.BasicCalibrationResult.CalibrationSubTypeId;
            
          
          
            IsNewItem = true;

            return lin;

        }



        public async Task ChangeMultipleControl(ChangeEventArgs arg, dynamic lin, bool AddWeight = false)
        { 
        
        
        
        }

            public async Task ChangeControl2(ChangeEventArgs arg, CalibrationItem lin, bool AddWeight = false)
        {
            try
            {
                if (arg == null || arg.Value.ToString() == "")
                {
                    return;
                }


                var result = Convert.ToDouble(arg.Value);

                if (AddWeight)
                {
                    AddWeightset(lin, result);
                }

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

        public void AddWeightset(CalibrationItem lin, double result = 0)
        {
            if (lin != null && lin?.WeightSets != null)
            {
                lin.WeightSets.Clear();

            }
            else
            {
                lin.WeightSets = new List<WeightSet>();
            }

            WeightSet w = new WeightSet();
            w.WeightNominalValue = (int)result;
            w.WeightActualValue = result;
            w.PieceOfEquipmentId_new = lin.WorkOrderDetailId;
            lin.WeightSets.Add(w);
            lin.WeightSets = null;

            //lin.BasicCalibrationResult.WeightApplied = result;
        }


        public async Task ChangeControl(ChangeEventArgs arg, CalibrationItem lin)
        {

            return;

#pragma warning disable CS0162 // Se detectó código inaccesible
            double result = 0;
#pragma warning restore CS0162 // Se detectó código inaccesible
            try
            {

                if (arg == null || arg.Value.ToString() == "")
                {
                    return;
                }

                result = Convert.ToDouble(arg.Value);


                if (lin != null && lin?.WeightSets != null)
                {
                    lin.WeightSets.Clear();

                }
                else
                {
                    lin.WeightSets = new List<WeightSet>();
                }

                WeightSet w = new WeightSet();
                w.WeightNominalValue = (int)result;
                w.WeightActualValue = result;
                w.PieceOfEquipmentId_new = lin.WorkOrderDetailId;
                lin.WeightSets.Add(w);
                lin.WeightSets = null;

                //lin.BasicCalibrationResult.WeightApplied = result;

                // remove previous one
                aTimer.Stop();
                // new timer
                aTimer.Start();
                //IniCalculated = 1;
                if (IniCalculated == 2)
                {

                    await Calculate();


                    IniCalculated = 1;
                }




            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {

            }

        }

       


        public string BoudValues(CalibrationItem lin)
        {



            return "";

        }

        [Parameter]
        public bool Accredited { get; set; }

        public bool IsWeightLoaded { get; set; }

       

        public async Task Calculate()
        {
            return;
            


        }

        


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
     

        List<WeightSet> Selected = new List<WeightSet>();




       


        public CalibrationItem Linearity = new CalibrationItem();

        [Inject] public AppStateCompany AppState { get; set; }

        [Parameter]
        public Domain.Aggregates.Entities.Status CurrentStatus { get; set; } = new Domain.Aggregates.Entities.Status();

        public EditContext editContext { get; set; }

        //[Parameter]
        //public WorkOrderDetail eq { get; set; } = new WorkOrderDetail();


        public List<CalibrationItem> LIST { get; set; } = new List<CalibrationItem>();


        public WeightSetComponent WeightSetComponent { get; set; } //= new WeightSetComponent();

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





        protected override async Task OnInitializedAsync()

        {
            HideElement=  CalibrationSubType + "-modal";

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

            List<GenericCalibration> tableList = null;

            List<GenericCalibration> getreturn = null;

            var HasData = false;
            if (_strGet != null)
            {
                getreturn =await  _strGet.GetItems(WorkOrderItemCreate.eq, IsCompresion);
                if (getreturn != null && getreturn.Count > 0)
                {
                    HasData = true;
                }
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

                    if(_poeServices != null)
                    {
                        _strCreate.PoeServices = _poeServices;

                        tableList = await _strCreate.CreateItems(WorkOrderItemCreate.eq, IsCompresion);
                    }
                   
                }
                

            }



            if(tableList != null && tableList.Count > 0)
            {
                RT.Clear();

                SortField = RT.GetPropertyName(() => Linearity.BasicCalibrationResult.Position);
                var list34 = new List<CalibrationItem>();
                foreach (var itemm in tableList)
                {


                    if (itemm.BasicCalibrationResult.Resolution == 0)
                    {
                        itemm.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Resolution;
                        itemm.BasicCalibrationResult.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;
                    }

                    var iu = itemm as CalibrationItem;

                    //RT.SaveNewItem(iu, "asc", SortField);
                    list34.Add(iu);
                  


                }

                LIST = list34.OrderBy(x => x.CalibrationSubTypeId).ToList();
                RT.ItemList = list34;
                RT.ItemsDataSource = list34;

                if (!IsWeightLoaded)
                {
                    //aTimer.Stop();
                    //aTimer.Start();
                    //IniCalculated = 2;
                    //await Calculate();
                }
            }
            
            else if(CalibrationSubTypeView != null)
            {
                NoDataMessage = CalibrationSubTypeView.NoDataMessage;

            }




            }

        public string NoDataMessage { get; set; }
        private void OnUserFinish(Object source, ElapsedEventArgs e)
        {

            
                
                aTimer.Stop();

                List<CalibrationItem> a = new List<CalibrationItem>();


                InvokeAsync(async () =>
           {

               await ExecuteFormula();

           });

        }


        [Parameter]
        public IExecuteMethod IExecuteMethod  { get; set; }

        public async Task ExecuteFormula()
        {

            try
            {


                await ShowProgress(false);
                //HasChange = false;
                var aa = await IExecuteMethod.ExcuteMethod(RT.Items.OrderBy(x => x.BasicCalibrationResult.Position).ToList<GenericCalibration>());
                int cont = 0;
                //Console.WriteLine("OnUserFinish " + RT.Items.Count + " --- " + aa.Count);
                foreach (var item in aa.OrderBy(x => x.BasicCalibrationResult.Position))
                {

                    //item.BasicCalibrationResult.MaxErrorNominal = aa.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == item.BasicCalibrationResult.SequenceID).BasicCalibrationResult.MaxErrorNominal;
                    //item.BasicCalibrationResult.MaxErrorNominalASTM = aa.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == item.BasicCalibrationResult.SequenceID).BasicCalibrationResult.MaxErrorNominalASTM;
                    RT.Items[cont] = item as CalibrationItem;
                    cont++;
                }
                LIST = RT.Items.OrderBy(x => x.BasicCalibrationResult.Position).ToList();
                RT.ItemList = RT.Items;
                RT.ItemsDataSource = RT.Items;

                //HasChange = true;
                //aTimer2.Start();
                StateHasChanged();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("error onuserfinish " + ex.Message);
            }
            finally
            {
                await CloseProgress();
                IniCalculated = 2;
            }



        }

            public List<CalibrationItem> FivePonints()
        {
            List<CalibrationItem> listlienarity = new List<CalibrationItem>();

            for (int i = 0; i < 5; i++)
            {
                CalibrationItem ln = new CalibrationItem();

                ln.SequenceID = i;
                ln.TestPoint = new TestPoint();
                ln.BasicCalibrationResult = new CalibrationItemResult();
                ln.WeightSets = new List<WeightSet>();
                listlienarity.Add(ln);
            }



            return listlienarity;

        }

        public List<CalibrationItem> TenPonints()
        {
            List<CalibrationItem> listlienarity = new List<CalibrationItem>();

            for (int i = 0; i < 5; i++)
            {
                listlienarity.Add(new CalibrationItem { SequenceID = i });
            }



            return listlienarity;


        }

        public async Task SetFivePoints()
        {
            LIST = await Task.FromResult(FivePonints());

            StateHasChanged();
        }



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

                var selec = await SelectStandards.SelectItems(WorkOrderItemCreate.eq, IsCompresion, itemRow, position);


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

                var messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.WeightComponent<GenericCalibration>), "Select ", parameters, op);

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

                var lst = RT.Items as List<GenericCalibration>;


                //r result of popup
                //resultado select.result list of poes
                //itemrow current row to asign
                //items: lst all items in grid
                //position
                await SelectStandards.StandardTask(WorkOrderItemCreate.eq, r,selec.Result, itemRow, lst, position);

                StateHasChanged();

                refresh = false;

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
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

    }


}

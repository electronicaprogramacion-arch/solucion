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
using BlazorApp1.Blazor.Blazor.Pages.Order;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public  class GenericComponentBase<CalibrationItem, CalibrationItemResult> 
        //: CalibrationSaaS.Infraestructure.Blazor.GridResultsComponentBase2<CalibrationItem, CalibrationItemResult> 
        where CalibrationItem : IGenericCalibrationSubType<CalibrationItemResult>, new() where CalibrationItemResult : IResult2, new()
    {

        

        public CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.EnviromentComponent EnviromentComponent { get; set; }

        [Inject] public Application.Services.IPieceOfEquipmentService<CallContext> _poeServices { get; set; }

        [CascadingParameter(Name = "CascadeParam1")]
        public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

        [Parameter]
        public int IsCompresion { get; set; } = 0;

        //[Inject]
        //public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public bool RefreshResolution { get; set; }

        public string TextMultiplerFound { get; set; } = "1";

        public string TextMultiplerLeft { get; set; } = "1";


        public string TextNewValue { get; set; }



        public bool EnableSetPoints { get; set; } = true;

        public CalibrationItem _Linearity { get; set; }


        public bool IsAddLinearity { get; set; }

        public string Description { get; set; } = "";

       


       
        [Inject] Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
        public string url { get; set; }



        public string SortField { get; set; }

        public bool HasDescendants { get; set; }


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



            lin.TestPoint.UnitOfMeasurement = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure;

            lin.TestPoint.UnitOfMeasurementID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;

            lin.TestPoint.UnitOfMeasurementOutID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;

            lin.TestPoint.Resolution = WorkOrderItemCreate.eq.Resolution;

            lin.TestPoint.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;

            //lin.TestPoint.UnitOfMeasurement = lin.TestPoint.UnitOfMeasurementID.GetUoM(AppState.UnitofMeasureList);

            lin.TestPoint.CalibrationType = LTILogic.GetCalibrationSubTypeStr(WorkOrderItemCreate.eq,IsCompresion);

            lin.TestPoint.TestPointTarget = 1;

            //lin.BasicCalibrationResult.WeightApplied = 0;

            //lin.BasicCalibrationResult.AsFound = 0;

            //lin.BasicCalibrationResult.AsFound = 0;


            //lin.BasicCalibrationResult.UnitOfMeasureID = lin.TestPoint.UnitOfMeasurementOutID;


            lin.BasicCalibrationResult.CalibrationSubTypeId = LTILogic.GetCalibrationSubType(WorkOrderItemCreate.eq,IsCompresion);

            lin.BasicCalibrationResult.SequenceID = RT.Items.Count + 1;

            lin.BasicCalibrationResult.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;

            lin.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;

            lin.SequenceID = RT.Items.Count + 1;

            lin.UnitOfMeasureId = lin.TestPoint.UnitOfMeasurementID;

            lin.TestPoint.NominalTestPoit = result;

         
            lin.CalibrationSubTypeId = lin.BasicCalibrationResult.CalibrationSubTypeId;
            
          
          
            IsNewItem = true;

            return lin;

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


       

       


        public string BoudValues(CalibrationItem lin)
        {



            return "";

        }

        [Parameter]
        public bool Accredited { get; set; }

        public bool IsWeightLoaded { get; set; }

       

       

        List<WeightSet> Selected = new List<WeightSet>();




        [Parameter]
        public string EntityID { get; set; }


        public CalibrationItem Linearity = new CalibrationItem();

        [Inject] public AppStateCompany AppState { get; set; }

        [Parameter]
        public Domain.Aggregates.Entities.Status CurrentStatus { get; set; } = new Domain.Aggregates.Entities.Status();

        public EditContext editContext { get; set; }      




        
        PieceOfEquipment poq = new PieceOfEquipment();

      


        public bool IsNew { get; set; }


        public bool FirstRender { get; set; }


      


#pragma warning disable CS0414 // El campo 'LinearityComponentBase.WeightSetListCount' está asignado pero su valor nunca se usa
        public int WeightSetListCount { get; set; } = 0;
#pragma warning restore CS0414 // El campo 'LinearityComponentBase.WeightSetListCount' está asignado pero su valor nunca se usa

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);


        }





        protected override async Task OnInitializedAsync()

        {

            ValidateGrid = LTILogic.ValidateRockwellList;

            url = Configuration.GetSection("Reports")["URL"];

            Linearity.TestPoint = new TestPoint();

            editContext = new EditContext(Linearity);

            aTimer = new System.Timers.Timer(500);

            aTimer.AutoReset = false;

            aTimer.Elapsed += OnUserFinish;

            await base.OnInitializedAsync();


            


            //return result;

            Console.WriteLine("Refreshresolution");
                Console.WriteLine(RefreshResolution);
                if (  ValidateGrid(WorkOrderItemCreate.eq, IsCompresion))
                {
                    Console.WriteLine("GetForceList");
                var cs = LTILogic.GetCalibrationSubType(WorkOrderItemCreate.eq, IsCompresion);
                var tableList = WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Rockwells.Where(x => x.CalibrationSubTypeId == cs).ToList(); //LTILogic.GetForceListGeneric<CalibrationItem>(WorkOrderItemCreate.eq, IsCompresion);  //eq?.BalanceAndScaleCalibration?.Linearities.ToList();

                    RT.Clear();
                    //SortField = RT.GetPropertyName(() => Linearity.TestPoint.Position);
                    SortField = RT.GetPropertyName(() => Linearity.BasicCalibrationResult.Position);
                    foreach (var itemm in tableList)
                    {


                        if (itemm.BasicCalibrationResult.Resolution == 0)
                        {
                            itemm.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Resolution;
                            itemm.BasicCalibrationResult.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;
                        }
                    //await RT.SaveNewItem(itemm);
#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                    var iu = itemm as CalibrationItem;

                    RT.SaveNewItem(iu, "asc", SortField);
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.

                    }

                    if (!IsWeightLoaded)
                    {
                        //aTimer.Stop();
                        //aTimer.Start();
                        //IniCalculated = 2;
                        //await Calculate();
                    }
                }

                else

                    /////NEw CODE
                    if (!ValidateGrid(WorkOrderItemCreate.eq, IsCompresion))
                {



                var pag = new Pagination<POE_Scale>();

                pag.Show = 1000;

                pag.Entity = new POE_Scale();

                pag.Entity.PieceOfEquipmentID = WorkOrderItemCreate.eq.PieceOfEquipmentId;

                var _poeGrpc = new PieceOfEquipmentGRPC(_poeServices);

                var g = new CallOptions(await GetHeaderAsync());

                var result = (await _poeGrpc.GetPOEScale(pag, Header));


                List<Rockwell> listlienarity = new List<Rockwell>();//LTILogic.CreateListGeneric<CalibrationItem>(WorkOrderItemCreate.eq, IsCompresion);

                var cs = LTILogic.GetCalibrationSubType(WorkOrderItemCreate.eq,IsCompresion);

                int cont2 = 1;
                int cont = 1;
                foreach (var itemr in result.List)
                {

                   var rock= new Rockwell();    
                    rock.BasicCalibrationResult=new RockwellResult();

                    rock.BasicCalibrationResult.ScaleRange = itemr.Scale + "-LO";

                    rock.CalibrationSubTypeId = cs;
                    rock.BasicCalibrationResult.CalibrationSubTypeId = rock.CalibrationSubTypeId;

                    rock.BasicCalibrationResult.Position = cont;

                    rock.BasicCalibrationResult.SequenceID = cont;
                    rock.SequenceID = cont;

                    var rock2 = new Rockwell();
                    rock2.BasicCalibrationResult = new RockwellResult();

                    rock2.BasicCalibrationResult.ScaleRange = itemr.Scale + "-HI";

                    rock2.CalibrationSubTypeId = cs;
                    rock2.BasicCalibrationResult.CalibrationSubTypeId = rock2.CalibrationSubTypeId;
                    rock2.BasicCalibrationResult.Position = cont++;
                    rock2.BasicCalibrationResult.SequenceID = cont;
                    rock2.SequenceID = cont;

                    listlienarity.Add(rock);
                    listlienarity.Add(rock2);

                    cont++;
                }


                if (result != null)
                    {
                        RT.Clear();
                        foreach (var itemm in listlienarity)
                        {
                            if (itemm.BasicCalibrationResult.Resolution == 0)
                            {
                                itemm.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Resolution;
                                itemm.BasicCalibrationResult.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;
                            }


                            if (itemm.TestPoint != null && itemm.TestPoint.IsDescendant)
                            {
                                HasDescendants = true;
                                break;
                            }

                        }


                        SortField = RT.GetPropertyName(() => Linearity.BasicCalibrationResult.Position);
                        
                    foreach (var itemm in listlienarity)
                        {

                            var iu = itemm as CalibrationItem;
                            RT.SaveNewItem(iu, "asc", SortField);

                        }

                        //LIST = listlienarity;
                    }

                    if (!IsWeightLoaded)
                    {
                        //aTimer.Stop();
                        //aTimer.Start();
                        //IniCalculated = 2;
                        //await Calculate();
                    }

                }
                //////////////// new code end



        }


        


        public async Task ExecuteFormula()
        {

            try
            {


                await ShowProgress(false);
                //HasChange = false;
                var aa = RT.Items.ToList();
                int cont = 0;
                Console.WriteLine("OnUserFinish " + RT.Items.Count + " --- " + aa.Count);
                foreach (var item in aa)
                {
                    List<double> sequence = new List<double>();
                    sequence.Add(item.BasicCalibrationResult.Test1);
                    sequence.Add(item.BasicCalibrationResult.Test2);

                    item.BasicCalibrationResult.Average = sequence.Average();
                    item.BasicCalibrationResult.Repeateability = sequence.standardDeviation() / sequence.Count;

                    List<double> sequence2 = new List<double>();

                    for (int ii = 0; ii < sequence.Count; ii++)
                    {
                        sequence2.Add(Math.Abs(item.BasicCalibrationResult.Nominal - sequence[ii]));
                    }

                    item.BasicCalibrationResult.Error = sequence2.Max();


                    RT.Items[cont] = item;
                    cont++;
                }
                LIST = RT.Items;
                RT.ItemList = RT.Items;
                RT.ItemsDataSource = RT.Items;

                
                //HasChange = true;
                //aTimer2.Start();
                StateHasChanged();

            }
            catch (Exception ex)
            {
                Console.WriteLine("error onuserfinish " + ex.Message);
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

       



        [Parameter]
        public string HideElement { get; set; } = "modalLinearity";

        [CascadingParameter] public IModalService Modal { get; set; }
        public string _message { get; set; }
        //Logger.LogDebug(Group.Name);
        public async Task SelectWeight(CalibrationItem item, int position)
        {



            var _poeGrpc = new PieceOfEquipmentGRPC(_poeServices);

            var g = new CallOptions(await GetHeaderAsync());

            var ss = item as RockwellResult;

            var resultado = (await _poeGrpc.GetPieceOfEquipmentByScale(item.BasicCalibrationResult.ScaleRange, Header));


            WeightSetList2 = new List<WeightSet>();


            int contu = 1;
            foreach(var item1 in resultado)
            {
                WeightSet wsi = new WeightSet();

                wsi.PieceOfEquipmentID = item1.PieceOfEquipmentID;
                wsi.WeightNominalValue = item1.Capacity;
                wsi.UnitOfMeasureID = item1.UnitOfMeasureID.Value;
                if (wsi.WeightSetID == 0)
                {
                    wsi.WeightSetID = contu;
                    contu++;
                }
               
                WeightSetList2.Add(wsi);

                
            }

            var SelWeightSetList = new List<WeightSet>();

            if (item?.Standards?.Count > 0)
            {
                foreach (var item1 in item.Standards)
                {

                    WeightSet wsi = new WeightSet();

                    wsi.PieceOfEquipmentID = item1.PieceOfEquipmentID;

                    var item2 = WeightSetList2.Where(x=>x.PieceOfEquipmentID== item1.PieceOfEquipmentID).FirstOrDefault();

                    wsi.WeightNominalValue = item2.WeightNominalValue;
                    wsi.UnitOfMeasureID = item2.UnitOfMeasureID;
                    wsi.WeightSetID = item2.WeightSetID;

                    SelWeightSetList.Add(wsi);
                }
            }


            var parameters = new ModalParameters();


            parameters.Add("target", item);

            parameters.Add("DataSource", WeightSetList2);

            parameters.Add("TestPoint", item.TestPoint);

            
          

            if (item.WeightSets != null)
            {
                foreach (var iw in item.WeightSets)
                {

                    
                    SelWeightSetList.Add(iw);


                }
            }



            parameters.Add("SelectWeight", SelWeightSetList);


            ModalOptions op = new ModalOptions();

           
            op.Position = ModalPosition.Center;

           

            op.Class = "blazored-modal " + ModalSize.MediumWindow;  //blazored-modal blazored-modal-scrollable allWindow

            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
            }

            var messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell.WeightComponent<CalibrationItem>),"Select " + item.BasicCalibrationResult.ScaleRange , parameters, op);

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

            List<WeightSet> WeightSetList = (List<WeightSet>)r.WeightSetList;

            List<WeightSet> RemoveList = (List<WeightSet>)r.RemoveList;

            double CalResult = r.CalculateWeight;

            Console.WriteLine("select 2");

           
            item.BasicCalibrationResult.Nominal= CalResult;

            if(WeightSetList?.Count > 0)
            {
                var poeselect = resultado.Where(x => x.PieceOfEquipmentID == WeightSetList[0].PieceOfEquipmentID).FirstOrDefault();


                double max = 0;
                double min = 0;
                if (WorkOrderItemCreate.eq.CertificationID==1)
                {
                    max = CalResult + poeselect.ToleranceValueISO;

                    min = CalResult - poeselect.ToleranceValueISO;
                }
                else
                {
                   max   = CalResult + poeselect.ToleranceValue;

                    min  = CalResult - poeselect.ToleranceValue;
                }

             

                item.BasicCalibrationResult.Max = max;

                item.BasicCalibrationResult.Min = min;

                item.BasicCalibrationResult.Uncertanty = poeselect.UncertaintyValue;


                //Only one
                if (item.Standards == null)
                {
                    item.Standards= new List<CalibrationSubType_Standard>();  
                }
                item.Standards.Clear();

                CalibrationSubType_Standard cs = new CalibrationSubType_Standard();
                cs.CalibrationSubTypeID = item.CalibrationSubTypeId;
                cs.PieceOfEquipmentID = poeselect.PieceOfEquipmentID;
                cs.WorkOrderDetailID = item.WorkOrderDetailId;
                cs.SecuenceID = item.SequenceID;               
                item.Standards.Add(cs);


            }
            else
            {
                item.BasicCalibrationResult.Max = 0;

                item.BasicCalibrationResult.Min = 0;

                item.BasicCalibrationResult.Uncertanty = 0;
                //Only one
            }

            if (CalResult == 0)
            {
                foreach (var item23 in RT.Items)
                {
                    if(item23.WeightSets != null)
                    {
                        item23.WeightSets.Clear();
                    }
                    
                }
            }

            if (WeightSetList != null)
            {
                Calculate cal = new Calculate();

                Console.WriteLine("select 4");
                //return;
               
                int cont = position;//RT.CurrentSelectIndex;

                if (cont < 0)
                {
                    cont = 0;
                }

                //for (int i = cont; i < RT.Items.Count; i++)
                //{
                    int i = cont;
                   
                    if (RT.Items[i].WeightSets == null)
                    {
                        RT.Items[i].WeightSets = new List<WeightSet>();
                    }
                    foreach (var ws in WeightSetList)
                    {
                        var result11 = RT.Items[i].WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                        if (result11 == null)
                        {   Console.WriteLine("select 44");
                            RT.Items[i].WeightSets.Add(ws);
                        }


                    }
                //}


           
               
            }
                 
            if(RemoveList != null)
            {
                Console.WriteLine("select 3");
                 foreach (var ws in RemoveList)
                {
                    var result11 = item.WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                    if (result11 != null)
                    {Console.WriteLine("select 33");
                        item.WeightSets.Remove(result11);
                    }
                }
            }




             //for (int i = cont; i < RT.Items.Count; i++)
                //{
                //    double suma = 0;
                   
                //    if (RT.Items[i].WeightSets != null)
                //    {
                //        foreach (var ws in RT.Items[i].WeightSets)
                //        {
                //            suma = suma + ws.WeightNominalValue;
                //        }


                //        //RT.Items[i].BasicCalibrationResult.Nominal = suma;
                //    }
                //    else
                //    {
                //        //RT.Items[i].BasicCalibrationResult.Nominal = 0;
                //    }


                //}
               
                /////////////////////////////////////////////////////////////////////////////////////////////////////

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

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
using Newtonsoft.Json;
using LinqKit;
using Blazed.Controls.Toast;
using System.Drawing;
using Helpers;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public class ASTMForceComponentBase : CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<WorkOrderDetail>
    {

        public bool ShowGrid { get; set; }

        [Parameter]
        public bool HasCopyButton { get; set; } = false;
        [Inject] public Application.Services.IPieceOfEquipmentService<CallContext> _poeServices { get; set; }

        [CascadingParameter(Name = "CascadeParam1")]
        public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

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
        public static List<Domain.Aggregates.Entities.Force> listlienarity { get; set; } = new List<Domain.Aggregates.Entities.Force>();
        //[Inject] public Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }


        [Inject]
        public Func<dynamic, Application.Services.IWorkOrderDetailServices<CallContext>> Service { get; set; }
        
        [Inject] 
        IConfiguration Configuration { get; set; }
        public string url { get; set; }


        //[Inject] public Application.Services.IWorkOrderDetailServices<CallContext> Service { get; set; }

        //[Inject]
        //public Func<IIndexedDbFactory, Application.Services.IUOMService<CallContext>> _UOMServices { get; set; }

        //[Inject] public Application.Services.IUOMService<CallContext> _UOMServices { get; set; }


        [Parameter]
        public ResponsiveTable<Force> RT { get; set; } = new ResponsiveTable<Force>();

        public string SortField { get; set; }

        public bool HasDescendants { get; set; }

        public int MyProperty { get; set; }

        //public Force RowAfterRender(Force lin)
        //{

           
        //    IniCalculated = 1;
           

        //    return lin;

        //}

        

        public Force RowAfterRender(Force lin)
        {

            
           
            var srt = RT.Items.Where(x => x.SequenceID == lin.SequenceID).ToList();

            if (srt.Count > 1)
            {

                HasChange = true;
                lin.SequenceID =  RT.Items.MaxBy(x => x.SequenceID).SequenceID + 1;
                lin.BasicCalibrationResult.SequenceID = lin.SequenceID;

                //TotalForcesCompt.Add(lin);

                int cont1 = 1;
                foreach (var itemn in RT.Items)
                {
                   
                    itemn.BasicCalibrationResult.Position = cont1;
                    cont1++;
                }

            }

            IniCalculated = 1;
           

            return lin;

        }

        

        public Force RowSave(Force lin)
        {
            



            return lin;

        
        }



            public Force RowChange(Force lin)
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

        public Force _Linearity { get; set; }


        public bool IsAddLinearity { get; set; }

        public async Task TaskAfterSave(Force item, string sav)
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

            HasChange= true;

        }

        public bool IsNewItem { get; set; }
        public Force DefaultNew()
        {
            Force lin = new Force();

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

         



            lin.BasicCalibrationResult = new ForceResult();
            lin.TestPoint = new TestPoint();
            lin.WeightSets = new List<WeightSet>();



           

            lin.TestPoint.UnitOfMeasurementID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;

            lin.TestPoint.UnitOfMeasurementOutID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;

            lin.TestPoint.Resolution = WorkOrderItemCreate.eq.Resolution;

            lin.TestPoint.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;

            

            lin.TestPoint.CalibrationType = LTILogic.GetCalibrationSubTypeStr(WorkOrderItemCreate.eq,IsCompresion);

            lin.TestPoint.TestPointTarget = 1;

            //lin.BasicCalibrationResult.WeightApplied = 0;

            //lin.BasicCalibrationResult.AsFound = 0;

            //lin.BasicCalibrationResult.AsFound = 0;


            //lin.BasicCalibrationResult.UnitOfMeasureID = lin.TestPoint.UnitOfMeasurementOutID;


            lin.BasicCalibrationResult.CalibrationSubTypeId = LTILogic.GetCalibrationSubType(WorkOrderItemCreate.eq,IsCompresion);
                        

            lin.BasicCalibrationResult.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;

            lin.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;

            lin.BasicCalibrationResult.SequenceID = RT.Items.MaxBy(x => x.SequenceID).SequenceID + 1; 

            lin.SequenceID = lin.BasicCalibrationResult.SequenceID; 

            lin.UnitOfMeasureId = lin.TestPoint.UnitOfMeasurementID;

            

            lin.BasicCalibrationResult.FS = result;

            lin.BasicCalibrationResult.Nominal = Math.Round((result / 100) * WorkOrderItemCreate.eq.PieceOfEquipment.Capacity);


            lin.CalibrationSubTypeId = lin.BasicCalibrationResult.CalibrationSubTypeId;
         
            IsNewItem = true;

            lin.TestPoint = null;

            return lin;

        }


        //public async Task ChangeControl2(ChangeEventArgs arg, Force lin, bool AddWeight = false)
        //{
        //    try
        //    {
        //        if (arg == null || arg.Value.ToString() == "")
        //        {
        //            return;
        //        }


        //        var result = Convert.ToDouble(arg.Value);

        //        if (AddWeight)
        //        {
        //            AddWeightset(lin, result);
        //        }

        //        // remove previous one
        //        aTimer.Stop();
        //        // new timer
        //        aTimer.Start();





        //    }
        //    catch
        //    {

        //    }



        //}

        public async Task ChangeControl2(ChangeEventArgs arg, Force lin, bool AddWeight = false)
        {
            try
            {
                //LIST = RT.Items;
                HasChange = true;

                aTimer.Stop();


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

                // new timer
                aTimer.Start();
                //IniCalculated = 1;
                //if (IniCalculated == 2)
                //{

                //    await Calculate();


                //    IniCalculated = 1;
                //}





            }
            catch
            {

            }



        }

        public bool HasChange { get; set; } = false;

        public bool HasCopy { get; set; } = false;

        public async Task ChangeControlFS(ChangeEventArgs arg, Force lin, bool AddWeight = false)
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


                HasChange=true; 

                // remove previous one
                aTimer.Stop();
                // new timer
                aTimer.Start();


            }
            catch
            {

            }



        }

        public int IniCalculated { get; set; }
        private static System.Timers.Timer aTimer;

        public void AddWeightset(Force lin, double result = 0)
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


        public async Task ChangeControl(ChangeEventArgs arg, Force lin)
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

        public List<Force> MemoryGrid { get; set; } = new List<Force>();


        public string BoudValues(Force lin)
        {



            return "";

        }

        [Parameter]
        public bool Accredited { get; set; }

        public bool IsWeightLoaded { get; set; }

        //[Inject]
        public CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext> Client { get; set; }

        public async Task Calculate()
        {
            return;
            int line = 0;
            try
            {
                //return;
                //Console.WriteLine("calculate");
                IsNewItem = false;

                if (RT == null || RT.Items == null || RT.Items.Count == 0)
                {
                    return;
                }
                line++;
                var linearities = RT.Items;
                
               

                if (!IsWeightLoaded)
                {
                    line++;
                    var p = new CallOptions();
                    
                    WorkOrderDetailGrpc wd = new WorkOrderDetailGrpc(Service, DbFactory, p);
                    
                    //if(eq.BalanceAndScaleCalibration== null)
                    //{
                    //    line++;
                    //    eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
                    //}
                    line++;
                    //eq.BalanceAndScaleCalibration.Forces = linearities;//eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == LTILogic.GetCalibrationSubType(eq, IsCompresion)).ToArray();
                    line++;
                    //var resul = await wd.GetConfiguredWeights(eq);
                    line++;
                    //eq.BalanceAndScaleCalibration = resul.BalanceAndScaleCalibration;
                    line++;
                    IsWeightLoaded = true;
                }



                 var result = 1;// LTILogic.GetCalculatesForISOandASTM( (eq).GetAwaiter().GetResult();
                //var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

                if (result != null)
                {
                    //eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;await _poeServices.GetCalculatesForISOandASTM(linearities, new CallContext());//yppp 06062022 

                    RT.Clear();

                    line++;

                    var res1 = LTILogic.GetCalculatesForISOandASTM(linearities, WorkOrderItemCreate.eq.GetCalibrationTypeID());
                    line++;
                    foreach (var item in res1)
                    {
                        //Console.WriteLine("calculate iisisi");
                        //Console.WriteLine(item.BasicCalibrationResult.RUN1);
                        RT.SaveNewItem(item,null,null,false);
                        //Console.WriteLine("calculate iisisi2");

                    }
                    line++;
                }
                StateHasChanged();

            }
            catch (Exception ex)
            {

                await ExceptionManager(ex, "Claculateastm :" + line + " " );
            }
            finally
            {
                IsNewItem = false;
            }



        }

////        public async Task Calculate2()
////        {
           
////            int line = 0;
////            try
////            {
////                //return;
////                Console.WriteLine("calculate");
////                IsNewItem = false;

////                if (RT == null || RT.Items == null || RT.Items.Count == 0)
////                {
////                    return;
////                }
////                line++;
////                var linearities = RT.Items;
                
////                //if (eq?.BalanceAndScaleCalibration == null)
////                //{
////                //    eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();

////                //    eq.BalanceAndScaleCalibration.Forces = linearities;
////                //}

////                //else
////                //{
////                //    var lsttemp = new List<Force>();

////                //    foreach (var item in  eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == LTILogic.GetCalibrationSubType(eq, IsCompresion)))
////                //    {
////                //        lsttemp.Add(item);
////                //    }
                        
                       
////                //   foreach(var item in  lsttemp)
////                //    {
////                //        eq.BalanceAndScaleCalibration.Forces.Remove(item);
////                //    }
////                //    foreach (var item in linearities)
////                //    {
////                //        eq.BalanceAndScaleCalibration.Forces.Add(item);
////                //    }
                    

////                //}



////                if (!IsWeightLoaded)
////                {
////                    line++;
////                    var p = new CallOptions();
                    
////                    WorkOrderDetailGrpc wd = new WorkOrderDetailGrpc(Service, DbFactory, p);

////                    //if(eq.BalanceAndScaleCalibration== null)
////                    //{
////                    //    line++;
////                    //    eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
////                    //}
////                    line++;
////                    //eq.BalanceAndScaleCalibration.Forces = linearities;//eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == LTILogic.GetCalibrationSubType(eq, IsCompresion)).ToArray();
////                    line++;
////                    //var resul = await wd.GetConfiguredWeights(eq);
////                    line++;
////                    //eq.BalanceAndScaleCalibration = resul.BalanceAndScaleCalibration;
////                    line++;
////                    IsWeightLoaded = true;
////                }


//////                var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

//////                if (result != null)
//////                {
//////                    eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

//////                    RT.Clear();

//////                    foreach (var item in eq.BalanceAndScaleCalibration.Forces)
//////                    {
//////#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
//////                        RT.SaveNewItem(item);
//////#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
//////                    }

//////                }

////                 var result = 1;// LTILogic.GetCalculatesForISOandASTM( (eq).GetAwaiter().GetResult();
////                //var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

////                if (result != null)
////                {
////                    //eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

////                    RT.Clear();

////                    line++;
////                    var dto = new ISOandASTM();
////                    dto.Forces = linearities;
////                    dto.WorkOrderDetail = WorkOrderItemCreate.eq;
////                    var res1 = await _poeServices.GetCalculatesForISOandASTM(dto, new CallContext());//yppp 06062022 LTILogic.GetCalculatesForISOandASTM(linearities, eq.CalibrationTypeID);
////                    line++;
////                    foreach (var item in res1)
////                    {
////                        Console.WriteLine("calculate iisisi");
////                        Console.WriteLine(item.BasicCalibrationResult.RUN1);
////                        var reso = item.BasicCalibrationResult.Uncertanty.ToString();

////                        var result2 = reso.RoundedFunction("0.01", 2, RoundType.RoundToResolution, "System.Double");

////                        item.BasicCalibrationResult.Uncertanty = Convert.ToDouble(result2);

////                        RT.SaveNewItem(item,null,null,false);
////                        Console.WriteLine("calculate iisisi2");

////                    }
////                    line++;
////                }
////                StateHasChanged();

////            }
////            catch (Exception ex)
////            {

////                await ShowError("Claculateastm :" + line + " " + ex.Message);
////            }
////            finally
////            {
////                IsNewItem = false;
////            }



////        }

        public List<Force> TotalForcesCompt { get; set; }


        public async Task CopyToCompression()
        {

            try
            {
                if (1 == 1)
                {

                    var resp = await ConfirmAsync("Copy to compression deletes compression data, are you sure?");

                    if (!resp)
                    {
                        return;
                    }


                    TotalForcesCompt = new List<Force>();

                    List<Force> TotalForces = new List<Force>();
                    List<Force> TotalForcesComp = new List<Force>();
                    //compresion
                    if (WorkOrderItemCreate.eq?.BalanceAndScaleCalibration?.Forces != null)
                    {
                        TotalForcesComp = WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == 7).ToList();
                    }

                    //Tension
                    TotalForces = RT.Items.ToList();//WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == 4).ToList();

                    if (TotalForcesComp != null && TotalForcesComp.Count > 0)
                    {
                        foreach (var item in TotalForcesComp)
                        {
                            WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Remove(item);
                        }
                    }


                    //List<Force> TotalForcesCompt = new List<Force>();

                    int cont = 1;
                    foreach (var item in TotalForces)
                    {

                        //var clonedJson = JsonConvert.SerializeObject(item);
                        var resultCloned = new Force();// JsonConvert.DeserializeObject<Force>(clonedJson);
                        resultCloned.BasicCalibrationResult = new ForceResult();
                        resultCloned.CalibrationSubTypeId = 7;
                        resultCloned.BasicCalibrationResult.CalibrationSubTypeId = 7;
                        resultCloned.BasicCalibrationResult.Position = item.BasicCalibrationResult.Position;
                        resultCloned.SequenceID = cont;
                        resultCloned.BasicCalibrationResult.SequenceID = cont;
                        resultCloned.WorkOrderDetailId = item.WorkOrderDetailId;
                        resultCloned.BasicCalibrationResult.WorkOrderDetailId = item.WorkOrderDetailId;

                        //var TotalForceComp1 = TotalForcesComp.Where(x => x.BasicCalibrationResult.SequenceID == resultCloned.BasicCalibrationResult.SequenceID).FirstOrDefault();

                        resultCloned.BasicCalibrationResult.FS = item.BasicCalibrationResult.FS;
                        resultCloned.BasicCalibrationResult.Nominal = item.BasicCalibrationResult.Nominal;
                        resultCloned.BasicCalibrationResult.Resolution = item.BasicCalibrationResult.Resolution;
                        if (item.CalibrationSubType_Weights != null)
                        {
                            foreach (var cs in item.CalibrationSubType_Weights)
                            {
                                cs.CalibrationSubTypeID = 7;

                            }
                        }                        

                        resultCloned.CalibrationSubType_Weights = item.CalibrationSubType_Weights;

                        //foreach (var cs in item.WeightSets)
                        //{
                        //    cs. = 7;

                        //}

                        resultCloned.WeightSets = item.WeightSets;




                        TotalForcesCompt.Add(resultCloned);
                        cont++;
                        //WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Add(resultCloned);
                    }


                    if (WorkOrderItemCreate.eq?.BalanceAndScaleCalibration?.Forces == null)
                    {
                        WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces = new List<Force>();
                    }

                    foreach (var item in TotalForcesCompt)
                    {
                        WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Add(item);
                    }
                    HasCopy = true;
                    await ShowToast("Values copied to Compression.", ToastLevel.Success);


                    //////////////// new code end
                }
                ////////////////// new code end
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                await ShowError("Error Copy the Items");
            }

        }
        


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task Assing()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            if (WeightSetComponent == null || WeightSetComponent?.lstWeightSet == null)
            {
                return;
            }
            //weightsets
            var a = WeightSetComponent?.lstWeightSet;//WeightSetComponent.Grid.Items;

            var b = RT.Items; // LIST;

            //var c = b.Select(x => x.TestPoint);

            Querys.AssingAlgoritm(a, b, AppState.UnitofMeasureList);


            //LIST = b;

            foreach (var item in RT.Items)
            {
                //item.BasicCalibrationResult.WeightApplied = item.CalculateWeightValue;
                //Console.WriteLine(item.CalculateWeightValue);
            }


            StateHasChanged();


        }

        List<WeightSet> Selected = new List<WeightSet>();




       


        public Force Linearity = new Force();

        [Inject] public AppStateCompany AppState { get; set; }

        [Parameter]
        public Domain.Aggregates.Entities.Status CurrentStatus { get; set; } = new Domain.Aggregates.Entities.Status();

        public EditContext editContext { get; set; }

        //[Parameter]
        //public WorkOrderDetail eq { get; set; } = new WorkOrderDetail();


        public List<Domain.Aggregates.Entities.Force> LIST { get; set; } = new List<Domain.Aggregates.Entities.Force>();


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
            //Console.WriteLine("OnAfterRenderAsync");
            RefreshResolution = WorkOrderItemCreate.ChangeResolution;
            if (RT != null)
            {
                RT.ShowControl = true;
                RT.ShowLabel = false;
            }

            //await base.OnAfterRenderAsync(firstRender);

            var a = Enabled;

            try
            {



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


                if (FirstRender && WorkOrderItemCreate.eq != null && WorkOrderItemCreate.eq.WOD_Weights != null)
                {

                    foreach (var w in WorkOrderItemCreate.eq.WOD_Weights)
                    {
                        if (w.WeightSet != null)
                        {
                            WeightSetList2.Add(w.WeightSet);
                        }

                    }


                    if (WeightSetComponent != null && WeightSetList2 != null)
                    {


                        await WeightSetComponent.Show(WeightSetList2);

                    }


                }


                //Console.WriteLine("RefreshResolution " + RefreshResolution);


            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                //Console.WriteLine("Exception ex " + ex.StackTrace);
            }
            finally
            {

            }
        }





        protected override async Task OnInitializedAsync()

        {

            try
            {
                await ShowProgress();

                url = Configuration.GetSection("Reports")["URL"];

                Linearity.TestPoint = new TestPoint();

                editContext = new EditContext(Linearity);

                aTimer = new System.Timers.Timer(300);

                aTimer.AutoReset = false;

                aTimer.Elapsed += OnUserFinish;

                await base.OnInitializedAsync();

                //Console.WriteLine("Refreshresolution");
                //Console.WriteLine(RefreshResolution);
                if (WorkOrderItemCreate.eq.GetCalibrationTypeID().HasValue && LTILogic.ValidateForceList(WorkOrderItemCreate.eq, IsCompresion, 0))
                {
                    //Console.WriteLine("GetForceList");

                    var tableList = LTILogic.GetForceList(WorkOrderItemCreate.eq, IsCompresion);

                    RT.Clear();

                    SortField = RT.GetPropertyName(() => Linearity.BasicCalibrationResult.Position);
                    foreach (var itemm in tableList)
                    {

                        if (itemm.BasicCalibrationResult.Resolution == 0)
                        {
                            itemm.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Resolution;
                            itemm.BasicCalibrationResult.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;
                        }

#pragma warning disable
                        //RT.SaveNewItem(itemm, "asc", SortField);
                        LIST = tableList.OrderBy(x => x.BasicCalibrationResult.Position).ToList();
                        RT.ItemList = tableList;
                        RT.ItemsDataSource = tableList;

                        ShowGrid = true;
#pragma warning restore

                    }

                    aTimer.Stop();

                    aTimer.Start();



                }

                else

                    /////NEw CODE
                    if (WorkOrderItemCreate.eq.GetCalibrationTypeID().HasValue && !LTILogic.ValidateForceList(WorkOrderItemCreate.eq, IsCompresion, 0))
                {
                    //Console.WriteLine("CreateForceList");
                    //Console.WriteLine(IsCompresion);
                    double sumRuns = 0;

                    List<Force> lst1;
                    if (WorkOrderItemCreate.eq.Refresh == true)
                    {
                        lst1 = LTILogic.GetForceList(WorkOrderItemCreate.eq, IsCompresion);
                    }
                    else
                    {
                        lst1 = LTILogic.CreateForceList(WorkOrderItemCreate.eq, IsCompresion);
                    }
                    listlienarity = lst1;//LTILogic.CreateForceList(WorkOrderItemCreate.eq, IsCompresion);

                    if (listlienarity != null)
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

                        LIST = listlienarity.OrderBy(x => x.BasicCalibrationResult.Position).ToList();
                        RT.ItemList = listlienarity;
                        RT.ItemsDataSource = listlienarity;
                        //foreach (var itemm in listlienarity)
                        //{

                        //    //RT.SaveNewItem(itemm, "asc", SortField);

                        //}
                        ShowGrid = true;

                    }


                }








            }
            catch (Exception ex) 
            { 
            
                await ExceptionManager(ex);
            
            
            }
            finally
            {
                WorkOrderItemCreate.eq.Refresh = false;
                await CloseProgress();  
            }

           


        }


        private void OnUserFinish(Object source, ElapsedEventArgs e)
        {
                        
                
                aTimer.Stop();

                List<Force> a = new List<Force>();

           


                InvokeAsync(async () =>
           {

               await ExecuteFormula();

           });

               

        }


        public async Task ExecuteFormula()
        {
            try
            {


                await ShowProgress();
                //HasChange = false;
                var dto = new ISOandASTM();
                dto.Forces = RT.Items.Where(x => x.CalibrationSubTypeId == IsCompresion).OrderBy(x => x.BasicCalibrationResult.Position).ToList();
                dto.WorkOrderDetail = WorkOrderItemCreate.eq;


                var aa = await _poeServices.GetCalculatesForISOandASTM(dto, new CallContext());
                int cont = 0;
                //Console.WriteLine("OnUserFinish " + RT.Items.Count + " --- " + aa.Count);
                foreach (var item in aa.Where(x => x.CalibrationSubTypeId == IsCompresion).OrderBy(x => x.BasicCalibrationResult.Position))
                {


                    RT.Items[cont] = item;
                    cont++;
                }
                LIST = RT.Items.Where(x => x.CalibrationSubTypeId == IsCompresion).OrderBy(x => x.BasicCalibrationResult.Position).ToList();
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

      


        [Parameter]
        public string HideElement { get; set; } = "modalLinearity";

        [CascadingParameter] public IModalService Modal { get; set; }
        public string _message { get; set; }
        //Logger.LogDebug(Group.Name);
        //public async Task SelectWeight(Force item, int position)
        //{



        //    if (item == null || WorkOrderItemCreate.eq?.WOD_Weights == null)
        //    {
        //        return;
        //    }



        //    var parameters = new ModalParameters();


        //    WeightSetList2 = new List<WeightSet>();
        //    foreach (var w in WorkOrderItemCreate.eq.WOD_Weights)
        //    {
        //        if(item.BasicCalibrationResult.Nominal >= w.WeightSet.WeightNominalValue)
        //        {
        //             WeightSetList2.Add(w.WeightSet);
        //        }

        //    }

        //    parameters.Add("target", item);

        //    parameters.Add("DataSource", WeightSetList2);

        //    parameters.Add("TestPoint", item.TestPoint);

        //    var SelWeightSetList = new List<WeightSet>();


        //    if (item.WeightSets != null)
        //    {
        //        foreach (var iw in item.WeightSets)
        //        {


        //            SelWeightSetList.Add(iw);


        //        }
        //    }



        //    parameters.Add("SelectWeight", SelWeightSetList);


        //    ModalOptions op = new ModalOptions();


        //    op.Position = ModalPosition.Center;



        //    op.Class = "blazored-modal " + ModalSize.MediumWindow;  //blazored-modal blazored-modal-scrollable allWindow

        //    if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
        //    {
        //        await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
        //    }

        //    var messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Force>),"Select " + item.BasicCalibrationResult.FS + "%", parameters, op);

        //    Console.WriteLine("select 1");
        //    var result = await messageForm.Result;

        //    if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
        //    {
        //        await JSRuntime.InvokeVoidAsync("showElement", HideElement);
        //    }

        //    if (result.Cancelled)
        //    {
        //        return;
        //    }
        //    _message = result.Data?.ToString() ?? string.Empty;


        //    WeightSetResult r = (WeightSetResult)result.Data;

        //    List<WeightSet> WeightSetList = (List<WeightSet>)r.WeightSetList;

        //    List<WeightSet> RemoveList = (List<WeightSet>)r.RemoveList;

        //    double CalResult = r.CalculateWeight;

        //    Console.WriteLine("select 2");





        //    if (WeightSetList != null)
        //    {
        //        Calculate cal = new Calculate();

        //        Console.WriteLine("select 4");
        //        //return;

        //        int cont = position;//RT.CurrentSelectIndex;

        //        if (cont < 0)
        //        {
        //            cont = 0;
        //        }

        //        //for (int i = cont; i < RT.Items.Count; i++)
        //        //{
        //            int i = cont;

        //            if (RT.Items[i].WeightSets == null)
        //            {
        //                RT.Items[i].WeightSets = new List<WeightSet>();
        //            }
        //            foreach (var ws in WeightSetList)
        //            {
        //                var result11 = RT.Items[i].WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
        //                if (result11 == null)
        //                {   Console.WriteLine("select 44");
        //                    RT.Items[i].WeightSets.Add(ws);
        //                RT.Items[i].BasicCalibrationResult.Resolution = ws.Resolution;
        //            }


        //            }
        //        //}




        //    }

        //    if(RemoveList != null)
        //    {
        //        Console.WriteLine("select 3");
        //         foreach (var ws in RemoveList)
        //        {
        //            var result11 = item.WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
        //            if (result11 != null)
        //            {Console.WriteLine("select 33");
        //                item.WeightSets.Remove(result11);
        //            }
        //        }
        //    }

        //     //for (int i = cont; i < RT.Items.Count; i++)
        //        //{
        //        //    double suma = 0;

        //        //    if (RT.Items[i].WeightSets != null)
        //        //    {
        //        //        foreach (var ws in RT.Items[i].WeightSets)
        //        //        {
        //        //            suma = suma + ws.WeightNominalValue;
        //        //        }


        //        //        //RT.Items[i].BasicCalibrationResult.Nominal = suma;
        //        //    }
        //        //    else
        //        //    {
        //        //        //RT.Items[i].BasicCalibrationResult.Nominal = 0;
        //        //    }


        //        //}

        //        /////////////////////////////////////////////////////////////////////////////////////////////////////

        //}


        public async Task SelectWeight(Force item, int position)
        {

            //Console.WriteLine("SelectWeight  Position" + position);
            //if (item == null || WeightSetComponent == null || WeightSetComponent.Grid == null || WeightSetComponent.Grid.Items == null)
            //{
            //    return;
            //}

            var parameters = new ModalParameters();

            if (item == null || WorkOrderItemCreate.eq?.WOD_Weights == null)
            {
                return;
            }


            WeightSetList2 = new List<WeightSet>();
            foreach (var w in WorkOrderItemCreate.eq.WOD_Weights)
            {
                if (item.BasicCalibrationResult.Nominal >= w.WeightSet.WeightNominalValue && item.BasicCalibrationResult.Nominal <= w.WeightSet.WeightNominalValue2)
                {
                    WeightSetList2.Add(w.WeightSet);
                }

            }

            parameters.Add("target", item);

            parameters.Add("DataSource", WeightSetList2);

            parameters.Add("TestPoint", item.TestPoint);

            var SelWeightSetList = new List<WeightSet>();


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

            var messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Force>), "Select Standard", parameters, op);
            var result = await messageForm.Result;

            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("showElement", HideElement, Enabled);
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

            if (WeightSetList != null)
            {
                Calculate cal = new Calculate();

                //Console.WriteLine("select 4");
                //return;

                int cont = position;//RT.CurrentSelectIndex;

                if (cont < 0)
                {
                    cont = 0;
                }

                //for (int i = cont; i < RT.Items.Count; i++)
                //{
                int i = cont;
                RT.Items[i].WeightSets = null;
                if (RT.Items[i].WeightSets == null)
                {
                    RT.Items[i].WeightSets = new List<WeightSet>();
                }
                foreach (var ws in WeightSetList)
                {
                    var result11 = RT.Items[i].WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                    if (result11 == null)
                    {
                        //Console.WriteLine("select 44");
                        RT.Items[i].WeightSets.Add(ws);
                        if (ws.Resolution != 0)
                            RT.Items[i].BasicCalibrationResult.Resolution = ws.Resolution;
                    }


                }
                //}




            }

            if (RemoveList != null)
            {
                //Console.WriteLine("select 3");
                foreach (var ws in RemoveList)
                {
                    var result11 = item.WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                    if (result11 != null)
                    {
                        //Console.WriteLine("select 33");
                        item.WeightSets.Remove(result11);
                    }
                }
            }

            HasChange = true;
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

            var w = ((PieceOfEquipment)arg.Value).WeightSets;
            WeightSetList2 = w.ToList();

            if (WeightSetComponent != null && WeightSetList2 != null)
            {


                await WeightSetComponent.Show(w.ToList());

            }

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

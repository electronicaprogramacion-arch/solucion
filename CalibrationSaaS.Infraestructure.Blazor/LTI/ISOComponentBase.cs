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
using Helpers;
using Helpers;
using Newtonsoft.Json;
using Blazed.Controls.Toast;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System.Security.Cryptography;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI
{
    public class ISOComponentBase : CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<WorkOrderDetail>
    {
        [Parameter]
        public bool HasCopyButton { get; set; } = false;

        public bool HasChange { get; set; } = false;

        public bool HasCopy { get; set; } = false;

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

        //[Inject] public Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }


        [Inject]
        public Application.Services.IWorkOrderDetailServices<CallContext> Service { get; set; }

        [Inject] IConfiguration Configuration { get; set; }
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

        public Force RowAfterRender(Force lin)
        {

            

            var srt = RT.Items.Where(x => x.SequenceID == lin.SequenceID).ToList();

            if (srt.Count > 1)
            {
                HasChange = true;
                //TotalForcesCompt.Add(lin);
                lin.SequenceID = RT.Items.MaxBy(x => x.SequenceID).SequenceID + 1;
                lin.BasicCalibrationResult.SequenceID = lin.SequenceID;
                int cont1 = 1;
                foreach (var itemn in RT.Items)
                {
                    //if(itemn.TestPoint == null)
                    //{
                    //    itemn.TestPoint = new TestPoint();
                    //}
                    itemn.BasicCalibrationResult.Position = cont1;
                    cont1++;
                }

            }

            IniCalculated = 1;


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

            HasChange = true;

        }



        public Force RowSave(Force lin)
        {
            
           


            return lin;

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

            //if (RT.Items != null)
            //{
            //    foreach (var item in RT.Items)
            //    {
            //        if (result == item?.TestPoint?.NominalTestPoit)
            //        {

            //            return null;


            //        }
            //    }
            //}



            lin.BasicCalibrationResult = new ForceResult();
            lin.TestPoint = new TestPoint();
            lin.WeightSets = new List<WeightSet>();



             

            lin.TestPoint.UnitOfMeasurementID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;

            lin.TestPoint.UnitOfMeasurementOutID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;

            lin.TestPoint.Resolution = WorkOrderItemCreate.eq.Resolution;

            lin.TestPoint.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;

             

            lin.TestPoint.CalibrationType = LTILogic.GetCalibrationSubTypeStr(WorkOrderItemCreate.eq,IsCompresion);;

            lin.TestPoint.TestPointTarget = 1;

           


            lin.BasicCalibrationResult.CalibrationSubTypeId = LTILogic.GetCalibrationSubType(WorkOrderItemCreate.eq,IsCompresion);

            lin.BasicCalibrationResult.SequenceID = RT.Items.MaxBy(x => x.SequenceID).SequenceID + 1;

            lin.BasicCalibrationResult.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;

            lin.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;

            lin.SequenceID = lin.BasicCalibrationResult.SequenceID; 

            lin.UnitOfMeasureId = lin.TestPoint.UnitOfMeasurementID;

            lin.TestPoint.NominalTestPoit = result;

            lin.CalibrationSubTypeId = lin.BasicCalibrationResult.CalibrationSubTypeId;

            lin.TestPoint = null;

            IsNewItem = true;

            return lin;

        }


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

        public int IniCalculated { get; set; }
        private static System.Timers.Timer aTimer;
        private static System.Timers.Timer aTimer2;

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


//        public async Task ChangeControl(ChangeEventArgs arg, Force lin)
//        {

//            return;

//#pragma warning disable CS0162 // Se detectó código inaccesible
//            double result = 0;
//#pragma warning restore CS0162 // Se detectó código inaccesible
//            try
//            {

//                if (arg == null || arg.Value.ToString() == "")
//                {
//                    return;
//                }

//                result = Convert.ToDouble(arg.Value);


//                if (lin != null && lin?.WeightSets != null)
//                {
//                    lin.WeightSets.Clear();

//                }
//                else
//                {
//                    lin.WeightSets = new List<WeightSet>();
//                }

//                WeightSet w = new WeightSet();
//                w.WeightNominalValue = (int)result;
//                w.WeightActualValue = result;
//                w.PieceOfEquipmentId_new = lin.WorkOrderDetailId;
//                lin.WeightSets.Add(w);
//                lin.WeightSets = null;

//                //lin.BasicCalibrationResult.WeightApplied = result;

//                // remove previous one
//                aTimer.Stop();
//                // new timer
//                aTimer.Start();
//                //IniCalculated = 1;
//                if (IniCalculated == 2)
//                {

//                    await Calculate();


//                    IniCalculated = 1;
//                }




//            }
//#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
//            catch (Exception ex)
//#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
//            {

//            }

//        }

        public List<Force> MemoryGrid { get; set; } = new List<Force>();


        public string BoudValues(Force lin)
        {



            return "";

        }

        [Parameter]
        public bool Accredited { get; set; }

        public bool IsWeightLoaded { get; set; }

        //[Inject]
        //public CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext> Client { get; set; }

        //        public async Task Calculate()
        //        {
        //            try
        //            {
        //                //return;
        //                Console.WriteLine("calculate");
        //                IsNewItem = false;

        //                if (RT == null || RT.Items == null || RT.Items.Count == 0)
        //                {
        //                    return;
        //                }

        //                var linearities = RT.Items;
        //                //if (eq?.BalanceAndScaleCalibration == null)
        //                //{
        //                //    eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
        //                //}

        //                //eq.BalanceAndScaleCalibration.Forces = linearities;



        //                if (!IsWeightLoaded)
        //                {
        //                    var p = new CallOptions();

        //                    eq.BalanceAndScaleCalibration.Forces = eq.BalanceAndScaleCalibration
        //                        .Forces.Where(x => x.CalibrationSubTypeId == LTILogic.GetCalibrationSubType(eq, IsCompresion))
        //                        .ToArray();

        //                    WorkOrderDetailGrpc wd = new WorkOrderDetailGrpc(Service, DbFactory, p);

        //                    var resul = await wd.GetConfiguredWeights(eq);

        //                    eq.BalanceAndScaleCalibration = resul.BalanceAndScaleCalibration;

        //                    IsWeightLoaded = true;
        //                }


        ////                var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

        ////                if (result != null)
        ////                {
        ////                    eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

        ////                    RT.Clear();

        ////                    foreach (var item in eq.BalanceAndScaleCalibration.Forces)
        ////                    {
        ////#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
        ////                        RT.SaveNewItem(item);
        ////#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
        ////                    }

        ////                }

        //                 var result = 1;// LTILogic.GetCalculatesForISOandASTM( (eq).GetAwaiter().GetResult();
        //                //var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

        //                if (result != null)
        //                {
        //                    //eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

        //                    RT.Clear();



        //                    var res1 = LTILogic.GetCalculatesForISOandASTM(linearities);

        //                    foreach (var item in res1)
        //                    {
        //                        Console.WriteLine("calculate iisisi");
        //                        Console.WriteLine(item.BasicCalibrationResult.RUN1);
        //                        RT.SaveNewItem(item,null,null,false);
        //                        Console.WriteLine("calculate iisisi2");

        //                    }

        //                }
        //                StateHasChanged();

        //            }
        //            catch (Exception ex)
        //            {

        //                await ShowError("Claculate :" + ex.Message);
        //            }
        //            finally
        //            {
        //                IsNewItem = false;
        //            }



        //        }

        //  public async Task Calculate()
        //{

        //    int line = 0;
        //    try
        //    {
        //        //return;
        //        Console.WriteLine("calculate");
        //        IsNewItem = false;

        //        if (RT == null || RT.Items == null || RT.Items.Count == 0)
        //        {
        //            return;
        //        }
        //        line++;
        //        var linearities = RT.Items;



        //        if (!IsWeightLoaded)
        //        {
        //            line++;
        //            var p = new CallOptions();

        //            WorkOrderDetailGrpc wd = new WorkOrderDetailGrpc(Service, DbFactory, p);

        //            eq.BalanceAndScaleCalibration.Forces = eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == LTILogic.GetCalibrationSubType(eq, IsCompresion)).ToArray();
        //            line++;
        //            var resul = await wd.GetConfiguredWeights(eq);
        //            line++;
        //            eq.BalanceAndScaleCalibration = resul.BalanceAndScaleCalibration;
        //            line++;
        //            IsWeightLoaded = true;
        //        }




        //         var result = 1;// LTILogic.GetCalculatesForISOandASTM( (eq).GetAwaiter().GetResult();
        //        //var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

        //        if (result != null)
        //        {
        //            //eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

        //            RT.Clear();

        //            line++;

        //            var res1 = LTILogic.GetCalculatesForISOandASTM(linearities);
        //            line++;
        //            foreach (var item in res1)
        //            {
        //                Console.WriteLine("calculate iisisi");
        //                Console.WriteLine(item.BasicCalibrationResult.RUN1);
        //                RT.SaveNewItem(item,null,null,false);
        //                Console.WriteLine("calculate iisisi2");

        //            }
        //            line++;
        //        }
        //        StateHasChanged();

        //    }
        //    catch (Exception ex)
        //    {

        //        await ShowError("Claculateastm :" + line + " " + ex.Message);
        //    }
        //    finally
        //    {
        //        IsNewItem = false;
        //    }



        //}
         IEnumerable<Force> GetForcesAsync(ICollection<Force> linearities)
        {
             //var res1 = await _poeServices.GetCalculatesForISOandASTM(linearities, new CallContext()); // LTILogic.GetCalculatesForISOandASTM(linearities, eq.CalibrationTypeID);

            // Console.WriteLine("calculate");
            // Console.WriteLine(res1.Count);
            foreach (var item in linearities)
            {
                yield return item;
            }      

        }

      public async Task Calculate(ICollection<Force> res1)
        {

            int line = 0;
            try
            {
                //return;
               
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
                    
                    //WorkOrderDetailGrpc wd = new WorkOrderDetailGrpc(Service, DbFactory, p);

                   
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
                   
                    //var res1 = await _poeServices.GetCalculatesForISOandASTM(linearities, new CallContext()); // LTILogic.GetCalculatesForISOandASTM(linearities, eq.CalibrationTypeID);


                    RT.Clear();

                    line++;
                   
                    line++;
                    foreach (var item in GetForcesAsync(res1))
                    {
                        
                        //Console.WriteLine(item.BasicCalibrationResult.Nominal);
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

                await ShowError("calculate iso :" + line + " " + ex.Message);
            }
            finally
            {
                IsNewItem = false;
            }



        }

//         public async Task Calculate2()
//        {

//            int line = 0;
//            try
//            {
                
//                IsNewItem = false;

//                if (RT == null || RT.Items == null || RT.Items.Count == 0)
//                {
//                    return;
//                }
//                line++;
//                var linearities = RT.Items;
                
               


//                if (!IsWeightLoaded)
//                {
//                    line++;
//                    var p = new CallOptions();
                    
//                    //WorkOrderDetailGrpc wd = new WorkOrderDetailGrpc(Service, DbFactory, p);

//                    //if(eq.BalanceAndScaleCalibration== null)
//                    //{
//                    //    line++;
//                    //    eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
//                    //}
//                    line++;
//                    //eq.BalanceAndScaleCalibration.Forces = linearities;//eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == LTILogic.GetCalibrationSubType(eq, IsCompresion)).ToArray();
//                    line++;
//                    //var resul = await wd.GetConfiguredWeights(eq);
//                    line++;
//                    //eq.BalanceAndScaleCalibration = resul.BalanceAndScaleCalibration;
//                    line++;
//                    IsWeightLoaded = true;
//                }


////                var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

////                if (result != null)
////                {
////                    eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

////                    RT.Clear();

////                    foreach (var item in eq.BalanceAndScaleCalibration.Forces)
////                    {
////#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
////                        RT.SaveNewItem(item);
////#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
////                    }

////                }

//                 var result = 1;// LTILogic.GetCalculatesForISOandASTM( (eq).GetAwaiter().GetResult();
//                //var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

//                if (result != null)
//                {
//                    //eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;
//                    //await _poeServices.GetCalculatesForISOandASTM(linearities, new CallContext());//await _poeServices.GetCalculatesForISOandASTM(linearities, new CallContext());//
//                     var res1 = await _poeServices.GetCalculatesForISOandASTM(linearities, new CallContext());//LTILogic.GetCalculatesForISOandASTM(linearities, eq.CalibrationTypeID);
//                    Console.WriteLine("calculate");
//                     Console.WriteLine(res1.Count);


//                    RT.Clear();

//                    line++;

                   
//                    line++;
//                    foreach (var item in res1)
//                    {
//                        Console.WriteLine("calculate iisisi");
//                        Console.WriteLine(item.BasicCalibrationResult.Nominal);
//                        Console.WriteLine(item.BasicCalibrationResult.RUN1);
//                        RT.SaveNewItem(item,null,null,false);
//                        Console.WriteLine("calculate iisisi2");

//                    }
//                    line++;
//                }
//                StateHasChanged();

//            }
//            catch (Exception ex)
//            {

//                await ShowError("Claculate iso :" + line + " " + ex.Message);
//            }
//            finally
//            {
//                IsNewItem = false;
//            }



//        }
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


        public static List<Domain.Aggregates.Entities.Force> LIST { get; set; } //= new List<Domain.Aggregates.Entities.Force>();


        public WeightSetComponent WeightSetComponent { get; set; } //= new WeightSetComponent();

        PieceOfEquipment poq = new PieceOfEquipment();

        [Parameter]
        public List<WeightSet> WeightSetList2 { get; set; } = new List<WeightSet>();


        public bool IsNew { get; set; }


        public bool FirstRender { get; set; }


        //WorkOrderDetail wod = new WorkOrderDetail();



#pragma warning disable CS0414 // El campo 'LinearityComponentBase.WeightSetListCount' está asignado pero su valor nunca se usa
        int WeightSetListCount = 0;
#pragma warning restore CS0414 // El campo 'LinearityComponentBase.WeightSetListCount' está asignado pero su valor nunca se usa

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (RT != null)
            {
                RT.ShowControl = true;
                RT.ShowLabel = false;
            }

            //Console.WriteLine("OnAfterRenderAsync");
            RefreshResolution = WorkOrderItemCreate.ChangeResolution;
            //if (firstRender && RT != null)
            //{
            //    RT.ShowControl = true;
            //    RT.ShowLabel = false;
            //}

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


        public static List<Domain.Aggregates.Entities.Force> listlienarity { get; set; } = new List<Domain.Aggregates.Entities.Force>();

        public bool ShowGrid { get; set; }

        List<Force> lst1 = new List<Force>();

        protected override async Task OnInitializedAsync()

        {


            if (RT != null)
            {
                RT.ShowControl = true;
                RT.ShowLabel = false;
            }

            //if (WorkOrderItemCreate.eq.Refresh)
            //{
            //    WorkOrderItemCreate.eq.Refresh = false;

            //    return;
            //}


            try
            {
                await ShowProgress();

                

                url = Configuration.GetSection("Reports")["URL"];

                Linearity.TestPoint = new TestPoint();

                editContext = new EditContext(Linearity);

                aTimer = new System.Timers.Timer(300);

                aTimer.AutoReset = false;

                aTimer.Elapsed += OnUserFinish;

                



                double sumRuns = 0;

                //LIST = null;

                if (LTILogic.ValidateForceList(WorkOrderItemCreate.eq, IsCompresion, 1) && WorkOrderItemCreate.eq.GetCalibrationTypeID().HasValue)
                {
                    //Console.WriteLine("GetForceList");
                    //TODO: This part was the one duplicating
                    lst1 = LTILogic.GetForceList(WorkOrderItemCreate.eq, IsCompresion);
                    //Console.WriteLine(lst1.Count);
                    int subtipec = LTILogic.GetCalibrationSubType(WorkOrderItemCreate.eq, IsCompresion);
                    if (FirstRender)
                    {
                        lst1.ForEach(x => x.BasicCalibrationResult.FirstCalculate = true);
                    }
                    else
                    {
                        lst1.ForEach(x => x.BasicCalibrationResult.FirstCalculate = false);
                    }   
                    var dto = new ISOandASTM();
                    dto.Forces = lst1.Where(x => x.CalibrationSubTypeId == IsCompresion).OrderBy(x => x.BasicCalibrationResult.Position).ToList();
                    dto.WorkOrderDetail = WorkOrderItemCreate.eq;

                    var force = await _poeServices.GetCalculatesForISOandASTM(dto, new CallContext()); //.ConfigureAwait(false).GetAwaiter().GetResult(); ;

                    //Console.WriteLine(force.Count);
                    RT.Clear();
                    RT.Clear2();
                    //SortField = RT.GetPropertyName(() => Linearity.TestPo int.Position);
                    SortField = RT.GetPropertyName(() => Linearity.BasicCalibrationResult.Position);

                    //if (force != null)
                    //{
                        foreach (var itemm in lst1)
                        {
                            if(force != null && force.Count > 0)
                            {
                                itemm.BasicCalibrationResult.MaxErrorNominal = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.MaxErrorNominal;
                                itemm.BasicCalibrationResult.MaxErrorNominalASTM = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.MaxErrorNominalASTM;
                            itemm.BasicCalibrationResult.ToastMessageForce = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.ToastMessageForce;
                            itemm.BasicCalibrationResult.ToastMessageForceTotal = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.ToastMessageForceTotal;
                            itemm.BasicCalibrationResult.ClassRun1 = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.ClassRun1;
                            itemm.BasicCalibrationResult.Class = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.Class;
                            itemm.BasicCalibrationResult.InToleranceFound = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.InToleranceFound;
                            itemm.BasicCalibrationResult.InToleranceLeft = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.InToleranceLeft;
                            itemm.BasicCalibrationResult.InToleranceAdjusted = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.InToleranceAdjusted;
                            itemm.BasicCalibrationResult.IntoleranceRun4 = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.IntoleranceRun4;
                            itemm.BasicCalibrationResult.MessageForceDifference = force.FirstOrDefault(x => x.BasicCalibrationResult.SequenceID == itemm.BasicCalibrationResult.SequenceID).BasicCalibrationResult.MessageForceDifference;
                        }
                            if (itemm.BasicCalibrationResult.Resolution == 0)
                            {
                                itemm.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Resolution;
                                itemm.BasicCalibrationResult.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;
                            }
                            //await RT.SaveNewItem(itemm);
#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                            //RT.SaveNewItem(itemm, "asc", SortField);
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.

                        }
                    //}
                    

                    LIST = lst1.OrderBy(x => x.BasicCalibrationResult.Position).ToList();
                    RT.ItemList = lst1;
                    RT.ItemsDataSource = lst1;

                    ShowGrid = true;

                   

                }

                else

                    /////NEw CODE
                    if (!LTILogic.ValidateForceList(WorkOrderItemCreate.eq, IsCompresion, 1) && WorkOrderItemCreate.eq.GetCalibrationTypeID().HasValue)
                {
                    //Console.WriteLine("CreateForceList");
                    //Console.WriteLine(IsCompresion);

                    //List<Force> lst1;
                    if (WorkOrderItemCreate.eq.Refresh == true)
                    {
                        lst1 = LTILogic.GetForceList(WorkOrderItemCreate.eq, IsCompresion);
                    }
                    else
                    {
                         lst1 = LTILogic.CreateForceList(WorkOrderItemCreate.eq, IsCompresion);
                    }
                    


                    if (lst1 != null)
                    {
                        //Console.WriteLine("Not Null Linearity");
                        RT.Clear();
                        foreach (var itemm in lst1)
                        {
                            //if (RefreshResolution)
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
                        foreach (var itemm in lst1)
                        {

                            //RT.SaveNewItem(itemm, "asc", SortField);

                        }
                        LIST = lst1.OrderBy(x => x.BasicCalibrationResult.Position).ToList();
                        RT.ItemList = lst1;
                        RT.ItemsDataSource = lst1;
                        ShowGrid = true;
                       
                    }
                    //Console.WriteLine("Passed if");
                }


            }
            catch (Exception ex) 
            { 
            //Console.WriteLine($"{ex.StackTrace}");  
            }
            finally
            {
                WorkOrderItemCreate.eq.Refresh = false;
                await CloseProgress();  

            }

            await base.OnInitializedAsync();
        }


        //public async Task CopyToCompression()
        //{

        //    try
        //    {
        //        if (1 == 1)
        //        {
        //            List<Force> TotalForces = new List<Force>();
        //            List<Force> TotalForcesComp = new List<Force>();
        //            TotalForcesComp = WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == 5).ToList();
        //            TotalForces = WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == 4).ToList();


        //            foreach (var item in TotalForcesComp)
        //            {
        //                WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Remove(item);
        //            }

        //            List<Force> TotalForcesCompt = new List<Force>();

        //                foreach (var item in TotalForces)
        //                {

        //                    var clonedJson = JsonConvert.SerializeObject(item);
        //                    var resultCloned = JsonConvert.DeserializeObject<Force>(clonedJson);
        //                    var TotalForceComp = TotalForcesComp.Where(x => x.BasicCalibrationResult.SequenceID == resultCloned.BasicCalibrationResult.SequenceID).FirstOrDefault();
        //                    TotalForceComp.BasicCalibrationResult.FS = resultCloned.BasicCalibrationResult.FS;
        //                    TotalForceComp.BasicCalibrationResult.Nominal = resultCloned.BasicCalibrationResult.Nominal;
        //                    TotalForcesCompt.Add(resultCloned);
        //                    WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Add(TotalForceComp);

        //                }
        //            await ShowToast("Values copied to Compression.", ToastLevel.Success);
        //        }
        //    }
        //    catch
        //    {
        //        await ShowError("Error Copy the Items");
        //    }


        //}

        public List<Force> TotalForcesCompt { get; set; }

        public async Task CopyToCompression()
        {

            try
            {
                if (HasChange)
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
                    if(WorkOrderItemCreate.eq?.BalanceAndScaleCalibration?.Forces != null)
                    {
                        TotalForcesComp = WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == 5).ToList();
                    }
                    
                    //Tension
                    TotalForces = RT.Items.ToList();//WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == 4).ToList();

                    if(TotalForcesComp != null && TotalForcesComp.Count > 0)
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
                        resultCloned.CalibrationSubTypeId = 5;
                        resultCloned.BasicCalibrationResult.CalibrationSubTypeId = 5;
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
                                cs.CalibrationSubTypeID = 5;

                            }
                        }

                        resultCloned.CalibrationSubType_Weights = item.CalibrationSubType_Weights;

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
                else
                {
                    await ShowToast("Don't  copy data, No change in grid", ToastLevel.Warning);
                }
                ////////////////// new code end
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                await ShowError("Error Copy the Items");
            }

        }

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


                HasChange = true;

                // remove previous one
                aTimer.Stop();
                // new timer
                aTimer.Start();


            }
            catch
            {

            }



        }




        private void OnUserFinish2(Object source, ElapsedEventArgs e)
        { 
                aTimer.Stop();
                aTimer2.Stop();
                if(HasChange)
                {                 
                 //Calculate(LIST);

                //HasChange = false;
                InvokeAsync(async () =>
                {
                    //Console.WriteLine("calculate show");

                    await Calculate(RT.Items);
                    HasChange = false;

                    //Console.WriteLine("calculate closeprogress");
                });

            }

        }

        private void OnUserFinish(Object source, ElapsedEventArgs e)
        {

           
                
                aTimer.Stop();

                List<Force> a = new List<Force>();


                InvokeAsync(async () =>
           {
                try
            {
               await ShowProgress(false);
                   //HasChange = false;
                   var dto = new ISOandASTM();
                   dto.Forces = RT.Items.Where(x => x.CalibrationSubTypeId == IsCompresion).OrderBy(x => x.BasicCalibrationResult.Position).ToList();
                   dto.WorkOrderDetail = WorkOrderItemCreate.eq;
                   // RT.Items.Where(x => x.CalibrationSubTypeId == IsCompresion).OrderBy(x => x.BasicCalibrationResult.Position).ToList() ; // 

                   var aa =await _poeServices.GetCalculatesForISOandASTM(dto, new CallContext());
               int cont = 0;
               //Console.WriteLine("OnUserFinish " + RT.Items.Count + " --- " + aa.Count);
                   foreach (var item in aa.Where(x => x.CalibrationSubTypeId == IsCompresion).OrderBy(x => x.BasicCalibrationResult.Position))
                   {

                       var reso = item.BasicCalibrationResult.Uncertanty.ToString();

                       var result2 = reso.RoundedFunction("0.01", 2, RoundType.RoundToResolution, "System.Double");

                       item.BasicCalibrationResult.Uncertanty = Convert.ToDouble(result2);

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
            catch(Exception ex)
            {
                   await ShowError(ex);
            }
            finally
            {
                await CloseProgress();
                IniCalculated = 2;
            }

           });

        }

        public List<Domain.Aggregates.Entities.Force> FivePonints()
        {
            List<Domain.Aggregates.Entities.Force> listlienarity = new List<Domain.Aggregates.Entities.Force>();

            for (int i = 0; i < 5; i++)
            {
                Domain.Aggregates.Entities.Force ln = new Domain.Aggregates.Entities.Force();

                ln.SequenceID = i;
                ln.TestPoint = new TestPoint();
                ln.BasicCalibrationResult = new ForceResult();
                ln.WeightSets = new List<WeightSet>();
                listlienarity.Add(ln);
            }



            return listlienarity;

        }

        public List<Domain.Aggregates.Entities.Force> TenPonints()
        {
            List<Domain.Aggregates.Entities.Force> listlienarity = new List<Domain.Aggregates.Entities.Force>();

            for (int i = 0; i < 5; i++)
            {
                listlienarity.Add(new Domain.Aggregates.Entities.Force { SequenceID = i });
            }



            return listlienarity;


        }

        public async Task SetFivePoints()
        {
            LIST = await Task.FromResult(FivePonints());

            StateHasChanged();
        }



        [Parameter]
        public string HideElement { get; set; } = "modalLinearity";

        [CascadingParameter] public IModalService Modal { get; set; }
        public string _message { get; set; }
        //Logger.LogDebug(Group.Name);
        public async Task SelectWeight(Force item, int position)
        {

            //Console.WriteLine("SelectWeight  Position" + position) ;
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

            var messageForm = Modal.Show(typeof(CalibrationSaaS.Infraestructure.Blazor.LTI.WeightComponent<Force>),"Select Standard", parameters, op);
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
                        {   //Console.WriteLine("select 44");
                            RT.Items[i].WeightSets.Add(ws);
                        if (ws.Resolution !=0)
                        RT.Items[i].BasicCalibrationResult.Resolution = ws.Resolution;
                        }


                    }
                //}


           
               
            }
                 
            if(RemoveList != null)
            {
                //Console.WriteLine("select 3");
                 foreach (var ws in RemoveList)
                {
                    var result11 = item.WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                    if (result11 != null)
                    {//Console.WriteLine("select 33");
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

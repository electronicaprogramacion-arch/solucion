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
namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Order
{
    public class LinearityComponentBase : CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<WorkOrderDetail>
    {

        [CascadingParameter(Name = "CascadeParam1")]
        public IWorkOrderItemCreate WorkOrderItemCreate { get; set; }

        [Parameter]
        public int IsCompresion { get; set; } = 0;

        //[Inject]
        //public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public bool RefreshResolution { get; set; }

        public ElementReference InputText { get; set; }

        [Inject]
        public IIndexedDbFactory DbFactory { get; set; }

        [Parameter]
        public bool Enabled { get; set; } = false;

        public string Description { get; set; } = "";

        //[Inject] public Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }


        [Inject]
        public Func<IIndexedDbFactory, Application.Services.IWorkOrderDetailServices<CallContext>> Service { get; set; }



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



            //Id = "@("asFoundTexbox_" + ITEM.SequenceID.ToString())"
            //
            //JSRuntime.fixedDecimal

            //JSRuntime.InvokeVoidAsync("fixedDecimal", "asFoundTexbox_"+ lin.SequenceID).ConfigureAwait(false).GetAwaiter().GetResult();

            //var a= String.Format("{0:0.00}", lin.BasicCalibrationResult.AsFound);

            //lin.BasicCalibrationResult.AsFound = Convert.ToDouble(a);


            //string input_decimal_number = lin.BasicCalibrationResult.AsFound.ToString(); ;
            //var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
            //if (regex.IsMatch(input_decimal_number))
            //{
            //    string decimal_places = regex.Match(input_decimal_number).Value;

            //}

            IniCalculated = 1;
            //var a = lin.BasicCalibrationResult.AsFound.ToStringNoTruncate((int)lin.TestPoint.DecimalNumber);

            //lin.BasicCalibrationResult.AsFound = a;

            return lin;

        }
        public Force RowChange(Force lin)
        {

            return lin;

            if (RT.IsDragAndDrop)
            {
                int posi = 100;
                var tmpg = RT.Items;
                foreach (var item in tmpg)
                {
                    item.TestPoint.Position = posi;
                    posi++;
                }

                return lin;
            }

            if (lin.TestPoint.Position >= 100)
            {
                return lin;
            }


            var rr = new List<TestPoint>();


            int cont = 100;

            var aarrl = new List<Force>();



            aarrl = RT.Items.Where(x => x.TestPoint.CalibrationType.ToLower() == "Tension" && x.TestPoint.IsDescendant == false)
                .OrderBy(x => x.TestPoint.NominalTestPoit).ToList();

            aarrl.ForEach(item22 =>
            {

                item22.TestPoint.Position = cont;
                cont++;

            });


            var aarr2 = RT.Items.Where(x => x.TestPoint.CalibrationType.ToLower() == "Tension" && x.TestPoint.IsDescendant == true)
                .OrderByDescending(x => x.TestPoint.NominalTestPoit).ToList();

            aarr2.ForEach(item22 =>
            {

                item22.TestPoint.Position = cont;
                cont++;

            });

            foreach (var item in aarr2)
            {
                aarrl.Add(item);
            }


            var arr3 = RT.Items.Where(x => x.TestPoint.CalibrationType.ToLower() == "eccentricity").OrderBy(x => x.TestPoint.NominalTestPoit).OrderBy(x => x.TestPoint.IsDescendant).ToList();
            arr3.ForEach(x =>
            {
                x.TestPoint.Position = cont;
                cont++;
            });

            var arr4 = RT.Items.Where(x => x.TestPoint.CalibrationType.ToLower() == "repeatability").OrderBy(x => x.TestPoint.NominalTestPoit).OrderBy(x => x.TestPoint.IsDescendant).ToList();
            arr4.ForEach(y =>
            {
                y.TestPoint.Position = cont;
                cont++;
            });



            foreach (var it in arr3)
            {
                aarrl.Add(it);
            }

            foreach (var it in arr4)
            {
                aarrl.Add(it);
            }

            RT.Clear();

            //for(int ii=0;ii< aarrl.Count; ii++)
            //{
            //    if(ii< aarrl.Count - 1)
            //    {
            //        RT.SaveNewItem(aarrl[ii],null,null,false).ConfigureAwait(false).GetAwaiter().GetResult();
            //    }
            //    else
            //    {
            //        RT.SaveNewItem(aarrl[ii], null, null, false).ConfigureAwait(false).GetAwaiter().GetResult();
            //    }
            //}

            RT.Items = aarrl;

            //cont++;
            //return t;






            if (RT.Items != null && RT.Items.Count > 0)
            {
                var item = RT.Items[0];
                if (item.TestPoint.NominalTestPoit == 0)
                    item.MinTolerance = 0;
                item.MaxTolerance = 0;
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
                await Calculate();

            }

            IsNewItem = false;

        }


        public async Task AddNewItem()
        {

            await RT.SaveNewItem(DefaultNew());


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



            lin.TestPoint.UnitOfMeasurement = eq.PieceOfEquipment.UnitOfMeasure;

            lin.TestPoint.UnitOfMeasurementID = eq.PieceOfEquipment.UnitOfMeasureID.Value;

            lin.TestPoint.UnitOfMeasurementOutID = eq.PieceOfEquipment.UnitOfMeasureID.Value;

            lin.TestPoint.Resolution = eq.Resolution;

            lin.TestPoint.DecimalNumber = eq.DecimalNumber;

            //lin.TestPoint.UnitOfMeasurement = lin.TestPoint.UnitOfMeasurementID.GetUoM(AppState.UnitofMeasureList);

            lin.TestPoint.CalibrationType = "Tension";

            lin.TestPoint.TestPointTarget = 1;

            //lin.BasicCalibrationResult.WeightApplied = 0;

            //lin.BasicCalibrationResult.AsFound = 0;

            //lin.BasicCalibrationResult.AsFound = 0;


            //lin.BasicCalibrationResult.UnitOfMeasureID = lin.TestPoint.UnitOfMeasurementOutID;


            lin.BasicCalibrationResult.CalibrationSubTypeId = 1;

            lin.BasicCalibrationResult.SequenceID = RT.Items.Count + 1;

            lin.BasicCalibrationResult.WorkOrderDetailId = eq.WorkOrderDetailID;

            lin.WorkOrderDetailId = eq.WorkOrderDetailID;

            lin.SequenceID = RT.Items.Count + 1;

            lin.UnitOfMeasureId = lin.TestPoint.UnitOfMeasurementID;

            lin.TestPoint.NominalTestPoit = result;

            //if(RT.Items != null && RT?.Items.Count > 0 &&  RT?.Items?.ElementAtOrDefault(RT.Items.Count - 1)?.TestPoint != null)
            //{
            //    lin.TestPoint.Position = RT.Items.ElementAtOrDefault(RT.Items.Count - 1).TestPoint.Position;
            //}


            // remove previous one
            //aTimer.Stop();
            //// new timer
            //aTimer.Start();
            ////IniCalculated = 1;
            //if (IniCalculated == 2)
            //{

            //Calculate().ConfigureAwait(false).GetAwaiter().GetResult();


            //    IniCalculated = 1;
            //}
            IsNewItem = true;

            return lin;

        }


        public async Task ChangeControl2(ChangeEventArgs arg, Force lin, bool AddWeight = false)
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


      public async Task Calculate()
        {

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
                
                //if (eq?.BalanceAndScaleCalibration == null)
                //{
                //    eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();

                //    eq.BalanceAndScaleCalibration.Forces = linearities;
                //}

                //else
                //{
                //    var lsttemp = new List<Force>();

                //    foreach (var item in  eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId == LTILogic.GetCalibrationSubType(eq, IsCompresion)))
                //    {
                //        lsttemp.Add(item);
                //    }
                        
                       
                //   foreach(var item in  lsttemp)
                //    {
                //        eq.BalanceAndScaleCalibration.Forces.Remove(item);
                //    }
                //    foreach (var item in linearities)
                //    {
                //        eq.BalanceAndScaleCalibration.Forces.Add(item);
                //    }
                    

                //}



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


//                var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

//                if (result != null)
//                {
//                    eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

//                    RT.Clear();

//                    foreach (var item in eq.BalanceAndScaleCalibration.Forces)
//                    {
//#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
//                        RT.SaveNewItem(item);
//#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
//                    }

//                }

                 var result = 1;// LTILogic.GetCalculatesForISOandASTM( (eq).GetAwaiter().GetResult();
                //var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

                if (result != null)
                {
                    //eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

                    RT.Clear();

                    line++;

                    var res1 = LTILogic.GetCalculatesForISOandASTM(linearities, eq.CalibrationTypeID);
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

                await ShowError("Claculate iso :" + line + " " + ex.Message);
            }
            finally
            {
                IsNewItem = false;
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




        [Parameter]
        public string EntityID { get; set; }


        public Force Linearity = new Force();

        [Inject] public AppStateCompany AppState { get; set; }

        [Parameter]
        public Domain.Aggregates.Entities.Status CurrentStatus { get; set; } = new Domain.Aggregates.Entities.Status();

        public EditContext editContext { get; set; }

        [Parameter]
        public WorkOrderDetail eq { get; set; } = new WorkOrderDetail();


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

            if (firstRender && RT != null)
            {
                RT.ShowControl = true;
                RT.ShowLabel = false;
            }

            await base.OnAfterRenderAsync(firstRender);

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


                if (FirstRender && eq != null && eq.WOD_Weights != null)
                {

                    foreach (var w in eq.WOD_Weights)
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

                //if (WeightSetComponent != null && WeightSetList2 != null)
                //{

                //    await WeightSetComponent.Show(WeightSetList2);

                //}


                if (firstRender && eq.CalibrationTypeID.HasValue && LTILogic.ValidateForceList(eq, IsCompresion))
                {

                    LIST = LTILogic.GetForceList(eq, IsCompresion);  //eq?.BalanceAndScaleCalibration?.Linearities.ToList();

                    RT.Clear();
                    //SortField = RT.GetPropertyName(() => Linearity.TestPoint.Position);
                    SortField = RT.GetPropertyName(() => Linearity.SequenceID);
                    foreach (var itemm in LIST)
                    {
                        if (RefreshResolution && itemm?.TestPoint != null)
                        {
                            itemm.BasicCalibrationResult.Resolution = eq.Resolution;
                            itemm.BasicCalibrationResult.DecimalNumber = eq.DecimalNumber;
                        }
                        //await RT.SaveNewItem(itemm);
#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                        RT.SaveNewItem(itemm, "asc", SortField);
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.

                    }

                    if (!IsWeightLoaded)
                    {
                        //aTimer.Stop();
                        //aTimer.Start();
                        //IniCalculated = 2;
                        await Calculate();
                    }
                }

                else

                    /////NEw CODE
                    if (firstRender && eq.CalibrationTypeID.HasValue && !LTILogic.ValidateForceList(eq, IsCompresion))
                {
                    //Console.WriteLine("CreateForceList");
                    //Console.WriteLine(IsCompresion);
                    var listlienarity = LTILogic.CreateForceList(eq, IsCompresion);


                    if (listlienarity != null)
                    {
                        RT.Clear();
                        foreach (var itemm in listlienarity)
                        {
                            if (RefreshResolution && itemm?.TestPoint != null)
                            {
                                itemm.TestPoint.Resolution = eq.Resolution;
                                itemm.TestPoint.DecimalNumber = eq.DecimalNumber;
                            }


                            if (itemm.TestPoint != null && itemm.TestPoint.IsDescendant)
                            {
                                HasDescendants = true;
                                break;
                            }

                        }


                        SortField = RT.GetPropertyName(() => Linearity.TestPoint.Position);
                        foreach (var itemm in listlienarity)
                        {

                            RT.SaveNewItem(itemm, "asc", SortField);

                        }

                        LIST = listlienarity;
                    }

                    if (!IsWeightLoaded)
                    {
                        //aTimer.Stop();
                        //aTimer.Start();
                        //IniCalculated = 2;
                        await Calculate();
                    }

                }
                //////////////// new code end



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


            Linearity.TestPoint = new TestPoint();

            editContext = new EditContext(Linearity);

            aTimer = new System.Timers.Timer(400);

            aTimer.Elapsed += OnUserFinish;



            await base.OnInitializedAsync();
        }


        private void OnUserFinish(Object source, ElapsedEventArgs e)
        {

            try
            {

                aTimer.Stop();
                InvokeAsync(async () =>
            {
                await ShowProgress(false);
                await Calculate();
                await CloseProgress();
                //Console.WriteLine("calculate closeprogress");
            });

                //Calculate().ConfigureAwait(false).GetAwaiter().GetResult();


            }
            catch
            {

            }
            finally
            {
                IniCalculated = 2;
            }

            //aTimer.Start();
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


            //if (item == null || WeightSetComponent == null || WeightSetComponent.Grid == null || WeightSetComponent.Grid.Items == null)
            //{
            //    return;
            //}

            if (item == null || WorkOrderItemCreate.eq?.WOD_Weights == null)
            {
                return;
            }


            var parameters = new ModalParameters();


            WeightSetList2 = new List<WeightSet>();
            foreach (var w in WorkOrderItemCreate.eq.WOD_Weights)
            {
                if (item.BasicCalibrationResult.Nominal >= w.WeightSet.WeightNominalValue)
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

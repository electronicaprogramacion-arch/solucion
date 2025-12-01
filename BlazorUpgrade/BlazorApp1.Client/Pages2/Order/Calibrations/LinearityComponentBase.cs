using Blazed.Controls;
using Blazor.IndexedDB.Framework;
using BlazorApp1.Blazor.Blazor.Pages.Order;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Helper;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Tools1;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Grpc.Core;
using Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using SqliteWasmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using static SQLite.SQLite3;
using WeightSetComponent = CalibrationSaaS.Infraestructure.Blazor.LTI.Scale.WeightSetComponent;

namespace BlazorApp1.Blazor.Pages.Order.Calibrations
{
    public class LinearityComponentBase : CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<WorkOrderDetail>
    {


        public string  Tenant { get; set; }

        public string CSSSelect = "btnWidth buttonLin";


        [CascadingParameter(Name = "CascadeParam1")]
        public BlazorApp1.Blazor.Blazor.Pages.Order.IWorkOrderItemCreate WorkOrderItemCreate { get; set; }



        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>> BasicsServices { get; set; }
        //[Inject]
        //public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public bool RefreshResolution { get; set; }

        public ElementReference InputText { get; set; }




        [Parameter]
        public bool Enabled { get; set; } = false;

        public string Description { get; set; } = "";

        //[Inject] public Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }


        [Inject]
        public Func<dynamic, CalibrationSaaS.Application.Services.IWorkOrderDetailServices<CallContext>> Service { get; set; }

        [Inject]
        public Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>> PoeService { get; set; }


        [Inject]
        IConfiguration Configuration { get; set; }
        public string url { get; set; }


        [Parameter]
        public ResponsiveTable<Linearity> RT { get; set; } = new ResponsiveTable<Linearity>();

        public string SortField { get; set; }

        public bool HasDescendants { get; set; }

        public int MyProperty { get; set; }

        public Linearity RowAfterRender(Linearity lin)
        {


            IniCalculated = 1;


            return lin;

        }
        public Linearity RowChange(Linearity lin)
        {
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

            if (lin?.TestPoint?.Position >= 100)
            {
                return lin;
            }


            var rr = new List<TestPoint>();


            int cont = 100;

            var aarrl = new List<Linearity>();



            aarrl = RT.Items.Where(x => x.TestPoint.CalibrationType.ToLower() == "linearity" && x.TestPoint.IsDescendant == false)
                .OrderBy(x => x.TestPoint.NominalTestPoit).ToList();

            aarrl.ForEach(item22 =>
            {

                item22.TestPoint.Position = cont;
                cont++;

            });


            var aarr2 = RT.Items.Where(x => x?.TestPoint?.CalibrationType.ToLower() == "linearity" && x.TestPoint.IsDescendant == true)
                .OrderByDescending(x => x?.TestPoint?.NominalTestPoit).ToList();

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


            RT.Items = aarrl;

            if (RT.Items != null && RT.Items.Count > 0)
            {
                var item = RT.Items[0];
                if (item.TestPoint.NominalTestPoit == 0)
                    item.MinTolerance = 0;
                item.MaxTolerance = 0;
            }

            foreach (var iy in RT.Items)
            {
                if (iy?.TestPoint != null)
                {
                    iy.BasicCalibrationResult.Position = iy.TestPoint.Position;
                }

            }

            return lin;

        }



        public string TextMultiplerFound { get; set; } = "1";

        public string TextMultiplerLeft { get; set; } = "1";


        public string TextNewValue { get; set; }

        public bool EnableWeightSet { get; set; } = true;


        public bool EnableSetPoints { get; set; } = true;

        public Linearity _Linearity { get; set; }


        public bool IsAddLinearity { get; set; }

        public async Task TaskAfterSave(Linearity item, string sav)
        {
            if (IsNewItem)
            {


            }

            IsNewItem = false;

        }


        public async Task AddNewItem()
        {

            await RT.SaveNewItem(DefaultNew());

            WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Linearities = RT.Items;

            await Calculate();

        }

        public bool IsNewItem { get; set; }
        public Linearity DefaultNew()
        {
            Linearity lin = new Linearity();

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





            lin.BasicCalibrationResult = new BasicCalibrationResult();
            lin.TestPoint = new TestPoint();
            lin.WeightSets = new List<WeightSet>();

            if (WorkOrderItemCreate?.eq?.PieceOfEquipment?.UnitOfMeasureID.HasValue==true)
            {
                lin.TestPoint.UnitOfMeasurement = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure;

                lin.TestPoint.UnitOfMeasurementID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;

                lin.TestPoint.UnitOfMeasurementOutID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;
            }

           

            if(WorkOrderItemCreate?.eq?.Tolerance != null)
            {
                lin.TestPoint.Resolution = WorkOrderItemCreate.eq.Tolerance.Resolution;

                lin.TestPoint.DecimalNumber = WorkOrderItemCreate.eq.Tolerance.DecimalNumber;

                lin.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Tolerance.Resolution;


            }
            else
            {
                lin.TestPoint.Resolution = WorkOrderItemCreate.eq.Resolution;

                lin.TestPoint.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;

                lin.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Resolution;


            }


            if (!IsOnline || 1 == 1)
            {
                lin.TestPointID = NumericExtensions.GetUniqueID(lin.TestPointID);
                lin.TestPoint.TestPointID = lin.TestPointID.Value;
            }


            //lin.TestPoint.UnitOfMeasurement = lin.TestPoint.UnitOfMeasurementID.GetUoM(AppState.UnitofMeasureList);

            lin.TestPoint.CalibrationType = "Linearity";

            lin.TestPoint.TestPointTarget = 1;

            lin.BasicCalibrationResult.WeightApplied = 0;

            lin.BasicCalibrationResult.AsFound = 0;

            lin.BasicCalibrationResult.AsFound = 0;


            lin.BasicCalibrationResult.UnitOfMeasureID = lin.TestPoint.UnitOfMeasurementOutID;


            lin.BasicCalibrationResult.CalibrationSubTypeId = 1;



            lin.BasicCalibrationResult.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;

            lin.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;
            if (RT?.Items?.Count > 0)
            {
                lin.SequenceID = RT.Items.MaxBy(x => x.SequenceID).SequenceID + 1;
                lin.BasicCalibrationResult.SequenceID = lin.SequenceID;
                lin.TestPoint.Description = "Manual TestPoint " + lin.SequenceID.ToString();
            }


            lin.UnitOfMeasureId = lin.TestPoint.UnitOfMeasurementID;

            lin.TestPoint.NominalTestPoit = result;

            //lin.TestPoint.LowerTolerance = lin.TestPoint.NominalTestPoit - lin.TestPoint.Resolution;

            //lin.TestPoint.UpperTolerance = lin.TestPoint.NominalTestPoit + lin.TestPoint.Resolution;

            //lin.MaxTolerance


            IsNewItem = true;



            return lin;

        }


        public async Task ChangeControl2(ChangeEventArgs arg, Linearity lin, bool AddWeight = false)
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

        public void AddWeightset(Linearity lin, double result = 0)
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


        public async Task ChangeControl(ChangeEventArgs arg, Linearity lin)
        {

            // return;

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

                lin.BasicCalibrationResult.WeightApplied = result;

                // remove previous one
                aTimer.Stop();
                // new timer
                aTimer.Start();
                //IniCalculated = 1;
                if (IniCalculated == 2)
                {

                    //await Calculate();


                    IniCalculated = 1;
                }




            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {

            }

        }

        public List<Linearity> MemoryGrid { get; set; } = new List<Linearity>();


        public string BoudValues(Linearity lin)
        {



            return "";

        }

        [Parameter]
        public bool Accredited { get; set; }

        public static bool IsWeightLoaded { get; set; }


        public List<PieceOfEquipment> Standards { get; set; }

        public async Task LoadStandards()
        {
            //try
            //{

            if (WorkOrderItemCreate.eq.WOD_Weights == null || WorkOrderItemCreate.eq.WOD_Weights.Count == 0)
            {
                return;
            }


            WorkOrderDetailGrpc wd = new WorkOrderDetailGrpc(Service, DbFactory, Header);

            var resul = await wd.GetConfiguredWeights(WorkOrderItemCreate.eq);
            if (resul?.BalanceAndScaleCalibration != null)
            {
                WorkOrderItemCreate.eq.BalanceAndScaleCalibration = resul.BalanceAndScaleCalibration;

                Console.WriteLine("LoadStandards");
            }

            //PieceOfEquipmentGRPC poeservice = new PieceOfEquipmentGRPC(PoeService, DbFactory, p);

            //var poes = await poeservice.GetAllWeightSets(WorkOrderItemCreate.eq.PieceOfEquipment);

            //Standards = poes.PieceOfEquipments;


            //}
            //catch (Exception ex)
            //{

            //}




        }


        public async Task Calculate()
        {

            //return;

            try
            {
                Console.WriteLine("calculate");
                IsNewItem = false;

                if (RT == null || RT.Items == null || RT.Items.Count == 0)
                {
                    return;
                }

                var linearities = RT.Items;
                Console.WriteLine(RT.Items.Count);

                if (WorkOrderItemCreate.eq?.BalanceAndScaleCalibration == null)
                {
                    WorkOrderItemCreate.eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
                }

                WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Linearities = linearities;



                if (!IsWeightLoaded)
                {
                    await LoadStandards();

                    IsWeightLoaded = true;
                }

                BasicsServiceGRPC basics = new BasicsServiceGRPC(BasicsServices, DbFactory);
                CalibrationType calibrationType = new CalibrationType()
                {
                    CalibrationTypeId = WorkOrderItemCreate.eq.BalanceAndScaleCalibration.CalibrationTypeId
                };
                calibrationType = await basics.GetCalibrationTypeById(calibrationType, new CallOptions());

                var result = Querys.WOD.CalculateValuesByID(WorkOrderItemCreate.eq, AppState.UnitofMeasureList, true, calibrationType).GetAwaiter().GetResult();

                RefreshResolution = false;
                if (result != null)
                {
                    WorkOrderItemCreate.eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

                    RT.Clear();

                    foreach (var item in WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Linearities)
                    {
#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                        RT.SaveNewItem(item);
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                    }

                }



                StateHasChanged();

            }
            catch (Exception ex)
            {

                await ExceptionManager(ex, "Calculate");
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
            string customer = Configuration.GetSection("Reports")["Customer"];

            if (WeightSetComponent == null || WeightSetComponent?.lstWeightSet == null)
            {
                return;
            }
            //weightsets
            var a = WeightSetComponent?.lstWeightSet;//WeightSetComponent.Grid.Items;

            var b = RT.Items; // LIST;

            //var c = b.Select(x => x.TestPoint);

            Querys.AssingAlgoritm(a, b, AppState.UnitofMeasureList, customer);

            var decimalNumber = (int)WorkOrderItemCreate.eq.DecimalNumber;
            foreach (var item in RT.Items)
            {
                item.BasicCalibrationResult.WeightApplied = Math.Round(item.CalculateWeightValue, decimalNumber);
                Console.WriteLine(item.CalculateWeightValue);
            }


            StateHasChanged();


        }

        List<WeightSet> Selected = new List<WeightSet>();




       


        public Linearity Linearity = new Linearity();

        [Inject] public AppStateCompany AppState { get; set; }

        [Parameter]
        public CalibrationSaaS.Domain.Aggregates.Entities.Status CurrentStatus { get; set; } = new CalibrationSaaS.Domain.Aggregates.Entities.Status();

        public EditContext editContext { get; set; }

        //[Parameter]
        //public WorkOrderDetail eq { get; set; } = new WorkOrderDetail();

        //[CascadingParameter(Name = "CascadeParam3")]
        //public WorkOrderDetail eq { get; set; }

        public List<CalibrationSaaS.Domain.Aggregates.Entities.Linearity> LIST { get; set; } = new List<CalibrationSaaS.Domain.Aggregates.Entities.Linearity>();


        public BlazorApp1.Blazor.LTI.Scale.WeightSetComponent WeightSetComponent { get; set; } //= new WeightSetComponent();

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
                await base.OnAfterRenderAsync(firstRender);



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

        public string Visibility { get; set; } = " display:block";

        public string Visibility2 { get; set; } = " display: none";

        public bool ShowGrid { get; set; } = true;

        protected override async Task OnInitializedAsync()

        {

            try
            {
                //await ShowProgress();
                Tenant = Configuration.GetSection("Reports")["Customer"];

                url = Configuration.GetSection("Reports")["URL"];
                Console.WriteLine("linearity OnInitializedAsync");
                Linearity.TestPoint = new TestPoint();

                editContext = new EditContext(Linearity);

                aTimer = new System.Timers.Timer(400);

                aTimer.AutoReset = false;

                aTimer.Elapsed += OnUserFinish;

                await base.OnInitializedAsync();

                Console.WriteLine("linearity onafter " + WorkOrderItemCreate.eq?.BalanceAndScaleCalibration?.Linearities?.Count());

                if (WorkOrderItemCreate?.eq != null && (WorkOrderItemCreate?.eq?.BalanceAndScaleCalibration?.Linearities?.Count() > 0))
                {
                    Console.WriteLine("linearity onafter " + WorkOrderItemCreate.eq?.BalanceAndScaleCalibration?.Linearities?.Count());
                    LIST = Querys.WOD.GetLinearityList(WorkOrderItemCreate.eq);  //eq?.BalanceAndScaleCalibration?.Linearities.ToList();

                    RT.Clear();
                    SortField = RT.GetPropertyName(() => Linearity.TestPoint.Position);
                    foreach (var itemm in LIST)
                    {
                        if (RefreshResolution)
                        {
                            if (WorkOrderItemCreate.eq.Tolerance != null)
                            {
                                itemm.TestPoint.Resolution = WorkOrderItemCreate.eq.Tolerance.Resolution;
                                itemm.TestPoint.DecimalNumber = WorkOrderItemCreate.eq.Tolerance.DecimalNumber;
                                itemm.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Tolerance.Resolution;
                            }
                            else
                            {
                                itemm.TestPoint.Resolution = WorkOrderItemCreate.eq.Resolution;
                                itemm.TestPoint.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;
                                itemm.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Resolution;
                            }


                        }




                        //await RT.SaveNewItem(itemm);


                        await RT.SaveNewItem(itemm, "asc", SortField);

                    }


                }

                else
                {
                    /////NEw CODE
                    if (WorkOrderItemCreate?.eq != null && WorkOrderItemCreate?.eq?.PieceOfEquipment != null
                        && (WorkOrderItemCreate?.eq?.PieceOfEquipment?.EquipmentTemplate != null
                           && WorkOrderItemCreate?.eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups != null
                           && WorkOrderItemCreate?.eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups?.ElementAtOrDefault(0)?.TestPoints?.Count > 0)
                           || WorkOrderItemCreate?.eq != null && WorkOrderItemCreate?.eq?.PieceOfEquipment != null
                           && WorkOrderItemCreate?.eq?.PieceOfEquipment?.TestGroups != null)
                    {




                        var listlienarity = Querys.WOD.CreateLinearityList(WorkOrderItemCreate.eq, !IsOnline);


                        if (listlienarity != null)
                        {
                            RT.Clear();
                            foreach (var itemm in listlienarity)
                            {
                                if (RefreshResolution)
                                {
                                    if (WorkOrderItemCreate.eq.Tolerance != null)
                                    {
                                        itemm.TestPoint.Resolution = WorkOrderItemCreate.eq.Tolerance.Resolution;
                                        itemm.TestPoint.DecimalNumber = WorkOrderItemCreate.eq.Tolerance.DecimalNumber;
                                        itemm.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Tolerance.Resolution;
                                    }
                                    else
                                    {
                                        itemm.TestPoint.Resolution = WorkOrderItemCreate.eq.Resolution;
                                        itemm.TestPoint.DecimalNumber = WorkOrderItemCreate.eq.DecimalNumber;
                                        itemm.BasicCalibrationResult.Resolution = WorkOrderItemCreate.eq.Resolution;
                                    }


                                }


                                if (itemm.TestPoint != null && itemm.TestPoint.IsDescendant)
                                {
                                    HasDescendants = true;
                                    break;
                                }

                            }

                            //if (!HasDescendants)
                            //{
                            //    SortField = RT.GetPropertyName(() => Linearity.TestPoint.NominalTestPoit);
                            //}
                            //else
                            //{

                            //    SortField = RT.GetPropertyName(() => Linearity.TestPoint.Position);
                            //}

                            SortField = RT.GetPropertyName(() => Linearity.TestPoint.Position);
                            foreach (var itemm in listlienarity)
                            {
                                //if(itemm.TestPoint != null && itemm.TestPoint.IsDescendant)
                                //{
                                //    HasDescendants = true;
                                //}

#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                                RT.SaveNewItem(itemm, "asc", SortField);
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                            }




                            LIST = listlienarity;
                        }
                    }

                }

                if (RT?.Items?.Count > 0)
                {
                    await Calculate();
                }


            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
            }
            finally
            {
                //await CloseProgress();
                ShowGrid = true;

                Visibility = " display:none";
                Visibility2 = "display:block";

            }



        }


        private void OnUserFinish(Object source, ElapsedEventArgs e)
        {
            aTimer.Stop();
            if (IsNewItem)
            {
                IsNewItem = false;
                return;
            }

            List<Linearity> a = new List<Linearity>();


            InvokeAsync(async () =>
            {
                try
                {


                    await ShowProgress(false);

                    if (!IsWeightLoaded)
                    {
                        await LoadStandards();

                        IsWeightLoaded = true;
                    }

                    //HasChange = false;
                    var aa1 = await Querys.WOD.CalculateValuesByID(WorkOrderItemCreate.eq, AppState.UnitofMeasureList, true);//await _poeServices.GetCalculatesForISOandASTM(RT.Items, new CallContext());


                    RefreshResolution = false;
                    WorkOrderItemCreate.CalibrationInstructions.changeTolerance = false;

                    //if (aa1?.BalanceAndScaleCalibration?.Linearities == null)
                    //{
                    //    throw new Exception();
                    //}

                    var aa =(ICollection<Linearity>) aa1.BalanceAndScaleCalibration.Linearities;
                    int cont = 0;
                    //Console.WriteLine("OnUserFinish " + RT.Items.Count.ToString() + " --- " + aa.Count.ToString());

                    foreach (var item in aa.OrderBy(x => x.TestPoint.Position))
                    {
                        Console.WriteLine("onuser");
                        Console.WriteLine(item.TestPoint.NominalTestPoit);
                        Console.WriteLine(item.TestPoint.Position);
                        RT.Items[cont] = item;
                        cont++;
                    }
                    LIST = RT.Items;
                    RT.ItemList = RT.Items;
                    RT.ItemsDataSource = RT.Items;
                    //await Task.Delay(50);
                    //HasChange = true;
                    //aTimer2.Start();
                    StateHasChanged();

                    //await Task.Delay(50);

                }
                catch (Exception ex)
                {
                    await ShowError(ex, "Onuser Finish");
                }
                finally
                {
                    await CloseProgress();
                    IniCalculated = 2;
                    aTimer.Stop();
                }

            });



        }

        //private void OnUserFinish(Object source, ElapsedEventArgs e)
        //{

        //    try
        //    {

        //        aTimer.Stop();
        //        InvokeAsync(async () =>
        //    {
        //        await ShowProgress(false);
        //        await Calculate();
        //        await CloseProgress();
        //    });

        //        //Calculate().ConfigureAwait(false).GetAwaiter().GetResult();

        //        IniCalculated = 2;






        //    }
        //    catch
        //    {

        //    }


        //    //aTimer.Start();
        //}

        public List<CalibrationSaaS.Domain.Aggregates.Entities.Linearity> FivePonints()
        {
            List<CalibrationSaaS.Domain.Aggregates.Entities.Linearity> listlienarity = new List<CalibrationSaaS.Domain.Aggregates.Entities.Linearity>();

            for (int i = 0; i < 5; i++)
            {
                CalibrationSaaS.Domain.Aggregates.Entities.Linearity ln = new CalibrationSaaS.Domain.Aggregates.Entities.Linearity();

                ln.SequenceID = i;
                ln.TestPoint = new TestPoint();
                ln.BasicCalibrationResult = new BasicCalibrationResult();
                ln.WeightSets = new List<WeightSet>();
                listlienarity.Add(ln);
            }



            return listlienarity;

        }

        public List<CalibrationSaaS.Domain.Aggregates.Entities.Linearity> TenPonints()
        {
            List<CalibrationSaaS.Domain.Aggregates.Entities.Linearity> listlienarity = new List<CalibrationSaaS.Domain.Aggregates.Entities.Linearity>();

            for (int i = 0; i < 5; i++)
            {
                listlienarity.Add(new CalibrationSaaS.Domain.Aggregates.Entities.Linearity { SequenceID = i });
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
        public async Task SelectWeight(Linearity item, int position)
        {


            //if (item == null || WeightSetComponent == null || WeightSetComponent.Grid == null || WeightSetComponent.Grid.Items == null)
            //{
            //    return;
            //}

            if (item == null || WeightSetComponent == null || WeightSetComponent.lstWeightSet == null
                || WeightSetComponent.lstWeightSet == null
                || WeightSetComponent.lstWeightSet.Count == 0)
            {
                return;
            }

            //Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
            //List<EquipmentTemplate> listEquipment;

            var parameters = new ModalParameters();
            //parameters.Add("SelectOnly", true);
            //parameters.Add("IsModal", true);


            WeightSetList2 = new List<WeightSet>();
            foreach (var w in WorkOrderItemCreate.eq.WOD_Weights)
            {
                //if (WorkOrderItemCreate?.eq?.PieceOfEquipment?.UnitOfMeasure?.UnitOfMeasureID == w?.WeightSet?.UnitOfMeasureID ||
                //   (w?.WeightSet?.UnitOfMeasure?.UnitOfMeasureBase != null
                //    && w.WeightSet.UnitOfMeasure.UnitOfMeasureBase.UnitOfMeasureBaseID == WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure.UnitOfMeasureBaseID) ||
                //    w.WeightSet.UnitOfMeasure != null && w.WeightSet.UnitOfMeasure.UnitOfMeasureBaseID == WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure.UnitOfMeasureBaseID)
                //{
                    WeightSetList2.Add(w.WeightSet);
                //}
                //else
                //{
                //    Console.WriteLine("w.WeightSet.UnitOfMeasure.UnitOfMeasureBase.UnitOfMeasureBaseID = " + w.WeightSet.UnitOfMeasure.UnitOfMeasureBase.UnitOfMeasureBaseID + "WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure.UnitOfMeasureBase.UnitOfMeasureBaseID =" + WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure.UnitOfMeasureBaseID);
                //}
            }

            parameters.Add("target", item);

            parameters.Add("DataSource", WeightSetList2);

            if (item.TestPoint == null)
            {
                item.TestPoint = new TestPoint()
                {
                    UnitOfMeasurementOut = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure
                };
            }
            parameters.Add("TestPoint", item.TestPoint);

            var SelWeightSetList = new List<WeightSet>();
            //if(RT != null) {
            //    foreach (var w in RT.Items)
            //    {
            //        if(w.WeightSets != null && w.WeightSets.Count > 0)
            //        {
            //            foreach (var iw in w.WeightSets)
            //            {
            //                SelWeightSetList.Add(iw);
            //            }
            //        }


            //    }
            //}

            if (item.WeightSets != null)
            {
                foreach (var iw in item.WeightSets)
                {
                    SelWeightSetList.Add(iw);
                }
            }



            parameters.Add("SelectWeight", SelWeightSetList);


            ModalOptions op = new ModalOptions();

            //op.HideHeader = true;

            //op.HideCloseButton = false;

            op.Position = ModalPosition.Center;

            //op.Animation = ModalAnimation.FadeIn(200);


            op.Class = "blazored-modal " + ModalSize.MediumWindow;  //blazored-modal blazored-modal-scrollable allWindow

            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
            }

            var messageForm = Modal.Show<Blazor.LTI.Scale.WeightComponent<Linearity>>("Select Weight", parameters, op);
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



            //List<WeightSet> WeightSetList = (List<WeightSet>)result.Data;

            if (WeightSetList != null)
            {
                Calculate cal = new Calculate();

                //item.AsLeft = 1223;
                var calculate = await cal.InlineCalculate(WeightSetList, item, url);

                //item.WeightSets = WeightSetList;

                //item.BasicCalibrationResult.WeightApplied = CalResult;

                //RT.Items[RT.Items.FindIndex(ind => ind.LinearityId== item.LinearityId)] = cal.InlineCalculate(WeightSetList,item);

                int cont = position;//RT.CurrentSelectIndex;

                if (cont < 0)
                {
                    cont = 0;
                }
                for (int i = cont; i < RT.Items.Count; i++)
                {
                    //var suma = RT.Items[i].BasicCalibrationResult.WeightApplied;
                    //RT.Items[i].BasicCalibrationResult.WeightApplied = suma + CalResult;
                    if (RT.Items[i].WeightSets == null)
                    {
                        RT.Items[i].WeightSets = new List<WeightSet>();
                    }
                    foreach (var ws in WeightSetList)
                    {
                        if (RT?.Items[i]?.TestPoint?.NominalTestPoit != 0) //&& RT.Items[i].TestPoint.TestPointID != 0
                        {

                            var result11 = RT.Items[i].WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                            if (result11 == null)
                            {
                                RT.Items[i].WeightSets.Add(ws);
                            }
                        }
                        else
                        {
                            await ShowToast("The nominal value is zero. ", Blazed.Controls.Toast.ToastLevel.Warning);
                        }

                    }
                }

                //for (int i = 0; i < RT.Items.Count; i++)
                //{
                //    foreach (var ws in WeightSetList)
                //    {
                //        var result11 = RT.Items[i].WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                //        if(result11 != null)
                //        {
                //            RT.Items[i].WeightSets.Remove(result11);
                //        }

                //    }
                //}

                //item.WeightSets = WeightSetList;
                foreach (var ws in RemoveList)
                {
                    var result11 = item.WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                    if (result11 != null)
                    {
                        item.WeightSets.Remove(result11);
                    }
                }
                var decimalNumber = (int)WorkOrderItemCreate.eq.DecimalNumber;
                for (int i = cont; i < RT.Items.Count; i++)
                {
                    double suma = 0;
                    //RT.Items[i].BasicCalibrationResult.WeightApplied = suma + CalResult;
                    if (RT.Items[i].WeightSets != null && RT?.Items[i]?.TestPoint?.NominalTestPoit != 0)
                    {
                        foreach (var ws in RT.Items[i].WeightSets)
                        {
                            suma = suma + ws.WeightNominalValue;
                        }


                        RT.Items[i].BasicCalibrationResult.WeightApplied = Math.Round(r.CalculateWeight, decimalNumber);
                    }
                    else
                    {
                        RT.Items[i].BasicCalibrationResult.WeightApplied = 0;
                    }


                }

                //foreach (var itemr in RT.Items)
                //{
                //    item.BasicCalibrationResult.WeightApplied = r.CalculateWeight;//totalWeight;
                //}

            }

            //StateHasChanged();


            //_EquipmentTemplateID = EmptyValidationDictionary["EquipmentTemplateID"].ToString();
            //_Model = EmptyValidationDictionary["Model"].ToString();
            //_Name = EmptyValidationDictionary["Name"].ToString();
            //_Manufacturer = EmptyValidationDictionary["Manufacturer"].ToString();


            //eq.EquipmentTemplateId = Convert.ToInt32(EmptyValidationDictionary["EquipmentTemplateID"].ToString());
            //_message = "";


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

        public void CloseSetTestPoint(ChangeEventArgs arg)
        {

            if (arg.Value != null)
            {

                ModalResult result = (ModalResult)arg.Value;

                if (result.Data != null)
                {

                    EquipmentTemplate e = (EquipmentTemplate)result.Data;

                    //uriHelper.NavigateTo(uriHelper.Uri, forceLoad: true);
                    //eq.PieceOfEquipment.EquipmentTemplate = e;

                    //StateHasChanged();

                }

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

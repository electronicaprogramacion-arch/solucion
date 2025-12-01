using Blazed.Controls;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Helper;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;


namespace BlazorApp1.Blazor.Pages.Order.Calibrations
{
    public class EccentricityComponentBase : CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<WorkOrderDetail>
    {

        public string Name { get; set; }

        public IPieceOfEquipmentService<CallContext> PoeServices { get; set; }

        [CascadingParameter(Name = "CascadeParam1")]
        public dynamic WorkOrderItemCreate { get; set; }

        public BasicCalibrationResult RowExecute(BasicCalibrationResult row)
        {
            int cont = 1;
            foreach (var item in RT.Items)
            {
                item.Position = cont;
                cont = cont + 1;
            }


            return row;
        }

        public string TextNewValue { get; set; }

        BasicCalibrationResult DefaultNew()
        {
            //if (string.IsNullOrEmpty(TextNewValue))
            //{
            //    return null;
            //}

            BasicCalibrationResult lin = new BasicCalibrationResult();

            string confirmed = "0";

            try
            {
                if (RT.Items != null && RT.Items.Count >= 0)
                {


                    confirmed = LIST[0].WeightApplied.ToString();

                }
                else
                {
                    confirmed = "0";//await JSRuntime.InvokeAsync<string>("prompt", "Enter a value");

                }

            }
            catch (Exception ex)
            {

            }

            try
            {
                lin.WeightApplied = Convert.ToDouble(confirmed);
            }
            catch (Exception ex)
            {

            }
            if (RT.Items != null && RT.Items.Count > 0)
            {
                lin.UnitOfMeasureID = RT.Items[0].UnitOfMeasureID;
            }
            else
            {
                lin.UnitOfMeasureID = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasureID.Value;
            }

            if (RT.Items != null)
            {
                lin.Position = RT.Items.Count + 1;
            }

            lin.SequenceID = RT.Items.Count + 1;
            lin.WorkOrderDetailId = WorkOrderItemCreate.eq.WorkOrderDetailID;
            lin.CalibrationSubTypeId = new Eccentricity().CalibrationSubTypeId;
            lin.Resolution = WorkOrderItemCreate.eq.Resolution;

            lin.UpperTolerance = lin.WeightApplied + lin.Resolution;
            lin.LowerTolerance = lin.WeightApplied - lin.Resolution;



            IsNewItem = true;
            return lin;

        }


        [Parameter]
        public bool Enabled { get; set; } = false;

        public string Description { get; set; } = "";

        // [Inject] public Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }


        [Parameter]
        public ResponsiveTable<BasicCalibrationResult> RT { get; set; } = new ResponsiveTable<BasicCalibrationResult>();


       


        public CalibrationSaaS.Domain.Aggregates.Entities.Eccentricity Entity { get; set; } = new CalibrationSaaS.Domain.Aggregates.Entities.Eccentricity();

        [Inject] public AppStateCompany AppState { get; set; }

        [Parameter]
        public Status CurrentStatus { get; set; } = new Status();

        public EditContext editContext { get; set; }

        //[Parameter]
        //public WorkOrderDetail eq { get; set; } = new WorkOrderDetail();


        public List<BasicCalibrationResult> LIST { get; set; } = new List<CalibrationSaaS.Domain.Aggregates.Entities.BasicCalibrationResult>();


        public BlazorApp1.Blazor.LTI.Scale.WeightSetComponent WeightSetComponent { get; set; } //= new WeightSetComponent();

        public PieceOfEquipment poq { get; set; } = new PieceOfEquipment();


        [Parameter]
        public List<WeightSet> WeightSetList2 { get; set; } = new List<WeightSet>();

        protected override async Task OnParametersSetAsync()
        {
            //var resultsetw = await _assetsServices.GetWeightSetXPoE(poq);

            //WeightSetList2 = resultsetw.ToList();

            //StateHasChanged();
        }


        int WeightSetListCount = 0;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {


            try
            {
                WeightSetList2 = new List<WeightSet>();

                if (firstRender && WorkOrderItemCreate.eq != null && WorkOrderItemCreate.eq.WOD_Weights != null)
                {

                    //foreach (var w in eq.WOD_Weights)
                    //{
                    //    WeightSetList2.Add(w.WeightSet);
                    //}


                    if (WeightSetComponent != null && WeightSetList2 != null)
                    {


                        await WeightSetComponent.Show(WeightSetList2);

                    }


                }


            }
            catch (Exception ex)
            {

            }


            //if (firstRender && eq != null && (eq?.BalanceAndScaleCalibration?.Eccentricity != null) && eq?.BalanceAndScaleCalibration?.Eccentricity?.BasicCalibrationResults != null)
            //{


            //    Entity = Querys.WOD.GetEccentricityList(eq);//eq.BalanceAndScaleCalibration.Eccentricity;
            //    if (Entity.BasicCalibrationResults != null)
            //    {
            //        LIST = Entity.BasicCalibrationResults.ToList();
            //        Entity.BasicCalibrationResults = LIST;
            //        RT.Clear();
            //        foreach (var item in LIST)
            //        {
            //            RT.SaveNewItem(item);
            //        }
            //        if (!IsWeightLoaded)
            //        {
            //            //aTimer.Stop();
            //            //aTimer.Start();
            //            //IniCalculated = 2;
            //            await Calculate();
            //        }
            //    }


            //}
            //else
            //{
            //    if (eq?.BalanceAndScaleCalibration?.Eccentricity == null
            //        && firstRender
            //        && eq != null
            //        && eq?.PieceOfEquipment?.EquipmentTemplate != null
            //   && eq.PieceOfEquipment.EquipmentTemplate.TestGroups != null
            //   && eq.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints.Count > 0
            //   && eq.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints
            //   .Where(x => x.CalibrationType.ToLower() == "eccentricity").FirstOrDefault() != null
            //    || eq?.BalanceAndScaleCalibration?.Eccentricity == null
            //    && firstRender

            //    && eq.PieceOfEquipment != null
            //    && eq?.PieceOfEquipment?.TestGroups?.Count > 0
            //    && eq?.PieceOfEquipment?.TestGroups.ElementAtOrDefault(0).TestPoints
            //    .Where(x => x.CalibrationType.ToLower() == "eccentricity").FirstOrDefault() != null
            //    )
            //    {
            //        Console.WriteLine("CreateEccentricityList");
            //        var re = Querys.WOD.CreateEccentricityList(eq);

            //        Entity = re;

            //        if (re != null && re?.BasicCalibrationResults != null)
            //        {
            //            LIST = Entity.BasicCalibrationResults.ToList();  //list[0].BasicCalibrationResults.ToList();

            //            RT.Clear();
            //            foreach (var item in LIST)
            //            {
            //                item.Resolution = eq.Resolution;
            //                RT.SaveNewItem(item);
            //            }
            //        }

            //        if (!IsWeightLoaded)
            //        {
            //            //aTimer.Stop();
            //            //aTimer.Start();
            //            //IniCalculated = 2;
            //            await Calculate();
            //        }


            //    }

            //}

            await base.OnAfterRenderAsync(firstRender);
        }

        public bool ShowGrid { get; set; }
        protected override async Task OnInitializedAsync()
        {



            editContext = new EditContext(Entity);

            //LIST = await Task.FromResult(TenPonints());
            aTimer = new System.Timers.Timer(400);

            aTimer.AutoReset = false;


            aTimer.Elapsed += OnUserFinish;

            await base.OnInitializedAsync();

            if (WorkOrderItemCreate?.eq != null && (WorkOrderItemCreate?.eq?.BalanceAndScaleCalibration?.Eccentricity != null) && WorkOrderItemCreate?.eq?.BalanceAndScaleCalibration?.Eccentricity?.TestPointResult != null)
            {


                Entity = Querys.WOD.GetEccentricityList(WorkOrderItemCreate.eq);//eq.BalanceAndScaleCalibration.Eccentricity;
                if (Entity.TestPointResult != null)
                {
                    LIST = Entity.TestPointResult.ToList();
                    Entity.TestPointResult = LIST;
                    RT.Clear();
                    foreach (var item in LIST)
                    {

                        RT.SaveNewItem(item);
                    }

                    if (!IsWeightLoaded)
                    {
                        //aTimer.Stop();
                        //aTimer.Start();
                        //IniCalculated = 2;
                        await Calculate();
                    }


                }


            }
            else
            {
                if (WorkOrderItemCreate != null && WorkOrderItemCreate?.eq?.BalanceAndScaleCalibration?.Eccentricity == null

                    && WorkOrderItemCreate?.eq != null
                    && WorkOrderItemCreate?.eq?.PieceOfEquipment?.EquipmentTemplate != null
               && WorkOrderItemCreate?.eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups != null
               && WorkOrderItemCreate?.eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups?.ElementAtOrDefault(0)?.TestPoints?.Count > 0
               && ((ICollection<TestPoint>)(WorkOrderItemCreate?.eq?.PieceOfEquipment?.EquipmentTemplate?.TestGroups?.ElementAtOrDefault(0)?.TestPoints))
               .Where(x => x.CalibrationType.ToLower() == "eccentricity").FirstOrDefault() != null
                || WorkOrderItemCreate?.eq?.BalanceAndScaleCalibration?.Eccentricity == null


                && WorkOrderItemCreate?.eq?.PieceOfEquipment != null
                && WorkOrderItemCreate?.eq?.PieceOfEquipment?.TestGroups?.Count > 0
                && ((ICollection<TestPoint>)WorkOrderItemCreate?.eq?.PieceOfEquipment?.TestGroups?.ElementAtOrDefault(0)?.TestPoints)
                .Where(x => x.CalibrationType.ToLower() == "eccentricity").FirstOrDefault() != null
                )
                {
                    Console.WriteLine("CreateEccentricityList");
                    var re = Querys.WOD.CreateEccentricityList(WorkOrderItemCreate.eq, !IsOnline);

                    Entity = re;

                    if (re != null && re?.TestPointResult != null)
                    {
                        LIST = Entity.TestPointResult.ToList();  //list[0].BasicCalibrationResults.ToList();

                        RT.Clear();
                        foreach (var item in LIST)
                        {
                            item.Resolution = WorkOrderItemCreate.eq.Resolution;
                            RT.SaveNewItem(item);
                        }
                    }

                    if (!IsWeightLoaded)
                    {
                        //aTimer.Stop();
                        //aTimer.Start();
                        //IniCalculated = 2;
                        await Calculate();
                    }


                }

            }

            ShowGrid = true;

        }




        public List<CalibrationSaaS.Domain.Aggregates.Entities.BasicCalibrationResult> TenPonints()
        {
            List<CalibrationSaaS.Domain.Aggregates.Entities.BasicCalibrationResult> listlienarity = new List<CalibrationSaaS.Domain.Aggregates.Entities.BasicCalibrationResult>();

            for (int i = 0; i < 5; i++)
            {
                listlienarity.Add(new CalibrationSaaS.Domain.Aggregates.Entities.BasicCalibrationResult { Position = i });
            }


            return listlienarity;


        }

        //public async Task SetFivePoints()
        //{
        //    LIST = await Task.FromResult(FivePonints());

        //    StateHasChanged();
        //}

        public void ChangeWindow(ChangeEventArgs e)
        {


        }

        [CascadingParameter] public IModalService Modal { get; set; }
        public string _message { get; set; }


        [Parameter]
        public string HideElement { get; set; } = "modalEccentricity";
        //Logger.LogDebug(Group.Name);
        public async Task SelectWeight(CalibrationSaaS.Domain.Aggregates.Entities.Eccentricity item)
        {
            //Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
            //List<EquipmentTemplate> listEquipment;

            var parameters = new ModalParameters();
            //parameters.Add("SelectOnly", true);
            //parameters.Add("IsModal", true);



            //parameters.Add("target", item);
            WeightSetList2 = new List<WeightSet>();

            if (item == null || WorkOrderItemCreate.eq?.WOD_Weights == null)
            {
                return;
            }


            WeightSetList2 = new List<WeightSet>();
            foreach (var w in WorkOrderItemCreate.eq.WOD_Weights)
            {
                if (WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure.UnitOfMeasureID == w.WeightSet.UnitOfMeasureID ||
                   (w.WeightSet.UnitOfMeasure.UnitOfMeasureBase != null
                    && w.WeightSet.UnitOfMeasure.UnitOfMeasureBase.UnitOfMeasureBaseID == WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure.UnitOfMeasureBaseID) ||
                    w.WeightSet.UnitOfMeasure != null && w.WeightSet.UnitOfMeasure.UnitOfMeasureBaseID == WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure.UnitOfMeasureBaseID)
                {
                    WeightSetList2.Add(w.WeightSet);
                }




            }

            parameters.Add("target", item);

            parameters.Add("DataSource", WeightSetList2);

            if (item.TestPoint == null)
            {
                item.TestPoint = new TestPoint()
                {
                    UnitOfMeasurementOut = WorkOrderItemCreate.eq.PieceOfEquipment.UnitOfMeasure,
                };
            }
            parameters.Add("TestPoint", item.TestPoint);



            var lista = WeightSetList2;//WeightSetComponent.lstWeightSet;

            parameters.Add("DataSource", lista);
            parameters.Add("TestPoint", Entity.TestPoint);

            var SelWeightSetList = new List<WeightSet>();
            if (RT != null)
            {
                //foreach (var w in RT.Items)
                //{
                if (Entity.WeightSets != null && Entity.WeightSets.Count > 0)
                {
                    foreach (var iw in Entity.WeightSets)
                    {
                        SelWeightSetList.Add(iw);
                    }
                }


                //}
            }

            parameters.Add("SelectWeight", SelWeightSetList);




            if (JSRuntime != null && !string.IsNullOrEmpty(HideElement))
            {
                await JSRuntime.InvokeVoidAsync("hideElement", HideElement);
            }

            var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.LTI.Scale.WeightComponent<Eccentricity>>("Select Weight", parameters);
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

            //List<WeightSet> WeightSetList = (List<WeightSet>)result.Data;


            if (WeightSetList != null)
            {
                Calculate cal = new Calculate();

                //item.AsLeft = 1223;
                var calculate = cal.CalculateEccentricity(WeightSetList, item);

                //int totalWeight = WeightSetList.Sum(pkg => pkg.WeightValue);
                var decimalNumber = (int)WorkOrderItemCreate.eq.DecimalNumber;
                foreach (var itemr in RT.Items)
                {
                    itemr.WeightApplied = Math.Round(r.CalculateWeight, decimalNumber);//totalWeight;
                }

                Entity.WeightSets = WeightSetList;

            }

            StateHasChanged();





        }

        public double Prom { get; set; }

        public double Prom2 { get; set; }

        public void ChangeValue(string Control, ChangeEventArgs arg)
        {
            var prome = RT.Items.Sum(x => x.AsFound);

            var prome2 = RT.Items.Sum(x => x.AsLeft);


            var div = RT.Items.Count;
            if (div != 0)
            {
                Prom = prome / div;

                Prom2 = prome2 / div;
            }



        }


        public async Task ChangeDescripcion(ChangeEventArgs arg)
        {

            Description += ((PieceOfEquipment)arg.Value).PieceOfEquipmentID + " | ";
            var w = ((PieceOfEquipment)arg.Value).WeightSets;
            WeightSetList2 = w.ToList();

            if (WeightSetList2 != null)
            {


                WeightSetComponent.Show(w.ToList());

            }
            //if (WeightSetList2 != null)
            //{
            //    var w = ((PieceOfEquipment)arg.Value).WeightSets;

            //    WeightSetComponent.Show(w.ToList());

            //}

        }

        public string TextValue { get; set; }
        public async Task SaveACC()
        {
            await ShowProgress();
            var tt = RT.Items;
            string res = TextValue;//await RT.Prompt();
            if (string.IsNullOrEmpty(res))
            {
                res = "0";
            }
            int cont = 0;
            foreach (var item in tt)
            {
                try
                {
                    cont = cont + 1;
                    var weigth = Convert.ToDouble(res);
                    item.WeightApplied = weigth;
                    item.Position = cont;
                }
                catch (Exception ex)
                {
                }

            }
            Console.WriteLine("SaveACC");
            LIST = tt;
            await Calculate();
            StateHasChanged();
            await CloseProgress();
        }

        public async Task AssignWeight()
        {
            //weightsets
            //var a = WeightSetComponent..Items;

            //var b = LIST;

            ////var c = b.Select(x => x.TestPoint);


            StateHasChanged();


        }
        public async Task AddNewItem()
        {

            await RT.SaveNewItem(DefaultNew());

            WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Eccentricity.TestPointResult = RT.Items;

        }

        public bool IsNewItem { get; set; }

        public List<PieceOfEquipment> Standards { get; set; }
        public async Task Calculate()
        {



            int lineerror = 0;

            try
            {

                IsNewItem = false;

                if (RT == null || RT.Items == null || RT.Items.Count == 0)
                {
                    return;
                }
                lineerror = 1;
                var linearities = RT.Items;
                if (WorkOrderItemCreate.eq?.BalanceAndScaleCalibration == null)
                {
                    WorkOrderItemCreate.eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
                    WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Eccentricity = new Eccentricity();
                    //eq.BalanceAndScaleCalibration.Eccentricity.BasicCalibrationResults = new Eccentricity();
                }
                if (WorkOrderItemCreate.eq?.BalanceAndScaleCalibration.Eccentricity == null)
                {
                    WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Eccentricity = new Eccentricity();
                }
                WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Eccentricity.TestPointResult = linearities;
                lineerror = 2;


                if (!IsWeightLoaded)
                {
                    //var p = new CallOptions();

                    //WorkOrderDetailGrpc wd = new WorkOrderDetailGrpc(Service, DbFactory, p);

                    //var resul = await wd.GetConfiguredWeights(eq);

                    //eq.BalanceAndScaleCalibration = resul.BalanceAndScaleCalibration;

                    IsWeightLoaded = true;
                }

                lineerror = 3;



                var result = Querys.WOD.CalculateValuesByID(WorkOrderItemCreate.eq, AppState.UnitofMeasureList).GetAwaiter().GetResult();

                lineerror = 4;

                if (result != null)
                {
                    WorkOrderItemCreate.eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

                    RT.Clear();

                    foreach (var item in WorkOrderItemCreate.eq.BalanceAndScaleCalibration.Eccentricity.TestPointResult)
                    {
                        RT.SaveNewItem(item);
                    }

                }

                lineerror = 5;

                StateHasChanged();

            }
            catch (Exception ex)
            {

                await ShowError(ex, "calculate", lineerror.ToString());
            }
            finally
            {
                IsNewItem = false;
            }



        }

        public int IniCalculated { get; set; }
        private static System.Timers.Timer aTimer;
        public bool IsWeightLoaded { get; set; }

        public async Task ChangeControl2(ChangeEventArgs arg, BasicCalibrationResult lin, bool AddWeight = false)
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

                //    await Calculate();


                //    IniCalculated = 1;
                //}





            }
            catch
            {

            }



        }

        public async Task TaskAfterSave(BasicCalibrationResult item, string sav)
        {
            //if (IsNewItem)
            //{
            //    await Calculate();

            //}

            //IsNewItem = false;

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
                });

                IniCalculated = 2;
            }
            catch
            {

            }


            //aTimer.Start();
        }

        public CalibrationSaaS.Domain.Aggregates.Entities.BasicCalibrationResult RowAfterRender(CalibrationSaaS.Domain.Aggregates.Entities.BasicCalibrationResult lin)
        {

            IniCalculated = 1;

            return lin;

        }
        private void HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                JSRuntime.InvokeVoidAsync("preventDefault", e);
            }
        }
    }
}

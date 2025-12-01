using Microsoft.AspNetCore.Components;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics;
using CalibrationSaaS.Infraestructure.Blazor.Helper;

using CalibrationSaaS.Domain.Aggregates.Querys;
using System.Timers;
using System.Collections.Generic;
using CalibrationSaaS.Domain.Aggregates.Entities;
using System.Threading.Tasks;
using Blazored.Modal;
using System.Linq;
using System;
using Microsoft.AspNetCore.Components.Forms;
using Blazed.Controls;
using Microsoft.JSInterop;
using Blazored.Modal.Services;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Order
{
    public partial class RepeabilityComponent:CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<int>
    {

        

    async Task DeleteI(BasicCalibrationResult res)
    {


        await RT.DeleteItem(res);
        if (RT.Items != null)
        {
            int conti = 1;
            foreach (var it in RT.Items)
            {
                it.Position = conti;
                conti = conti + 1;
            }
        }

        StateHasChanged();
    }

    public string TextValue { get; set; }

    [Parameter]
    public bool Enabled { get; set; } = false;

    [Parameter]
    public Status CurrentStatus { get; set; }

    public string Description { get; set; } = "";

    //[Inject] public Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }

    public bool LabelDown { get; set; } = false;


    public ResponsiveTable<BasicCalibrationResult> RT { get; set; } = new ResponsiveTable<BasicCalibrationResult>();


    [Parameter]
    public string EntityID { get; set; }


    public Repeatability Entity = new Repeatability();

    [Inject] public AppStateCompany AppState { get; set; }

    //[Parameter]
    //public Status CurrentStatus { get; set; } = new Status();

    public EditContext editContext { get; set; }

    [Parameter]
    public WorkOrderDetail eq { get; set; } = new WorkOrderDetail();


    public List<BasicCalibrationResult> LIST { get; set; } = new List<Domain.Aggregates.Entities.BasicCalibrationResult>();


    public WeightSetComponent WeightSetComponent { get; set; } = new WeightSetComponent();

    PieceOfEquipment poq = new PieceOfEquipment();

    public bool EnableWeightSet { get; set; } = true;

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

            if (firstRender && eq != null && eq.WOD_Weights != null)
            {



                if (WeightSetComponent != null && WeightSetList2 != null)
                {


                    await WeightSetComponent.Show(WeightSetList2);

                }


            }
        }
        catch (Exception ex)
        {

        }


        if (firstRender && eq != null && (eq?.BalanceAndScaleCalibration?.Repeatability?.BasicCalibrationResults != null))
        {
            Entity = Querys.WOD.GetRepeatabilityList(eq);//eq.BalanceAndScaleCalibration.Eccentricity;
            LIST = Entity.BasicCalibrationResults.ToList();

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
        else
        {
            if (eq?.BalanceAndScaleCalibration?.Repeatability == null
                && firstRender && eq != null
                && eq.PieceOfEquipment != null
                && eq.PieceOfEquipment.EquipmentTemplate != null
           && eq.PieceOfEquipment.EquipmentTemplate.TestGroups != null
           && eq.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints.Count > 0
            && eq.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints
           .Where(x => x.CalibrationType.ToLower() == "repeatability").FirstOrDefault() != null
           || eq != null && eq?.BalanceAndScaleCalibration?.Repeatability == null && firstRender
          && eq.PieceOfEquipment != null && eq?.PieceOfEquipment?.TestGroups != null
          && eq.PieceOfEquipment.TestGroups.ElementAtOrDefault(0).TestPoints
           .Where(x => x.CalibrationType.ToLower() == "repeatability").FirstOrDefault() != null
          )
            {


                var re = Querys.WOD.CreateRepeatabilityList(eq);

                if (re != null && re?.BasicCalibrationResults != null)
                {
                    Entity = re;
                    LIST = Entity.BasicCalibrationResults.ToList();  //list[0].BasicCalibrationResults.ToList();

                    RT.Clear();
                    foreach (var item in LIST)
                    {
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

        await base.OnAfterRenderAsync(firstRender);
    }


    protected override async Task OnInitializedAsync()
    {
        editContext = new EditContext(Entity);

        aTimer = new System.Timers.Timer(400);

        aTimer.Elapsed += OnUserFinish;

        await base.OnInitializedAsync();



    }


    //public List<Repeatability> FivePonints()
    //{
    //    List<Domain.Aggregates.Entities.Repeatability> listlienarity = new List<Domain.Aggregates.Entities.Repeatability>();

    //    for (int i = 0; i < 5; i++)
    //    {
    //        listlienarity.Add(new Domain.Aggregates.Entities.Linearity { LinearityId = i });
    //    }

    //    listlienarity.ElementAtOrDefault(0).Label = "10";
    //    listlienarity.ElementAtOrDefault(0).Label = "20";
    //    listlienarity.ElementAtOrDefault(0).Label = "30";
    //    listlienarity.ElementAtOrDefault(0).Label = "50";
    //    listlienarity.ElementAtOrDefault(0).Label = "100";

    //    return listlienarity;

    //}

    public List<Domain.Aggregates.Entities.BasicCalibrationResult> TenPonints()
    {
        List<Domain.Aggregates.Entities.BasicCalibrationResult> listlienarity = new List<Domain.Aggregates.Entities.BasicCalibrationResult>();

        for (int i = 0; i < 5; i++)
        {
            listlienarity.Add(new Domain.Aggregates.Entities.BasicCalibrationResult { Position = i });
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
    //Logger.LogDebug(Group.Name);
    [Parameter]
    public string HideElement { get; set; } = "modalRepeatibility";
    public async Task SelectWeight(Repeatability item)
    {
        //Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();
        //List<EquipmentTemplate> listEquipment;

        var parameters = new ModalParameters();
        //parameters.Add("SelectOnly", true);
        //parameters.Add("IsModal", true);



        //parameters.Add("target", item);
        WeightSetList2 = new List<WeightSet>();
        foreach (var w in eq.WOD_Weights)
        {
            WeightSetList2.Add(w.WeightSet);
        }

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

        var messageForm = Modal.Show<WeightComponent<Repeatability>>("Select Weight", parameters);
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

        // List<WeightSet> WeightSetList = (List<WeightSet>)result.Data;


        if (WeightSetList != null)
        {
            Calculate cal = new Calculate();

            //item.AsLeft = 1223;
            var calculate = cal.InlineCalculateRepeatibility(WeightSetList, item);

            //item.WeightApplied = calculate.WeightApplied;

            //item.Weights = WeightSetList;

            //item.AsLeft = 23;

            //RT.Items[RT.Items.FindIndex(ind => ind.LinearityId== item.LinearityId)] = cal.InlineCalculate(WeightSetList,item);

            //var teamTotalScores =
            //from player in WeightSetList
            //group player by player.PieceOfEquipmentId into playerGroup
            //select new
            //{
            //    Team = playerGroup.Key,
            //    TotalScore = playerGroup.Sum(x => x.WeightValue),
            //};

            //int totalWeight = WeightSetList.Sum(pkg => pkg.WeightValue);


            foreach (var itemr in RT.Items)
            {
                itemr.WeightApplied = r.CalculateWeight;//totalWeight;
            }
            Entity.WeightSets = WeightSetList;


        }

        StateHasChanged();





    }

    public double Prom { get; set; }

    public double Prom2 { get; set; }

    void ChangeValue(string Control, ChangeEventArgs arg)
    {

        try
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
        catch (Exception ex)
        {

        }






    }


    async Task ChangeDescripcion(ChangeEventArgs arg)
    {

        Description += ((PieceOfEquipment)arg.Value).PieceOfEquipmentID + " | ";

        var w = ((PieceOfEquipment)arg.Value).WeightSets;
        WeightSetList2 = w.ToList();

        if (WeightSetList2 != null)
        {


            await WeightSetComponent.Show(w.ToList());

        }

    }

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
            if (RT.Items != null && RT.Items.Count > 0)
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
            lin.UnitOfMeasureID = eq.PieceOfEquipment.UnitOfMeasureID.Value;
        }
        int conti = 1;
        foreach (var it in RT.Items)
        {
            it.Position = conti;
            conti = conti + 1;
        }

        if (RT.Items != null)
        {
            lin.Position = conti;
        }

        lin.CalibrationSubTypeId = new Repeatability().CalibrationSubTypeId;
        lin.SequenceID = conti;

        IsNewItem = true;

        return lin;

    }


    public async Task SaveACC()
    {
        await ShowProgress();
        var tt = RT.Items;
        string res = TextValue;//await RT.Prompt();
        if (string.IsNullOrEmpty(res))
        {
            res = "0";
        }
        foreach (var item in tt)
        {
            try
            {
                var weigth = Convert.ToDouble(TextValue);
                item.WeightApplied = weigth;
            }
            catch (Exception ex)
            {
            }

        }

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

    string BoudValues(Linearity lin)
    {



        return "";

    }

    public async Task AddNewItem()
    {

        await RT.SaveNewItem(DefaultNew());


    }


    public bool IsNewItem { get; set; }

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

            var linearities = RT.Items;
            if (eq?.BalanceAndScaleCalibration == null)
            {
                eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
                eq.BalanceAndScaleCalibration.Repeatability = new Repeatability();
            }
            if (eq?.BalanceAndScaleCalibration?.Repeatability == null)
            {
                eq.BalanceAndScaleCalibration.Repeatability = new Repeatability();
            }
            lineerror = 2;
            eq.BalanceAndScaleCalibration.Repeatability.BasicCalibrationResults = linearities;
            lineerror++;


            if (!IsWeightLoaded)
            {
                //var p = new CallOptions();

                //WorkOrderDetailGrpc wd = new WorkOrderDetailGrpc(Service, DbFactory, p);

                //var resul = await wd.GetConfiguredWeights(eq);

                //eq.BalanceAndScaleCalibration = resul.BalanceAndScaleCalibration;

                IsWeightLoaded = true;
            }

            lineerror++;
            var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();
            lineerror++;
            if (result != null)
            {
                eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

                RT.Clear();
                lineerror++;
                foreach (var item in eq.BalanceAndScaleCalibration.Repeatability.BasicCalibrationResults)
                {
                    RT.SaveNewItem(item);
                }

            }

            lineerror++;

            StateHasChanged();

        }
        catch (Exception ex)
        {

            await ShowError(ex.Message + " repea " + lineerror);
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
        if (IsNewItem)
        {
            await Calculate();

        }

        IsNewItem = false;

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
    }

    public Domain.Aggregates.Entities.BasicCalibrationResult RowAfterRender(Domain.Aggregates.Entities.BasicCalibrationResult lin)
    {

        IniCalculated = 1;

        return lin;

    }



    }
}

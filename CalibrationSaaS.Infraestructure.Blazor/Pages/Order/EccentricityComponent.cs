using CalibrationSaaS.Domain.Aggregates.Entities;
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
    public partial class EccentricityComponent:CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<WorkOrderDetail>
    {
         BasicCalibrationResult RowExecute(BasicCalibrationResult row)
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

        try {
            lin.WeightApplied = Convert.ToDouble(confirmed);
        }
        catch(Exception ex)
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

        if(RT.Items != null)
        {
            lin.Position = RT.Items.Count + 1;
        }

        lin.SequenceID= RT.Items.Count + 1;
        lin.WorkOrderDetailId = eq.WorkOrderDetailID;
        lin.CalibrationSubTypeId = new Eccentricity().CalibrationSubTypeId;
        lin.Resolution = eq.Resolution;
        IsNewItem = true;
        return lin;

    }


    [Parameter]
    public bool Enabled { get; set; } = false;

    public string Description { get; set; } = "";

    // [Inject] public Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }


    [Parameter]
    public ResponsiveTable<BasicCalibrationResult> RT { get; set; } = new ResponsiveTable<BasicCalibrationResult>();


    [Parameter]
    public string EntityID { get; set; }


    public Domain.Aggregates.Entities.Eccentricity Entity = new Domain.Aggregates.Entities.Eccentricity();

    [Inject] public AppStateCompany AppState { get; set; }

    [Parameter]
    public Domain.Aggregates.Entities.Status CurrentStatus { get; set; } = new Domain.Aggregates.Entities.Status ();

    public EditContext editContext { get; set; }

    [Parameter]
    public WorkOrderDetail eq { get; set; } = new WorkOrderDetail();


    public List<BasicCalibrationResult> LIST { get; set; } = new List<Domain.Aggregates.Entities.BasicCalibrationResult>();


    public WeightSetComponent WeightSetComponent { get; set; } //= new WeightSetComponent();

    PieceOfEquipment poq = new PieceOfEquipment();


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


        if (firstRender && eq != null && (eq?.BalanceAndScaleCalibration?.Eccentricity != null))
        {


            Entity = Querys.WOD.GetEccentricityList(eq);//eq.BalanceAndScaleCalibration.Eccentricity;
            if(Entity?.BasicCalibrationResults != null)
                {
                   
                    LIST = Entity.BasicCalibrationResults.ToList();
            
                Entity.BasicCalibrationResults = LIST;
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
            if (eq?.BalanceAndScaleCalibration?.Eccentricity ==null
                && firstRender
                && eq != null
                && eq?.PieceOfEquipment?.EquipmentTemplate != null
           && eq.PieceOfEquipment.EquipmentTemplate.TestGroups != null
           && eq.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints.Count > 0
           && eq.PieceOfEquipment.EquipmentTemplate.TestGroups.ElementAtOrDefault(0).TestPoints
           .Where(x=>x.CalibrationType.ToLower()== "eccentricity").FirstOrDefault() != null
            || eq?.BalanceAndScaleCalibration?.Eccentricity == null
            && firstRender

            && eq.PieceOfEquipment != null
            && eq?.PieceOfEquipment?.TestGroups?.Count > 0
            && eq?.PieceOfEquipment?.TestGroups.ElementAtOrDefault(0).TestPoints
            .Where(x => x.CalibrationType.ToLower() == "eccentricity").FirstOrDefault() != null
            )
            {
                //Console.WriteLine("CreateEccentricityList");
                var re = Querys.WOD.CreateEccentricityList(eq);

                Entity = re;

                if (re != null && re?.BasicCalibrationResults != null)
                {
                    LIST = Entity.BasicCalibrationResults.ToList();  //list[0].BasicCalibrationResults.ToList();

                    RT.Clear();
                    foreach (var item in LIST)
                    {
                        item.Resolution = eq.Resolution;
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

        //LIST = await Task.FromResult(TenPonints());
        aTimer = new System.Timers.Timer(400);

        aTimer.Elapsed += OnUserFinish;

        await base.OnInitializedAsync();
    }




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


    [Parameter]
    public string HideElement { get; set; } = "modalEccentricity";
    //Logger.LogDebug(Group.Name);
    public async Task SelectWeight(Domain.Aggregates.Entities.Eccentricity item)
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

        var messageForm = Modal.Show<WeightComponent<Eccentricity>>("Select Weight", parameters);
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
        var prome = RT.Items.Sum(x => x.AsFound);

        var prome2 = RT.Items.Sum(x => x.AsLeft);


        var div = RT.Items.Count;
        if (div != 0)
        {
            Prom = prome / div;

            Prom2 = prome2 / div;
        }



    }


    void ChangeDescripcion(ChangeEventArgs arg)
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

    public string  TextValue { get; set; }
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
        //Console.WriteLine("SaveACC");
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
            lineerror = 1;
            var linearities = RT.Items;
            if (eq?.BalanceAndScaleCalibration == null)
            {
                eq.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
                eq.BalanceAndScaleCalibration.Eccentricity = new Eccentricity();
                //eq.BalanceAndScaleCalibration.Eccentricity.BasicCalibrationResults = new Eccentricity();
            }
            if (eq?.BalanceAndScaleCalibration.Eccentricity == null)
            {
                eq.BalanceAndScaleCalibration.Eccentricity = new Eccentricity();
            }
            eq.BalanceAndScaleCalibration.Eccentricity.BasicCalibrationResults = linearities;
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

            var result = Querys.WOD.CalculateValuesByID(eq).GetAwaiter().GetResult();

            lineerror = 4;

            if (result != null)
            {
                eq.BalanceAndScaleCalibration = result.BalanceAndScaleCalibration;

                RT.Clear();

                foreach (var item in eq.BalanceAndScaleCalibration.Eccentricity.BasicCalibrationResults)
                {
                    RT.SaveNewItem(item);
                }

            }

            lineerror = 5;

            StateHasChanged();

        }
        catch (Exception ex)
        {

            await ShowError(ex.Message + " ecce " + lineerror);
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

    public Domain.Aggregates.Entities.BasicCalibrationResult RowAfterRender(Domain.Aggregates.Entities.BasicCalibrationResult lin)
    {

        IniCalculated = 1;

        return lin;

    }




    }
}

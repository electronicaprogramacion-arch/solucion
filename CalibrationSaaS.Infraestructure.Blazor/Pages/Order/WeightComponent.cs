using Blazed.Controls;
using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Infraestructure.Blazor.Helper;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Order
{
    public partial class WeightComponent<Target> : ComponentBase, IDisposable where Target : new()
    {

        [Parameter]
    public List<WeightSet> SelectWeight { get; set; }

    [Parameter]
    public TestPoint TestPoint { get; set; }

    public string LabelConversion { get; set; }

    [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }

    int cont = 0;

    [Parameter]
    public Target target { get; set; } = new Target();

    [Parameter]
    public List<WeightSet> SelectItem { get; set; } = new List<WeightSet>();

    [Parameter]
    public List<WeightSet> DesSelectItem { get; set; } = new List<WeightSet>();

    [Parameter]
    public List<WeightSet> DataSource { get; set; } = new List<WeightSet>();


    [Inject] CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany AppState { get; set; }

    public ResponsiveTable<WeightSet>
                 RT
    { get; set; } = new ResponsiveTable<WeightSet>();


    public string ErrorMessage { get; set; }


    protected void ChangeWindow(ChangeEventArgs item)
    {
        ChangeEventArgs e = new ChangeEventArgs();

        e.Value = item.Value;

        //OnChangeControl.InvokeAsync(e);
    }
    double result = 0;
    Calculate c = new Calculate();
    public async Task AddItem(object e, WeightSet add)
    {
        string UOMABB = "";
        //oly one
        //await Clear();
        ////

        try
        {
            bool remove = false;
            bool additem = false;
            if (!SelectItem.Contains(add))
            {
                SelectItem.Add(add);
                additem = true;

            }
            else
            {
                SelectItem.Remove(add);
                //SelectWeight.Remove(add);
                remove = true;
            }

            if (remove)
            {
                DesSelectItem.Add(add);
            }

            if (additem)
            {
                var c1 = DesSelectItem.Where(x => x.PieceOfEquipmentID == add.PieceOfEquipmentID && x.WeightSetID == add.WeightSetID).FirstOrDefault();

                if (c1 != null)
                {
                    DesSelectItem.Remove(c1);
                }
            }


            Calculate c = new Helper.Calculate();

            result = 0;
                
                if(TestPoint != null)
                {
                     foreach (var item in SelectItem)
                     {


                        result = result + item.ConversionMethod(TestPoint, AppState.UnitofMeasureList, out UOMABB);


                     }
                }

                LabelConversion = result.ToString() + " " + UOMABB;

                SelectWeight = SelectItem;

                StateHasChanged();
           
        }
        catch (Exception ex)
        {
            LabelConversion = ex.Message;
            return;
        }
       
    }

    //     public async Task RemoveItem(object e, WeightSet add)
    //{
    //    string UOMABB = "";
    //    //oly one
    //    //await Clear();
    //    ////

    //    try
    //    {
    //        bool remove = false;
    //        bool additem = false;
    //        if (!SelectItem.Contains(add))
    //        {
    //            SelectItem.Add(add);
    //            additem = true;

    //        }
    //        else
    //        {
    //            SelectItem.Remove(add);
    //            //SelectWeight.Remove(add);
    //            remove = true;
    //        }

    //        if (remove)
    //        {
    //            DesSelectItem.Add(add);
    //        }

    //        if (additem)
    //        {
    //            var c1 = DesSelectItem.Where(x => x.PieceOfEquipmentID == add.PieceOfEquipmentID && x.WeightSetID == add.WeightSetID).FirstOrDefault();

    //            if (c1 != null)
    //            {
    //                DesSelectItem.Remove(c1);
    //            }
    //        }


    //        Calculate c = new Helper.Calculate();

    //        result = 0;
    //            if(TestPoint != null)
    //            {
    //                 foreach (var item in SelectItem)
    //        {


    //            result = result + item.ConversionMethod(TestPoint, AppState.UnitofMeasureList, out UOMABB);


    //        }
    //            }
    //       LabelConversion = result.ToString() + " " + UOMABB;

    //            SelectWeight = SelectItem;

    //       StateHasChanged();
           
    //    }
    //    catch (Exception ex)
    //    {
    //        LabelConversion = ex.Message;
    //        return;
    //    }
       
    //}
    public async Task Clear()
    {
            foreach (var item in SelectItem)
            {
                DesSelectItem.Add(item);
            }
            
       
        SelectWeight.Clear();
        SelectItem.Clear();

    }


    public async Task SelectModal()
    {

        Calculate c = new Helper.Calculate();

        if (c.LinearityValidation(SelectItem, target))
        {
            WeightSetResult res = new WeightSetResult();

            res.WeightSetList = SelectItem;
            res.CalculateWeight = result;
            res.RemoveList = DesSelectItem;
            await BlazoredModal.CloseAsync(ModalResult.Ok(res));

        }
        else
        {
            ErrorMessage = "No valid";

        }
    }


    public string GetName(string item)
    {

        if (string.IsNullOrEmpty(item))
        {
            return "";
        }

        var res = AppState.UnitofMeasureList.Where(x => x.UnitOfMeasureID == Convert.ToInt32(item)).FirstOrDefault();

        return res.Abbreviation;
    }


    protected override async Task OnParametersSetAsync()
    {
            SelectItem = SelectWeight;
            //SelectWeight = new List<WeightSet>();
        
        
        }



        public void Dispose()
        {



        }
    }
}

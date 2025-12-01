using System;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;

using CalibrationSaaS.Infraestructure.Blazor.Services;

using Grpc.Core;
using CalibrationSaaS.Domain.Aggregates.Querys;
using Helpers;
using System.Linq.Expressions; 
using CalibrationSaaS.Domain.Aggregates.Entities;

using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System.Threading.Tasks;
using System;
using Blazed.Controls;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;
using CalibrationSaaS.Infraestructure.Blazor;
namespace BlazorApp1.Blazor.Pages.Basics.TestPoints
{
    public  class RangeTestPoint_base: CalibrationSaaS.Infraestructure.Blazor.KavokuComponentBase<int>
    { 
   [Parameter]
    public Func<RangeTolerance, Task<bool>> DeleteFun { get; set; }


    public async Task<bool> Delete(RangeTolerance range)
    {

        return await DeleteFun(range);

    }


    [Parameter]
    public Func<RangeTolerance, string, Task> AfterActionF { get; set; }


    public async Task AfterAction(RangeTolerance range, string tipo)
    {

        await AfterActionF(range, tipo);

    }



    [Parameter]
    public bool Enabled { get; set; }


    [Parameter]
    public string ID { get; set; }



    [Parameter]
    public string GridID { get; set; }


    [Parameter]
    public int Type { get; set; }

    [Parameter]
    public string Title { get; set; } = "Accuracy";

    public string Message { get; set; }

    public RangeTolerance DefultTestPoint = new RangeTolerance();

    [Inject] public AppState AppState { get; set; }

    public List<RangeTolerance> _Group;

    [Parameter]
    public List<RangeTolerance> Group
    {

        get
        {
            if (_Group != null)
            {
                return _Group.Where(x => x.ToleranceTypeID == Type).ToList();
            }
            else
            {
                return _Group;
            }

        }
        set
        {

            _Group = value;


        }
    }

    public ResponsiveTable<RangeTolerance>
        RT
    { get; set; } = new ResponsiveTable<RangeTolerance>();

    public void NewItem()
    {

    }

    public async Task Show(List<RangeTolerance> group)
    {
        if (group != null && RT != null)
        {
            //if (RT != null && RT.Items != null && group.Count > 0)
            //{
            //    RT.Items.Clear();
            //    RT.ItemList = new List<RangeTolerance>();
            //    RT.ItemsDataSource = new List<RangeTolerance>();
            //}


            _Group = group; //.Where(x => x.ToleranceTypeID == Type).ToList();
            RT.Clear();
            foreach (var item in group)
            {
                RT.SaveNewItem(item);
            }


            StateHasChanged();
        }


    }

    public List<RangeTolerance> Format()
    {

        return _Group;
    }

    public string NewText { get; set; }

    protected override async Task OnInitializedAsync()
    {


        NewText = "Add New " + Title + " Range";
    }


    //Console.WriteLine(Group.Name);
    public void ChangeMax(RangeTolerance range, ChangeEventArgs arg)
    {
        Int32 value;
        var obj = Int32.TryParse(arg.Value.ToString(), out value);
        if (obj)
        {

            if (value <= range.MinValue)
            {
                Message = "the maximum value cannot be less than the minimum value";

            }
            else
            {
                Message = "";

            }
        }


    }

    public void ChangeMin(RangeTolerance range, ChangeEventArgs arg)
    {
        Int32 value;
        var obj = Int32.TryParse(arg.Value.ToString(), out value);

        //if (obj)
        //{
        //    Message = "the minimum value cannot be greater than the maximum value";

        //}
        //else
        //{
        //    Message = "";
        //}

        if (obj)
        {

            if (value > range.MaxValue)
            {
                Message = "the minimum value cannot be greater than the maximum value";

            }
            else
            {
                Message = "";

            }
        }


    }

    public async Task SaveGrid(ChangeEventArgs arg)
    {
        RangeTolerance obj = (RangeTolerance)arg.Value;





    }

    public string DecimalCalculate(RangeTolerance args)
    {

        string result2 = "";
        try
        {
            var val = args.Resolution;
            args.ToleranceTypeID = Type;
            if (val != null)
            {
                var result = NumericExtensions.CalculateDecimalNumber(Convert.ToDecimal(val));
                result2 = ((int)result).ToString();
            }
        }
        catch (Exception ex)
        {
        }

        return result2;
    }

   public  string Bound(RangeTolerance args)
    {
        args.ToleranceTypeID = Type;
        return "";
    }
    }
}

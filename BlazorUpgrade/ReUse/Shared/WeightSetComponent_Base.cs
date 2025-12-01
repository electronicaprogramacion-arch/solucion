using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;

namespace CalibrationSaaS.Infraestructure.Blazor.Shared
{
    public class WeightSetComponent_Base : WeightSetComponentBase
    {

        //public async Task<IEnumerable<WeightSet>> Filter33(WeightSet w)
        //{

        //    ResultSet<WeightSet> r = new ResultSet<WeightSet>();

        //    var a = lstWeightSet.Where(x => x.PieceOfEquipmentID == w.PieceOfEquipmentID).ToList();

        //    r.List = a;
        //    r.PageTotal = a.Count;

        //    return a;

        //}

        public async Task CancelItem(WeightSet arg)
        {

            POE = arg.PieceOfEquipmentID;

            await Grid.FilterList(arg);


        }



        //[Parameter]
        //public Status CurrentStatus { get; set; }


        [Parameter]
        public bool Certified { get; set; }

        public WeightSet DefultTestPoint = new WeightSet();

      
       

        public List<WeightSet> Format()
        {

            return _Group;
        }

        [Parameter]
        public int GroupTestPointID { get; set; }

      

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();    

        }

        //public async Task SelectWeightSet33()
        //{

        //    var parameters = new ModalParameters();
        //    parameters.Add("SelectOnly", true);
        //    parameters.Add("IsModal", true);
        //    parameters.Add("Checkbox", false);
        //    parameters.Add("Indicator", true);
        //    if (!string.IsNullOrEmpty(Option))
        //    {
        //        parameters.Add("Parameter1", Convert.ToInt32(Option));
        //    }
        //    parameters.Add("CalibrationTypeID", WorkOrderItemCreate.eq.CalibrationTypeID);

        //    ModalOptions op = new ModalOptions();
        //    op.ContentScrollable = true;
        //    op.Class = "blazored-modal " + ModalSize.MediumWindow;
           
        //    var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.GenericMethods.POEWeightSet_Select>("Select Standard", parameters, op);
            
        //    var result = await messageForm.Result;
        //    Console.Write("result " + result);


        //    if (!result.Cancelled)
        //        _message = result.Data?.ToString() ?? string.Empty;

        //    if (result.Data == null)
        //    {
        //        return;
        //    }


        //    PieceOfEquipment PieceOfEquipment = (PieceOfEquipment)result.Data;
        //    int conteo = lstWeightSet.Count;
           
        //    if (PieceOfEquipment.WeightSets != null)
        //    {

        //        Description = PieceOfEquipment.PieceOfEquipmentID + " " + PieceOfEquipment.SerialNumber;



        //        foreach (var item in PieceOfEquipment.WeightSets)
        //        {
        //            var a = lstWeightSet.Where(x => x.WeightSetID == item.WeightSetID).FirstOrDefault();

        //            if (a == null)
        //            {
        //                item.PieceOfEquipmentID = PieceOfEquipment.PieceOfEquipmentID;
        //                lstWeightSet.Add(item);
        //            }
        //            else
        //            {

        //            }

        //        }
        //        if (lstWeightSet.Count <= conteo)
        //        {

        //            return;
        //        }


        //        ListPOE.Add(PieceOfEquipment);
        //        var queryLastNames =
        //        from student in lstWeightSet
        //        group student by student.PieceOfEquipmentID into newGroup
        //        orderby newGroup.Key
        //        select newGroup;

        //        if (Group2 == null)
        //        {
        //            Group2 = new List<WeightSet>();
        //        }
        //        Group2.Clear();
        //        foreach (var item3 in queryLastNames)
        //        {
        //            int cont = 0;
        //            foreach (var item4 in item3)
        //            {
        //                if (cont == 0)
        //                {
        //                    Group2.Add(item4);
        //                }
        //                cont = cont + 1;

        //            }
        //        }

 
        //        foreach (var item45 in lstWeightSet)
        //        {
        //            item45.Option = Option;
        //        }

        //        PieceOfEquipment.WeightSets = lstWeightSet;
        //        ChangeEventArgs arg = new ChangeEventArgs();

        //        arg.Value = PieceOfEquipment;

        //        await ChangeAdd(arg);


        //        StateHasChanged();
        //    }
        //}

        //public void ChangeCheckBox(ChangeEventArgs e)
        //{
        //    if (Accredited)
        //    {
        //        Accredited = false;
        //    }
        //    else
        //    {
        //        Accredited = true;
        //    }

        //}

    }
}

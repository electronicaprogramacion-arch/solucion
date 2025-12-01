using Blazored.Modal;
using Blazored.Modal.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using System.Text.RegularExpressions;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Scale
{



    public class WeightSetComponent_Base : WeightSetComponentBase
    {



      





       


        [Parameter]
        public bool Certified { get; set; }




        [Parameter]
        public Func<WeightSet, Task> DeleteFunction { get; set; }


        //public string Description { get; set; } = "";

        //[CascadingParameter] public IModalService Modal { get; set; }

        public WeightSet DefultTestPoint = new WeightSet();








        //public void NewItem()
        //{

        //}
       
        public List<WeightSet> Format()
        {

            return _Group;
        }

        [Parameter]
        public int GroupTestPointID { get; set; }


        public async Task SelectWeightSet2()
        {

            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            parameters.Add("Checkbox", false);
            parameters.Add("Indicator", true);
            if (!string.IsNullOrEmpty(Option))
            {
                parameters.Add("Parameter1", Option);
            }

            parameters.Add("CalibrationTypeID", WorkOrderItemCreate.eq.GetCalibrationTypeID());
            parameters.Add("Accredited", WorkOrderItemCreate.eq.IsAccredited);
            ModalOptions op = new ModalOptions();
            op.ContentScrollable = true;
            op.Class = "blazored-modal " + ModalSize.MediumWindow;
            //PieceOfEquipmentDueDate_Search
            var messageForm = Modal.Show<POEWeightSet_Select>("Select Standard", parameters, op);
            //var messageForm = Modal.Show<CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics.POEWeightSet_Select>("Select Standard", parameters, op);
            var result = await messageForm.Result;
            Console.Write("result " + result);


            if (!result.Cancelled)
                _message = result.Data?.ToString() ?? string.Empty;

            if (result.Data == null)
            {
                return;
            }


            PieceOfEquipment PieceOfEquipment = (PieceOfEquipment)result.Data;
            int conteo = lstWeightSet.Count;

            if (PieceOfEquipment.WeightSets != null)
            {

                Description = PieceOfEquipment.PieceOfEquipmentID + " " + PieceOfEquipment.SerialNumber;



                foreach (var item in PieceOfEquipment.WeightSets)
                {
                    var a = lstWeightSet.Where(x => x.WeightSetID == item.WeightSetID && x.Option == Option).FirstOrDefault();

                    if (a == null)
                    {
                        item.PieceOfEquipmentID = PieceOfEquipment.PieceOfEquipmentID;

                        item.Option = Option;
                        lstWeightSet.Add(item);
                    }
                    else
                    {

                    }

                }
                if (lstWeightSet.Count <= conteo)
                {

                    return;
                }


                ListPOE.Add(PieceOfEquipment);
                var queryLastNames =
                from student in lstWeightSet
                group student by student.PieceOfEquipmentID into newGroup
                orderby newGroup.Key
                select newGroup;

                if (Group2 == null)
                {
                    Group2 = new List<WeightSet>();
                }
                Group2.Clear();
                foreach (var item3 in queryLastNames)
                {
                    int cont = 0;
                    foreach (var item4 in item3)
                    {
                        if (cont == 0)
                        {
                            Group2.Add(item4);
                        }
                        cont = cont + 1;

                    }
                }


                PieceOfEquipment.WeightSets = lstWeightSet;
                ChangeEventArgs arg = new ChangeEventArgs();

                arg.Value = PieceOfEquipment;

                await ChangeAdd(arg);


                StateHasChanged();
            }
            else
            {
                throw new Exception("No weights configured");
            }



        }

       



    }
}

using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Helper;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Order;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using Reports.Domain.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.PressureGage
{
    public class SelectStandard : LoadComponentBase, ISelectItems<WorkOrderDetail>
    {

      

        public CreateModel Parameter { get; set; }
        public Func<dynamic, IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }

        public async Task<SelectList> SelectItems(WorkOrderDetail eq, int CalibrationSubTypeId, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2> item,
            int Position, string Map = ""   )
        {


            SelectList SelectList = new SelectList();

            var json = item.TestPointResult[0].Object;


            dynamic row = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
           

            var _poeGrpc = new PieceOfEquipmentGRPC(PoeServices,DbFactory);


            var WeightSetList2 = new List<WeightSet>();

            if (eq.WOD_Weights == null)
            {
                throw new Exception("Standards Not Found");
            }


            foreach (var itemr in eq.WOD_Weights)
            {
                WeightSetList2.Add(itemr.WeightSet);
            }

     
            var SelWeightSetList = new List<WeightSet>();

            if (item?.Standards?.Count > 0)
            {
                foreach (var item1 in item.Standards)
                {

                    WeightSet wsi = new WeightSet();

                    wsi.PieceOfEquipmentID = item1.PieceOfEquipmentID;

                    var item2 = WeightSetList2.Where(x => x.PieceOfEquipmentID == item1.PieceOfEquipmentID).FirstOrDefault();

                    wsi.WeightNominalValue = item2.WeightNominalValue;
                    wsi.UnitOfMeasureID = item2.UnitOfMeasureID;
                    wsi.WeightSetID = item2.WeightSetID;

                    SelWeightSetList.Add(wsi);
                }
            }



            if (item.WeightSets != null)
            {
                foreach (var iw in item.WeightSets)
                {


                    SelWeightSetList.Add(iw);


                }
            }

            SelectList.SetList = WeightSetList2;
            SelectList.SelecList = SelWeightSetList;
            SelectList.Result = null;// resultado.ToList();

            return SelectList;
        }

        public async Task StandardTask(WorkOrderDetail eq, WeightSetResult r, List<PieceOfEquipment> resultado, 
            IGenericCalibrationSubTypeCollection<GenericCalibrationResult2> itemParent, 
            List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>> Items, int position, string Map = ""  )
        {


            //WeightSetResult r = (WeightSetResult)result.Data;

            List<WeightSet> WeightSetList = (List<WeightSet>)r.WeightSetList;

            List<WeightSet> RemoveList = (List<WeightSet>)r.RemoveList;

            double CalResult = r.CalculateWeight;

            Console.WriteLine("select 2");


            dynamic item = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(itemParent.TestPointResult[0].Object);


            //dynamic item = null;

            item.AsReceived = CalResult;

            if (WeightSetList?.Count > 0 && resultado != null && resultado.Count > 0)
            {
                var poeselect = resultado.Where(x => x.PieceOfEquipmentID == WeightSetList[0].PieceOfEquipmentID).FirstOrDefault();


                double max = 0;
                double min = 0;
              
                if (itemParent.Standards == null)
                {
                    itemParent.Standards = new List<CalibrationSubType_Standard>();
                }
                itemParent.Standards.Clear();

                CalibrationSubType_Standard cs = new CalibrationSubType_Standard();
                cs.CalibrationSubTypeID = itemParent.CalibrationSubTypeId;
                cs.PieceOfEquipmentID = poeselect.PieceOfEquipmentID;
                cs.WorkOrderDetailID = itemParent.WorkOrderDetailId;
                cs.SecuenceID = itemParent.SequenceID;
                itemParent.Standards.Add(cs);


            }
            else
            {
              
            }

            if (CalResult == 0)
            {
                foreach (var item23 in Items)
                {
                    if (item23.WeightSets != null)
                    {
                        item23.WeightSets.Clear();
                    }

                }
            }

            if (WeightSetList != null)
            {
                Calculate cal = new Calculate();

                Console.WriteLine("select 4");
                //return;

                int cont = position;//RT.CurrentSelectIndex;

                if (cont < 0)
                {
                    cont = 0;
                }

                //for (int i = cont; i < RT.Items.Count; i++)
                //{
                int i = cont;

                if (Items[i].WeightSets == null)
                {
                    Items[i].WeightSets = new List<WeightSet>();
                }
                foreach (var ws in WeightSetList)
                {
                    var result11 = Items[i].WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                    if (result11 == null)
                    {
                        Console.WriteLine("select 44");
                        Items[i].WeightSets.Add(ws);
                    }


                }
                //}

            }

            if (RemoveList != null)
            {
                Console.WriteLine("select 3");
                foreach (var ws in RemoveList)
                {
                    var result11 = itemParent.WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                    if (result11 != null)
                    {
                        Console.WriteLine("select 33");
                        itemParent.WeightSets.Remove(result11);
                    }
                }
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(item);

            itemParent.TestPointResult[0].Object = json;

        }
    }
}

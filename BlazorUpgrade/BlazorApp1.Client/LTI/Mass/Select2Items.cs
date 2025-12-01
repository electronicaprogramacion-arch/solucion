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

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Mass
{
    public class Select2Standard : LoadComponentBase, ISelectItems<WorkOrderDetail>
    {

        
        public CreateModel Parameter { get ; set ; }
        public Func<dynamic, IPieceOfEquipmentService<CallContext>> PoeServices { get ; set ; }

        public async Task<SelectList> SelectItems(WorkOrderDetail eq, int CalibrationSubTypeId, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2> item, int Position, string Map = "" )
        {


            SelectList SelectList = new SelectList();

            var json = item.TestPointResult[0].Object;


            dynamic row = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
            //dynamic row= null;  

            var _poeGrpc = new PieceOfEquipmentGRPC(PoeServices,DbFactory);

            //var g = new CallOptions(await GetHeaderAsync());

            //var ss = item as RockwellResult;
            //if(row.ScaleRange == null)
            //{
            //    throw new Exception("Scale range not defined");

            //}

            //string sr = row.ScaleRange;

            //var resultado = await _poeGrpc.GetPieceOfEquipmentByScale(sr);


            //if(resultado == null  || resultado.Count() ==0)
            //{
            //    throw new Exception("Standards Not Found");  
            //}


            var WeightSetList2 = new List<WeightSet>();


            //int contu = 1;
            //foreach (var item1 in resultado)
            //{
            //    WeightSet wsi = new WeightSet();

            //    wsi.PieceOfEquipmentID = item1.PieceOfEquipmentID;
            //    wsi.WeightNominalValue = item1.Capacity;
            //    wsi.UnitOfMeasureID = item1.UnitOfMeasureID.Value;
            //    if (wsi.WeightSetID == 0)
            //    {
            //        wsi.WeightSetID = contu;
            //        contu++;
            //    }

            //    WeightSetList2.Add(wsi);


            //}
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (eq.WOD_Weights == null)
            {
                throw new Exception("Standards Not Found");
            }


            foreach (var itemr in eq.WOD_Weights.Where(x => x.WeightSet.Option == "29"))
            {
                WeightSetList2.Add(itemr.WeightSet);
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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




            //if (item.WeightSets != null)
            //{
            //    foreach (var iw in item.WeightSets)
            //    {


            //        SelWeightSetList.Add(iw);


            //    }
            //}

            SelectList.SetList = WeightSetList2;
            SelectList.SelecList = SelWeightSetList;
            SelectList.Result = null;// resultado.ToList();

            return SelectList;
        }

        public async Task StandardTask(WorkOrderDetail eq, WeightSetResult r, List<PieceOfEquipment> resultado, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2> itemParent, List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>> Items, int position, string Map = ""  )
        {


            //WeightSetResult r = (WeightSetResult)result.Data;

            List<WeightSet> WeightSetList = null;

            if(r.WeightSetList != null)
            {
                WeightSetList = r.WeightSetList.Where(x=>x.Option=="29").ToList();
            }

            List<WeightSet> RemoveList = null;
            if (r.RemoveList != null)
            {
                RemoveList = r.RemoveList.Where(x => x.Option == "29").ToList();
            }


            //List<WeightSet> RemoveList = (List<WeightSet>)r.RemoveList;

            double CalResult = r.CalculateWeight;

            Console.WriteLine("select 2");


            dynamic item = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(itemParent.TestPointResult[0].Object);


            //dynamic item = null;

            //item.AsReceived = CalResult;
            //&& resultado != null && resultado.Count > 0
            if (WeightSetList?.Count > 0 )
            {
                PieceOfEquipment poeselect = null;
                if (resultado != null && resultado.Count > 0)
                {
                    poeselect  = resultado.Where(x => x.PieceOfEquipmentID == WeightSetList[0].PieceOfEquipmentID).FirstOrDefault();
                }
               


                double max = 0;
                double min = 0;
               


                //Only one
                if (itemParent.Standards == null)
                {
                    itemParent.Standards = new List<CalibrationSubType_Standard>();
                }
                itemParent.Standards.Clear();

                CalibrationSubType_Standard cs = new CalibrationSubType_Standard();
                cs.CalibrationSubTypeID = itemParent.CalibrationSubTypeId;
                if(poeselect != null) {

                    cs.PieceOfEquipmentID = poeselect.PieceOfEquipmentID;
                }
                else
                {
                    cs.PieceOfEquipmentID = WeightSetList[0].PieceOfEquipmentID;
                }
               
                cs.WorkOrderDetailID = itemParent.WorkOrderDetailId;
                cs.SecuenceID = itemParent.SequenceID;
                itemParent.Standards.Add(cs);


            }
            else
            {
                //item.Max = 0;

                //item.Min = 0;

                //item.Uncertanty = 0;
                //Only one
            }

            if (CalResult == 0)
            {
                //foreach (var item23 in Items)
                //{
                //    if (item23.WeightSets != null)
                //    {
                //        item23.WeightSets.Clear();
                //    }

                //}

                Console.WriteLine("zero standar value");

            }
            //Load weighsets
            if (WeightSetList != null  && 1==2)
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
                if(Items!= null && Items.Count > 0)
                {
                    if (Items[i].WeightSets == null)
                    {
                        Items[i].WeightSets = new List<WeightSet>();
                    }
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
                foreach (var ws in RemoveList.Where(x=>x.Option=="29"))
                {
                    var result11 = itemParent.Standards.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID).FirstOrDefault();
                    if (result11 != null)
                    {
                        Console.WriteLine("select 33");
                        itemParent.Standards.Remove(result11);
                    }
                }
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(item);

            itemParent.TestPointResult[0].Object = json;

        }
    }
}

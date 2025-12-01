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

namespace CalibrationSaaS.Infraestructure.Blazor.Reticles
{
    public class SelectStandard : LoadComponentBase, ISelectItems<WorkOrderDetail>
    {

      

        public CreateModel Parameter { get; set; }

        public int StandardType { get; set; }


        public int StandardFormMap { get; set; }


        public int TestPointMap { get; set; }

        public string Option { get; set; }


        public bool MultiSelect { get; set; }
        public Func<dynamic, IPieceOfEquipmentService<CallContext>> PoeServices { get; set; }

        public async Task<SelectList> SelectItems(WorkOrderDetail eq, int CalibrationSubTypeId, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2> item, 
            int Position, string Map = "")
        {


            SelectList SelectList = new SelectList();

            var json = item.TestPointResult[0].Object;


            dynamic row = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
            

            var _poeGrpc = new PieceOfEquipmentGRPC(PoeServices,DbFactory);

           

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

            if (string.IsNullOrEmpty(Option))
            {
                foreach (var itemr in eq.WOD_Weights)
                {
                    WeightSetList2.Add(itemr.WeightSet);
                }
            }
            else
            {
                foreach (var itemr in eq.WOD_Weights.Where(x => x.WeightSet.Option == Option))
                {
                    WeightSetList2.Add(itemr.WeightSet);
                }
            }

        

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var SelWeightSetList = new List<WeightSet>();

            if (item?.Standards?.Count > 0 )
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



        /// <summary>
        /// this code is for standards
        /// </summary>
        /// <param name="eq"></param>
        /// <param name="r"></param>
        /// <param name="resultado"></param>
        /// <param name="itemParent"></param>
        /// <param name="Items"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public async Task StandardTask(WorkOrderDetail eq, WeightSetResult r, List<PieceOfEquipment> resultado, 
            IGenericCalibrationSubTypeCollection<GenericCalibrationResult2> itemParent, 
            List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>> Items, int position, string Map = "")
        {




            List<WeightSet> WeightSetList = null;

            if (r.WeightSetList != null)
            {

                if (string.IsNullOrEmpty(Option))
                {
                    WeightSetList = r.WeightSetList.ToList();
                }
                else
                {
                    WeightSetList = r.WeightSetList.Where(x => x.Option == Option).ToList();
                }
                    
            }

            List<WeightSet> RemoveList = null;
            if (r.RemoveList != null)
            {
                if (string.IsNullOrEmpty(Option))
                {
                    RemoveList = r.RemoveList.ToList();

                }
                else
                {
                    RemoveList = r.RemoveList.Where(x => x.Option == Option).ToList();
                }

                   
            }
            double CalResult = r.CalculateWeight;

            Console.WriteLine("select 2");


            dynamic item = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(itemParent.TestPointResult[0].Object);


            //var tol = res.GetPropValue("Class" + item.Class);


            //dynamic item = null;

            //item.AsReceived = CalResult;

            if (WeightSetList?.Count > 0 && resultado != null && resultado.Count > 0)
            {
                var poeselect = resultado.Where(x => x.PieceOfEquipmentID == WeightSetList[0].PieceOfEquipmentID).FirstOrDefault();


                //Only one
                if (itemParent.Standards == null)
                {
                    itemParent.Standards = new List<CalibrationSubType_Standard>();
                }

                if (!MultiSelect)
                {
                    itemParent.Standards.Clear();
                }
                

                CalibrationSubType_Standard cs = new CalibrationSubType_Standard();
                cs.CalibrationSubTypeID = itemParent.CalibrationSubTypeId;
                cs.PieceOfEquipmentID = poeselect.PieceOfEquipmentID;
                cs.WorkOrderDetailID = itemParent.WorkOrderDetailId;
                cs.SecuenceID = itemParent.SequenceID;
                cs.ComponentID= itemParent.WorkOrderDetailId.ToString();
                cs.Component = "WorkOrderItem";
                itemParent.Standards.Add(cs);


            }
            else
            {
                //item.Max = 0;

                //item.Min = 0;

                //item.Uncertanty = 0;
                //Only one
            }


            if (string.IsNullOrEmpty(Option))
            {


                if (WeightSetList?.Count > 0 && resultado == null)
                {
                    if (itemParent.WeightSets == null)
                    {
                        itemParent.WeightSets = new List<WeightSet>();
                    }
                    foreach (var item24 in WeightSetList)
                    {
                        itemParent.WeightSets.Add(item24);
                    }


                }

            }
            else
            {
                if (WeightSetList?.Where(x=>x.Option==Option).Count() > 0 && resultado == null)
                {
                    if (itemParent.WeightSets == null)
                    {
                        itemParent.WeightSets = new List<WeightSet>();
                    }
                    foreach (var item24 in WeightSetList)
                    {
                        itemParent.WeightSets.Add(item24);
                    }


                }

            }


            if (RemoveList != null)
            {
                Console.WriteLine("select 3");


                if (string.IsNullOrEmpty(Option))
                {
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
                else
                {

                    foreach (var ws in RemoveList.Where(x => x.Option == "28"))
                    {
                        var result11 = itemParent.WeightSets.Where(x => x.PieceOfEquipmentID == ws.PieceOfEquipmentID && x.WeightSetID == ws.WeightSetID).FirstOrDefault();
                        if (result11 != null)
                        {
                            Console.WriteLine("select 33");
                            itemParent.WeightSets.Remove(result11);
                        }
                    }
                }
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(item);

            itemParent.TestPointResult[0].Object = json;

        }
    }
}

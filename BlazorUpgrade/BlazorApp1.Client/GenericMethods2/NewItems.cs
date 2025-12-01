using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class NewItems : ComponentBase, INewItem<WorkOrderDetail,GenericCalibration2>
    {
        public CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public CreateModel Parameter { get ; set ; }
        public AppState AppState { get; set; }
        public async Task<List<GenericCalibration2>> DefaultNewItem(WorkOrderDetail eq, int CalibrationSubTypeId)
        {
            var uom = eq.PieceOfEquipment.UnitOfMeasure;

            List<GenericCalibration2> list = new List<GenericCalibration2>();
            GenericCalibration2 cal = new GenericCalibration2();

            //cal.SequenceID = 1;

            //if (string.IsNullOrEmpty(eq.SerialNumber))
            //{
            //    throw new Exception("Serial POE is Empty");
            //}
            //cal.ComponentID = eq.PieceOfEquipmentID;

            //cal.CalibrationSubTypeId = CalibrationSubTypeId;


            dynamic gce = new ExpandoObject();
            GenericCalibrationResult2 gcr = new GenericCalibrationResult2();
            //gce.StandardPressure = item;
            //int cont = eq.BalanceAndScaleCalibration.GenericCalibration2.Where(x=>x.CalibrationSubTypeId == CalibrationSubTypeId).Count()+2;

          // cal.BasicCalibrationResult = gcr.CreateNew(cont, CalibrationSubTypeId, "WorkOrderItem", eq.WorkOrderDetailID.ToString(), eq.Resolution, gce);
          cal.TestPointResult = NewMethod(eq, CalibrationSubTypeId, uom.Abbreviation);

            list.Add(cal);

            return list;



        }

        private static List<GenericCalibrationResult2> NewMethod(WorkOrderDetail eq,int CalibrationSubTypeId, string UoM)
        {
            var list = new List<GenericCalibrationResult2>();
            int cont = 0;
            var gen = new GenericCalibrationResult2();
            if (eq.BalanceAndScaleCalibration.TestPointResult !=null)
             cont =  eq.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId && x.ComponentID == eq.WorkOrderDetailID.ToString()).Count() + 1; //eq.BalanceAndScaleCalibration.GenericCalibration2.Count() + 2;
            //gen.SequenceID = 1;

            //gen.ComponentID = eq.PieceOfEquipmentID;

            gen.Component = "WorkOrderItem";

            gen.CalibrationSubTypeId= CalibrationSubTypeId;

            gen.Position = cont;
            gen.SequenceID = cont;
            
            dynamic gce = new ExpandoObject();


            //gce.IN = eq.PieceOfEquipmentID;
            //gce.SN = eq.SerialNumber;

            gce.UoM = UoM;
            gce.ResultAsFound = "Fail";
            gce.ResultAsLeft = "Fail";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(gce);

            gen.Object = json;


            var s = gen as IResult2;

            list.Add(gen);

            return list ;
        }
    }
}

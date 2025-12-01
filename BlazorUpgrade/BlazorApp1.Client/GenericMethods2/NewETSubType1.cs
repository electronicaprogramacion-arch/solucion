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
    public class NewPOESubtype1 : ComponentBase, INewItem<PieceOfEquipment,GenericCalibration2>
    {
        public Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }
        public CreateModel Parameter { get ; set ; }
        public AppState AppState { get; set; }
        public async Task<List<GenericCalibration2>> DefaultNewItem(PieceOfEquipment eq, int CalibrationSubTypeId)
        {

            Parameter.Data = new List<Dictionary<string, object>>();

            List<GenericCalibrationResult2> tespo = null;
            if (Parameter.CalibrationSubtypeID.HasValue)
            {
                tespo = eq.EquipmentTemplate.TestPointResult.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value).ToList();
            }
            else
            {
                tespo = eq.EquipmentTemplate.TestPointResult;
            }


            var tespo2 = eq.EquipmentTemplate.TestPointResult.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID2.Value).FirstOrDefault();


            if (tespo2 == null)
            {
                throw new Exception("LT and UT not configured");
            }

            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(tespo2.Object);


            var lt = Model.LowRange;

            var ut = Model.HighRange;


            CalibrationSaaS.Infraestructure.Blazor.GenericMethods.CreateBase<PieceOfEquipment,GenericCalibration2,GenericCalibrationResult2> 
                c = new CalibrationSaaS.Infraestructure.Blazor.GenericMethods.CreateBase<PieceOfEquipment, GenericCalibration2, GenericCalibrationResult2>();

            Parameter.NumberTestpoints = 1.ToString();

            c.Parameter = Parameter;
            var result = await c.CreateItems(eq, CalibrationSubTypeId);





            if (result != null && result.Count > 0)
            {
                int cont = 0;
                foreach (var item in result)
                {

                    dynamic objparent = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(tespo[cont].Object);

                    dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(item.TestPointResult.FirstOrDefault().Object);

                    obj.LT = lt;
                    obj.UT = ut;
                    //obj.SetPoint = objparent.SetPoint;
                    var obj2 = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                    item.TestPointResult[0].Object = obj2;

                    cont++;
                }
            }



            return result;




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

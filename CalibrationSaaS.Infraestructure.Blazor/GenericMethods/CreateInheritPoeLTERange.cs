using Bogus;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.Aggregates.Querys;
namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class CreateInheritPoeSubtype1 : CreateBase<WorkOrderDetail, GenericCalibration2,GenericCalibrationResult2>, ICreateItems<WorkOrderDetail, GenericCalibration2>
    {



        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)
        {
            Parameter.Data = new List<Dictionary<string, object>>();

            List<GenericCalibrationResult2> tespo = null;
            if (Parameter.CalibrationSubtypeID.HasValue)
            {
                tespo = eq.PieceOfEquipment.TestPointResult.Where(x => !string.IsNullOrEmpty(GroupName) && x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value && x.GroupName==GroupName).ToList();
            }
            else
            {
                tespo = eq.PieceOfEquipment.TestPointResult;
            }


            var tespo2 = eq.PieceOfEquipment.TestPointResult.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID2.Value).FirstOrDefault();


            if(tespo2 == null)
            {
                throw new Exception("LT and UT not configured");
            }

            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(tespo2.Object);


            var lt = Model.LowRange;

            var ut = Model.HighRange;

            var uom = Model.UoM;
          

            Parameter.NumberTestpoints = tespo.Count.ToString();
            var result = await base.CreateItems(eq, CalibrationSubTypeId, GroupName);





            if (result != null && result.Count > 0)
            {
                int cont = 0;
                foreach (var item in result)
                {

                    dynamic objparent = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(tespo[cont].Object);

                    dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(item.TestPointResult.FirstOrDefault().Object);

                    obj.LT = lt;
                    obj.UT = ut;
                    obj.SetPoint = objparent.SetPoint;
                    obj.UoM = objparent.UoM;
                    var obj2 = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                    item.TestPointResult[0].Object = obj2;

                    cont++;
                }
            }



            return result;




        }




    }
}

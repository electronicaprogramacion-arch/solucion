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
    public class CreateInheritPoeWithHeader : CreateBase<WorkOrderDetail, GenericCalibration2,GenericCalibrationResult2>, ICreateItems<WorkOrderDetail, GenericCalibration2>
    {



        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)
        {
            Parameter.Data = new List<Dictionary<string, object>>();

            List<GenericCalibrationResult2> tespo = null;
            List<GenericCalibrationResult2> tespoHeader = null;
            if (Parameter.CalibrationSubtypeID.HasValue)
            {
                tespo = eq.PieceOfEquipment.TestPointResult.Where(x => !string.IsNullOrEmpty(GroupName) && x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value && x.GroupName==GroupName && x.Position > 0).ToList();
            }
            else
            {
                tespo = eq.PieceOfEquipment.TestPointResult?.Where(x=> x.Position > 0).ToList();
            }
            if (Parameter.CalibrationSubtypeID.HasValue)
            {
                tespoHeader = eq.PieceOfEquipment.TestPointResult.Where(x => !string.IsNullOrEmpty(GroupName) && x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value && x.GroupName == GroupName && x.Position == -1).ToList();
            }
            else
            {
                tespoHeader = eq.PieceOfEquipment.TestPointResult?.Where(x => x.Position == -1).ToList();
            }

            var tespo2 = eq.PieceOfEquipment.TestPointResult.Where(x => Parameter.CalibrationSubtypeID2 .HasValue && x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID2.Value).FirstOrDefault();

            if (tespoHeader?.Count > 0)
            {
                if (Parameter.Header == null)
                {
                    Parameter.Header = new Header();

                }

                foreach (var item in tespoHeader)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic.Add("genericcalibrationresult2", item);

                    Parameter.Header.Data = (dic);
                }
                Parameter.NumberTestpoints = "";
                Parameter.Property = "";
            }
            else
            {

            }

            if (tespo2 == null)
            {
                throw new Exception("LT and UT not configured");
            }

            var Model = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(tespo2.Object);


            var lt = Model.LowRange;

            var ut = Model.HighRange;

            //var uom = Model.UoM;
          

            Parameter.NumberTestpoints = tespo.Count.ToString();

            var result2 = await base.CreateItems(eq, CalibrationSubTypeId, GroupName);


            var result = result2.Where(x =>  x.TestPointResult != null
            && x.TestPointResult.Where(x => x.Position > 0).Count() > 0).ToList();

            var resultHeader = result2.Where(x => x.TestPointResult != null
            && x.TestPointResult.Where(x => x.Position == -1).Count() > 0).ToList();


            if (result != null && result.Count > 0 && tespo != null )
            {
                int cont = 0;
                foreach (var item in result)
                {

                    dynamic objparent = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(tespo[cont].Object);

                    dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(item.TestPointResult.FirstOrDefault().Object);

                   

                    obj.LT = lt;
                    obj.UT = ut;
                    obj.Nominal = objparent.SetPoint;
                    obj.UoM = objparent.UoM;
                   
                    var obj2 = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                    item.TestPointResult[0].Object = obj2;

                    cont++;
                }
            }


            if (resultHeader != null && resultHeader.Count > 0 && tespoHeader != null && tespoHeader.Count > 0)
            {
                int cont = 0;
                foreach (var item in resultHeader)
                {

                    dynamic objparent = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(tespoHeader[cont].Object);

                    dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(item.TestPointResult.FirstOrDefault().Object);



                    obj.UnitOfMeasureType = objparent.UnitOfMeasureType;
                    obj.ToleranceRow = objparent.ToleranceRow;

                    //obj.UoM = objparent.UoM;
                    var obj2 = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                    item.TestPointResult[0].Object = obj2;

                    cont++;
                }
            }

            if (resultHeader != null)
            {
                result.AddRange(resultHeader);
            }

           

            return result;




        }




    }
}

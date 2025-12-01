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
    public class CreateInheritPoe : CreateBase<WorkOrderDetail, GenericCalibration2,GenericCalibrationResult2>, ICreateItems<WorkOrderDetail, GenericCalibration2>
    {

        public async Task<List<GenericCalibration2>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId, string GroupName = null)
        {
            Parameter.Data = new List<Dictionary<string, object>>();

            List<GenericCalibrationResult2> tespo = null;


            List<GenericCalibrationResult2> tespo2 = eq?.PieceOfEquipment?.TestPointResult;

            if (tespo2 == null || tespo2?.Count==0)
            {
                tespo2= eq?.PieceOfEquipment.EquipmentTemplate?.TestPointResult;    
            }

            if (GroupName == null && Parameter.CalibrationSubtypeID.HasValue)
            {
                tespo = tespo2?.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value).ToList();
            }
            else if (GroupName != null && Parameter.CalibrationSubtypeID.HasValue)
            {
                tespo = tespo2?.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value && x.GroupName == GroupName).ToList();
            }
            else if (GroupName != null)
            {
                tespo = tespo2.Where(x => x.GroupName == GroupName).ToList();
            }
            else

            if (Parameter.CalibrationSubtypeID.HasValue)
            {
                tespo = tespo2.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value).ToList();
            }
            else
            {
                tespo = tespo2;
            }

            if (tespo?.Count > 0)
            {

                foreach (var item in tespo)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic.Add("genericcalibrationresult2", item);

                    Parameter.Data.Add(dic);
                }
                Parameter.NumberTestpoints = "";
                Parameter.Property = "";

            }

           


            return await base.CreateItems(eq, CalibrationSubTypeId);




        }




    }
}

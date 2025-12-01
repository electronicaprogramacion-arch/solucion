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
    public class CreateInheritET : CreateBase<PieceOfEquipment, GenericCalibration2, GenericCalibrationResult2>, ICreateItems<PieceOfEquipment, GenericCalibration2>//ComponentBase, ICreateItems<PieceOfEquipment,Generic
    {
       
        public async Task<List<GenericCalibration2>> CreateItems(PieceOfEquipment eq, int CalibrationSubTypeId, string GroupName = null)
        {
            Parameter.Data = new List<Dictionary<string, object>>();
            List<GenericCalibrationResult2> tespo = null;
            if (Parameter.CalibrationSubtypeID.HasValue && GroupName== null)
            {
                tespo = eq.EquipmentTemplate?.TestPointResult?.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value).ToList();
            }
            else if (Parameter.CalibrationSubtypeID.HasValue && GroupName != null)
            {
                tespo = eq.EquipmentTemplate?.TestPointResult?.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value && x.GroupName == GroupName).ToList();
            }
            else if (GroupName != null)
            {
                tespo = eq.EquipmentTemplate?.TestPointResult?.Where(x => x.GroupName == GroupName).ToList();
            }
            else
            {
                tespo = eq.EquipmentTemplate.TestPointResult;
            }

            if(tespo?.Count > 0)
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
            else
            {
                
            }


            return await base.CreateItems(eq, CalibrationSubTypeId);

         


        }



    }
}

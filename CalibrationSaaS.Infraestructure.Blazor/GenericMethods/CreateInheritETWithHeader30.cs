using Bogus;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Helpers;
using Helpers.Models;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class CreateInheritETWithHeader30 : CreateBase<PieceOfEquipment, GenericCalibration2, GenericCalibrationResult2>, ICreateItems<PieceOfEquipment, GenericCalibration2>//ComponentBase, ICreateItems<PieceOfEquipment,Generic
    {

        public async Task<List<GenericCalibration2>> CreateItems(PieceOfEquipment eq, int CalibrationSubTypeId, string GroupName = null)
        {
            Parameter.Data = new List<Dictionary<string, object>>();
            List<GenericCalibrationResult2> tespo = null;
            List<GenericCalibrationResult2> tespoHeader = null;
            if (Parameter.CalibrationSubtypeID.HasValue && GroupName == null )
            {
                tespo = eq.EquipmentTemplate?.TestPointResult?.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value && x.Position > 0).ToList();
            }
            else if (Parameter.CalibrationSubtypeID.HasValue && GroupName != null)
            {
                tespo = eq.EquipmentTemplate?.TestPointResult?.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value && x.GroupName == GroupName && x.Position > 0).ToList();
            }
            else if (GroupName != null)
            {
                tespo = eq.EquipmentTemplate?.TestPointResult?.Where(x => x.GroupName == GroupName && x.Position > 0).ToList();
            }
            else
            {
                tespo = eq.EquipmentTemplate.TestPointResult.Where(x => x.Position > 0).ToList();
            }


            if (Parameter.CalibrationSubtypeID.HasValue && GroupName == null)
            {
                tespoHeader = eq.EquipmentTemplate?.TestPointResult?.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value && x.Position == -1).ToList();
            }
            else if (Parameter.CalibrationSubtypeID.HasValue && GroupName != null)
            {
                tespoHeader = eq.EquipmentTemplate?.TestPointResult?.Where(x => x.CalibrationSubTypeId == Parameter.CalibrationSubtypeID.Value && x.GroupName == GroupName && x.Position == -1).ToList();
            }
            else if (GroupName != null)
            {
                tespoHeader = eq.EquipmentTemplate?.TestPointResult?.Where(x => x.GroupName == GroupName && x.Position == -1).ToList();
            }
            else
            {
                tespoHeader = eq.EquipmentTemplate.TestPointResult.Where(x => x.Position == -1).ToList();
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
            else
            {

            }

            if (tespoHeader?.Count > 0)
            {
                if(Parameter.Header == null)
                {
                    Parameter.Header= new Header();

                }

                foreach (var item in tespoHeader)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic.Add("genericcalibrationresult2", item);

                    Parameter.Header.Data= Helpers.JsonToDictionaryDeserializer.DeserializeWithNewtonsoft(item.Object);
                }
                Parameter.NumberTestpoints = "";
                Parameter.Property = "";
            }
            else
            {

            }


            var res= await base.CreateItems(eq, CalibrationSubTypeId);

            return res;

         


        }



    }
}

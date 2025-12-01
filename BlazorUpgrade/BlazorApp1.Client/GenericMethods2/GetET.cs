using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods.GetET
{
    public class Get : GetBase<EquipmentTemplate, GenericCalibration2>, IGetItems<EquipmentTemplate, GenericCalibration2>
    {
        public async Task<List<GenericCalibration2>> GetItems(EquipmentTemplate eq, int CalibrationSubTypeId, int ihneritCalSubtypeId)
        {

            var result =await base.GetItems(eq, CalibrationSubTypeId, ihneritCalSubtypeId);

            return result;




            if (eq == null
                || eq.TestPointResult == null || eq.TestPointResult.Count == 0
                || eq.TestPointResult.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).FirstOrDefault() == null)
            {
                return null;
            }



            var list = eq.TestPointResult
                 .Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).ToList();


            List<GenericCalibration2> ListGC3 = new List<GenericCalibration2>();
            foreach (var itemGC in list)
            {
                if (itemGC.GenericCalibration2 == null)
                {

                    ListGC3.Add(itemGC.CreateNew(itemGC));
                }
                else
                {
                    //if(itemGC.GenericCalibration2.BasicCalibrationResult == null)
                    //{
                    itemGC.GenericCalibration2.TestPointResult = new List<GenericCalibrationResult2>();
                    //}

                    itemGC.GenericCalibration2.TestPointResult.Add(itemGC);

                    ListGC3.Add(itemGC.GenericCalibration2);
                }
                //itemGC.GenericCalibration2.BasicCalibrationResult.Add(itemGC);


            }
            return ListGC3;
        }

    }
}

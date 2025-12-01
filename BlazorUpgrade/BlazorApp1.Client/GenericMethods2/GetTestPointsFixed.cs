using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class GetTestPointsFixed : ComponentBase, IGetItems<WorkOrderDetail, GenericCalibration2>
    {
        public async Task<List<GenericCalibration2>> GetItems(WorkOrderDetail eq, int CalibrationSubTypeId, int ihneritCalSubtypeId)
        {

            if (eq == null || eq.GetCalibrationTypeID() == null || eq.BalanceAndScaleCalibration == null
                || eq.BalanceAndScaleCalibration.TestPointResult == null || eq.BalanceAndScaleCalibration.TestPointResult.Count == 0
                || eq.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).FirstOrDefault() == null)
            {
                return null;
            }



            var list = eq.BalanceAndScaleCalibration.TestPointResult
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

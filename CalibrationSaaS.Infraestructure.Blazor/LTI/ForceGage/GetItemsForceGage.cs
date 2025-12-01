using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.ForceGage
{
    public class GetItemsForceGage : ComponentBase, IGetItems
    {
        public async Task<List<GenericCalibration>> GetItems(WorkOrderDetail eq, int CalibrationSubTypeId)
        {

            if (eq == null || eq.GetCalibrationTypeID()==null || eq.BalanceAndScaleCalibration == null
               || eq.BalanceAndScaleCalibration.GenericCalibration == null || eq.BalanceAndScaleCalibration.GenericCalibration.Count == 0 
               || eq.BalanceAndScaleCalibration.GenericCalibration.Where(x=>x.CalibrationSubTypeId== CalibrationSubTypeId).FirstOrDefault()==null)
            {
                return null;
            }
                       


            var list = eq.BalanceAndScaleCalibration.GenericCalibration
                 .Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).ToList();


            

            return list;
        }

       
    }
}

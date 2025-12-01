using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Rockwell
{
    public class GetItemsAsFoundISO : ComponentBase, IGetItems
    {
        public async Task<List<GenericCalibration>> GetItems(WorkOrderDetail eq, int CalibrationSubTypeId)
        {

            if (eq == null || !eq.CalibrationTypeID.HasValue || eq.BalanceAndScaleCalibration == null
               || eq.BalanceAndScaleCalibration.GenericCalibration == null || eq.BalanceAndScaleCalibration.GenericCalibration.Count == 0)
            {
                return null;
            }


            var tableList = eq.BalanceAndScaleCalibration.GenericCalibration.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).ToList();



            return tableList;
        }

    }
}

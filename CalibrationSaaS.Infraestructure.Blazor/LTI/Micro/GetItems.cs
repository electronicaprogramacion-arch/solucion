using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Micro
{
    public class GetItemsMicro : ComponentBase, IGetItems
    {
        public async Task<List<GenericCalibration>> GetItems(WorkOrderDetail eq, int CalibrationSubTypeId)
        {
            var list = eq.BalanceAndScaleCalibration.GenericCalibration
                 .Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).ToList();
        
                return list;    
        }

    }
}

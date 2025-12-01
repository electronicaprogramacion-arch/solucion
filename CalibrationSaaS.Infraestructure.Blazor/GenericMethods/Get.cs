using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class Get : GetBase<WorkOrderDetail, GenericCalibration2>, IGetItems<WorkOrderDetail, GenericCalibration2>
    {
        public async Task<List<GenericCalibration2>> GetItems(WorkOrderDetail eq, int CalibrationSubTypeId,int ihneritCalSubtypeId)
        {


            return await base.GetItems(eq, CalibrationSubTypeId, ihneritCalSubtypeId);

          
        }

    }
}

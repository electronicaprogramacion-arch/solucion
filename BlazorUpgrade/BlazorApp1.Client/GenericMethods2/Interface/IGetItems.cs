using CalibrationSaaS.Domain.Aggregates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Aggregates.Interfaces
{
    public interface IGetItems
    {
        Task<List<GenericCalibration>> GetItems(WorkOrderDetail eq,int CalibrationSubTypeId);




    }
}
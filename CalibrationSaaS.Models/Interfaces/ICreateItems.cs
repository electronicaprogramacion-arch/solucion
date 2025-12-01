using CalibrationSaaS.Domain.Aggregates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Aggregates.Interfaces
{
    public interface ICreateItems
    {
        Task<List<GenericCalibration>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId);
    }
}
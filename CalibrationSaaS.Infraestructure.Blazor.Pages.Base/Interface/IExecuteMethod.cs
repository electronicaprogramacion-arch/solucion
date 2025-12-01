using CalibrationSaaS.Domain.Aggregates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces
{
    public interface IExecuteMethod
    {
        Task<List<GenericCalibration>> ExcuteMethod(List<GenericCalibration> GridData);
    }
}
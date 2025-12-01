using CalibrationSaaS.Domain.Aggregates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp1.Blazor.Blazor.Interfaces
{
    public interface IExecuteMethod
    {
        Task<List<GenericCalibration>> ExcuteMethod(List<GenericCalibration> GridData);
    }
}
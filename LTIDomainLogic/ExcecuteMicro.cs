
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
namespace LTIDomainLogic
{
    public class ExcecuteMicro : IExecuteMethod
    {
        public Task<List<GenericCalibration>> ExcuteMethod(List<GenericCalibration> GridData)
        {
            throw new NotImplementedException();
        }
    }
}
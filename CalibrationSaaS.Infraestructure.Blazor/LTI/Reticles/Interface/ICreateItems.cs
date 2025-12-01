using CalibrationSaaS.Domain.Aggregates.Entities;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Aggregates.Interfaces
{
    public interface ICreateItems
    {
         Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }

        Task<List<GenericCalibration>> CreateItems(WorkOrderDetail eq, int CalibrationSubTypeId);
    }
}
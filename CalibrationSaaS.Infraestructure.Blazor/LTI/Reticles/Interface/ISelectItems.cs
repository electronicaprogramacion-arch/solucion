using CalibrationSaaS.Domain.Aggregates.Entities;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Aggregates.Interfaces
{
    public interface ISelectItems
    {

        Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }


        Task<SelectList> SelectItems(WorkOrderDetail eq,int CalibrationSubTypeId,GenericCalibration item,int Position);

     
        Task StandardTask(WorkOrderDetail eq, WeightSetResult r, List<PieceOfEquipment> resultado, GenericCalibration itemParent, List<GenericCalibration> Items, int position);





    }
}
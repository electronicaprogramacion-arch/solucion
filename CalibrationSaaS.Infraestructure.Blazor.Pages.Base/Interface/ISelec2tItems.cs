using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces
{
    public interface ISelect2Items<Entity>
    {

        Application.Services.IPieceOfEquipmentService<CallContext> PoeServices { get; set; }


        Task<SelectList> SelectItems(Entity eq,int CalibrationSubTypeId, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2> item, int Position);


        Task StandardTask(Entity eq, WeightSetResult r, List<PieceOfEquipment> resultado, IGenericCalibrationSubTypeCollection<GenericCalibrationResult2> itemParent, List<IGenericCalibrationSubTypeCollection<GenericCalibrationResult2>> Items, int position);





    }
}